var viewModel = {
	robotState: ko.observable({ IsRunning: false, Sensors: [] }),
	portName: ko.observable(),
	velocity: ko.observable(100),
	increaseVelocity: function () {
		if (this.velocity() == 500) {
			return;
		}
	
		this.velocity(this.velocity() + 50);
	},
	decreaseVelocity: function () {
		if (this.velocity() == 0) {
			return;
		}
		this.velocity(this.velocity() - 50);
	},
	toggleRunning: function () {
		$.ajax({
			type: 'POST',
			url: this.robotState().IsRunning ? "/API/Stop" : "/API/Start",
			data: "portName=" + this.portName(),
			success: function () {

			},
			error: function (ex) {
				console.log(ex);
				alert("Server returned an error");
			}
		});
	},
	forward: function () {
		console.log("Forward");
		$.ajax({
			type: 'POST',
			url: "/API/Commands/DriveStraight",
			data: "velocity=" + this.velocity()
		})
	},
	forwardLeft: function () {
		console.log("Forward-Left");
		$.ajax({
			type: 'POST',
			url: "/API/Commands/Drive",
			data: "velocity=" + this.velocity() + "&radius=300"
		})
	},
	forwardRight: function () {
		console.log("Forward-Right");
		$.ajax({
			type: 'POST',
			url: "/API/Commands/Drive",
			data: "velocity=" + this.velocity() + "&radius=-300"
		})
	},
	backLeft: function () {
		console.log("Back-Left");
		$.ajax({
			type: 'POST',
			url: "/API/Commands/Drive",
			data: "velocity=-" + this.velocity() + "&radius=300"
		})
	},
	backRight: function () {
		console.log("Back-Right");
		$.ajax({
			type: 'POST',
			url: "/API/Commands/Drive",
			data: "velocity=-" + this.velocity() + "&radius=-300"
		})
	},
	turnInPlaceClockwise: function () {
		console.log("Clockwise");
		$.ajax({
			type: 'POST',
			url: "/API/Commands/TurnInPlaceClockwise",
			data: "velocity=" + this.velocity()
		})
	},
	turnInPlaceCounterClockwise: function () {
		console.log("Counter-Clockwise");
		$.ajax({
			type: 'POST',
			url: "/API/Commands/TurnInPlaceCounterClockwise",
			data: "velocity=" + this.velocity()
		})
	},
	stop: function () {
		console.log("Stop");
		$.ajax({
			type: 'POST',
			url: "/API/Commands/DriveStop"
		})
	},
	back: function () {
		console.log("Back");
		$.ajax({
			type: 'POST',
			url: "/API/Commands/DriveStraight",
			data: "velocity=-" + this.velocity() + ""
		})

	}
};
viewModel.isRunning = ko.dependentObservable(function () {
	return this.robotState().IsRunning;
}, viewModel);
viewModel.status = ko.dependentObservable(function () {
	return this.robotState().IsRunning ? "RUNNING" : "NOT RUNNING";
}, viewModel);
viewModel.nextRunningAction = ko.dependentObservable(function () {
	return this.robotState().IsRunning ? "Stop" : "Start";
}, viewModel);
viewModel.ports = ko.dependentObservable(function () {
	return this.robotState().Ports;
}, viewModel);
viewModel.batteryPercentage = ko.dependentObservable(function () {
	var result = "Unknown";
	this.robotState().Sensors.forEach(function(item) {
		if (item.Name == "BatteryCharge") {
			result = item.Percentage + "%";
		}
	});
	return result;
}, viewModel);
ko.applyBindings(viewModel);

var Key = {
	_pressed: {},
	
	counter: 0,

	LEFT: 37,
	UP: 38,
	RIGHT: 39,
	DOWN: 40,

	isDown: function (keyCode) {
		return this._pressed[keyCode];
	},

	onKeydown: function (event) {
		if (this._pressed[event.keyCode] != true)
			this.counter++;
		this._pressed[event.keyCode] = true;
	
	},

	onKeyup: function (event) {
		if (this._pressed[event.keyCode] != false)
			this.counter++;
	
		delete this._pressed[event.keyCode];
		this.counter++;
	}
};

$(function () {
	startEventLoop();
	startSensorPolling();
	$(window).keyup(function (ev) { Key.onKeyup(ev); });
	$(window).keydown(function (ev) {
		if (ev.keyCode == 87) //W
			viewModel.increaseVelocity();
		else if (ev.keyCode == 83) //S
			viewModel.decreaseVelocity();
		Key.onKeydown(ev);
	});

})

function startEventLoop() {
	window.setTimeout("eventLoopRun()", 50);
}

var lastState = 0;
var lastVelocity = viewModel.velocity();
function eventLoopRun() {
	var counter = Key.counter;
	if (counter > lastState || lastVelocity != viewModel.velocity()) {
		if(viewModel.velocity() == 0) {
			viewModel.stop();
		}
		else if (Key.isDown(Key.UP) && Key.isDown(Key.DOWN)) {
			viewModel.stop()
		}
		else if (Key.isDown(Key.LEFT) && Key.isDown(Key.RIGHT) && Key.isDown(Key.UP)) {
			viewModel.forward();
		}
		else if (Key.isDown(Key.RIGHT) && Key.isDown(Key.UP)) {
			viewModel.forwardRight();
		}
		else if (Key.isDown(Key.LEFT) && Key.isDown(Key.UP)) {
			viewModel.forwardLeft();
		}
		else if (Key.isDown(Key.UP)) {
			viewModel.forward();
		}
		else if (Key.isDown(Key.LEFT) && Key.isDown(Key.RIGHT)) {
			viewModel.stop();
		}
		else if (Key.isDown(Key.RIGHT) && Key.isDown(Key.DOWN)) {
			viewModel.backRight();
		}
		else if (Key.isDown(Key.LEFT) && Key.isDown(Key.DOWN)) {
			viewModel.backLeft();
		}
		else if (Key.isDown(Key.DOWN)) {
			viewModel.back();
		}		
		else if (Key.isDown(Key.LEFT)) {
			viewModel.turnInPlaceCounterClockwise();
		}
		else if (Key.isDown(Key.RIGHT)) {
			viewModel.turnInPlaceClockwise();
		}
		else {
			viewModel.stop();
		}
	}
	lastState = counter;
	lastVelocity = viewModel.velocity();
	startEventLoop();
}

function startSensorPolling() {
	window.setTimeout("poll()", 1000);
}

function poll() {
	$.ajax({
		url: "/API/State",
		dataType: "json",
		success: function (data) {
			viewModel.robotState(data);
			startSensorPolling();
		},
		error: function (data) {
			startSensorPolling();
		}
	});
}

