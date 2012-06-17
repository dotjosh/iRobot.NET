DEMOMODE = false;
DEFAULT_VELOCITY = 100;
VELOCITY_INCREMENT = 50;

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
	}
};

var ajaxQueue = (function () {
	var requests = [];

	return {
		enqeue: function (opt) {
			requests.push(opt);
		},
		run: function () {
			var self = this,
			    originalSuccess;

			if (requests.length) {
				originalSuccess = requests[0].complete;

				requests[0].complete = function() {
					if (typeof(originalSuccess) === 'function') originalSuccess();
					requests.shift();
					self.run.apply(self, []);
				};

				$.ajax(requests[0]);
			}
			else {
				self.tid = setTimeout(function () {
					self.run.apply(self, []);
				}, 50);				
			}
			
		}
	};
} ());

var viewModel = {
	robotState: ko.observable({ IsConnected: DEMOMODE || false, IsStreaming: DEMOMODE || false, Sensors: [] }),
	portName: ko.observable(),
	velocity: ko.observable(DEFAULT_VELOCITY),
	currentMovement: ko.observable(),
	increaseVelocity: function () {
		if (this.velocity() == 500) {
			return;
		}

		this.velocity(this.velocity() + VELOCITY_INCREMENT);
	},
	decreaseVelocity: function () {
		if (this.velocity() == 0) {
			return;
		}
		this.velocity(this.velocity() - VELOCITY_INCREMENT);
	},
	connect: function () {
		ajaxQueue.enqeue({
			type: 'POST',
			url: "/API/Connect",
			data: "portName=" + this.portName(),
			success: function () {

			},
			error: function (ex) {
				console.log(ex);
				alert("Server returned an error");
			}
		});
	},
	disconnect: function () {
		ajaxQueue.enqeue({
			type: 'POST',
			url: "/API/Disconnect",
			success: function () {

			},
			error: function (ex) {
				console.log(ex);
				alert("Server returned an error");
			}
		});
	},
	startStream: function () {
		ajaxQueue.enqeue({
			type: 'POST',
			url: "/API/StartStream",
			success: function () {

			},
			error: function (ex) {
				console.log(ex);
				alert("Server returned an error");
			}
		});
	},
	stopStream: function () {
		ajaxQueue.enqeue({
			type: 'POST',
			url: "/API/StopStream",
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
		viewModel.currentMovement("forward");
		ajaxQueue.enqeue({
			type: 'POST',
			url: "/API/Commands/DriveStraight",
			data: "velocity=" + this.velocity()
		})
	},
	forwardLeft: function () {
		console.log("Forward-Left");
		viewModel.currentMovement("forwardLeft");
		ajaxQueue.enqeue({
			type: 'POST',
			url: "/API/Commands/Drive",
			data: "velocity=" + this.velocity() + "&radius=300"
		});
	},
	forwardRight: function () {
		console.log("Forward-Right");
		viewModel.currentMovement("forwardRight");
		ajaxQueue.enqeue({
			type: 'POST',
			url: "/API/Commands/Drive",
			data: "velocity=" + this.velocity() + "&radius=-300"
		});
	},
	backLeft: function () {
		console.log("Back-Left");
		viewModel.currentMovement("backLeft");
		ajaxQueue.enqeue({
			type: 'POST',
			url: "/API/Commands/Drive",
			data: "velocity=-" + this.velocity() + "&radius=300"
		});
	},
	backRight: function () {
		console.log("Back-Right");
		viewModel.currentMovement("backRight");
		ajaxQueue.enqeue({
			type: 'POST',
			url: "/API/Commands/Drive",
			data: "velocity=-" + this.velocity() + "&radius=-300"
		});
	},
	turnInPlaceClockwise: function () {
		console.log("Clockwise");
		viewModel.currentMovement("clockwise");
		ajaxQueue.enqeue({
			type: 'POST',
			url: "/API/Commands/TurnInPlaceClockwise",
			data: "velocity=" + this.velocity()
		});
	},
	turnInPlaceCounterClockwise: function () {
		console.log("Counter-Clockwise");
		viewModel.currentMovement("counterClockwise");
		ajaxQueue.enqeue({
			type: 'POST',
			url: "/API/Commands/TurnInPlaceCounterClockwise",
			data: "velocity=" + this.velocity()
		});
	},
	stop: function () {
		console.log("Stop");
		viewModel.currentMovement("stop");
		ajaxQueue.enqeue({
			type: 'POST',
			url: "/API/Commands/DriveStop"
		});
	},
	back: function () {
		console.log("Back");
		viewModel.currentMovement("back");
		ajaxQueue.enqeue({
			type: 'POST',
			url: "/API/Commands/DriveStraight",
			data: "velocity=-" + this.velocity() + ""
		});
	},
	dock: function () {
		console.log("Dock");
		ajaxQueue.enqeue({
			type: 'POST',
			url: "/API/Commands/StartDemo",
			data: "demo=1"
		});
	},
	cancelDock: function () {
		console.log("Cancel Dock");
		ajaxQueue.enqeue({
			type: 'POST',
			url: "/API/Commands/StartDemo",
			data: "demo=-1"
		});
		this.switchToFullMode();
	},
	switchToFullMode: function () {
		console.log("Switch to full mode");
		ajaxQueue.enqeue({
			type: 'POST',
			url: "/API/Commands/SwitchToFullMode"
		});
	}
};
viewModel.isConnected = ko.dependentObservable(function () {
	return this.robotState().IsConnected;
}, viewModel);
viewModel.isStreaming = ko.dependentObservable(function () {
	return this.robotState().IsStreaming;
}, viewModel);
viewModel.status = ko.dependentObservable(function () {
	return this.robotState().IsConnected ? "CONNECTED" : "DISCONNECTED";
}, viewModel);
viewModel.ports = ko.dependentObservable(function () {
	return this.robotState().Ports;
}, viewModel);
viewModel.batteryPercentage = ko.dependentObservable(function () {
	var result = 0;
	this.robotState().Sensors.forEach(function(item) {
		if (item.Name == "BatteryCharge") {
			result = item.Percentage;
		}
	});
	return result;
}, viewModel);
viewModel.cliffLeft = ko.dependentObservable(function () {
	var result = "Unknown";
	this.robotState().Sensors.forEach(function(item) {
		if (item.Name == "CliffLeft") {
			result = item.IsCliff;
		}
	});
	return result;
}, viewModel);
viewModel.cliffRight = ko.dependentObservable(function () {
	var result = "Unknown";
	this.robotState().Sensors.forEach(function(item) {
		if (item.Name == "CliffRight") {
			result = item.IsCliff;
		}
	});
	return result;
}, viewModel);
viewModel.bumpLeft = ko.dependentObservable(function () {
	var result = "Unknown";
	this.robotState().Sensors.forEach(function(item) {
		if (item.Name == "BumpsAndWheelDrops") {
			result = item.BumpLeft;
		}
	});
	return result;
}, viewModel);
viewModel.bumpRight = ko.dependentObservable(function () {
	var result = "Unknown";
	this.robotState().Sensors.forEach(function(item) {
		if (item.Name == "BumpsAndWheelDrops") {
			result = item.BumpRight;
		}
	});
	return result;
}, viewModel);
ko.applyBindings(viewModel);

function updateVelocityBar(newValue) {
	$("#velocityBar").progressbar({
		value: newValue / 5
	});
}

function updateBatteryPercentage(newValue) {
	$("#batteryPercentage").progressbar({
		value: newValue
	});
}

$(function () {
	startEventLoop();
	startSensorPolling();
	ajaxQueue.run(); 
	$(window).keyup(function (ev) { Key.onKeyup(ev); });
	$(window).keydown(function (ev) {
		if (ev.keyCode == 87) //W
			viewModel.increaseVelocity();
		else if (ev.keyCode == 83) //S
			viewModel.decreaseVelocity();
		Key.onKeydown(ev);
	});
	updateVelocityBar(viewModel.velocity());
	viewModel.velocity.subscribe(function (newValue) {
		updateVelocityBar(newValue);
	})
	updateBatteryPercentage(viewModel.batteryPercentage());
	viewModel.batteryPercentage.subscribe(function (newValue) {
		updateBatteryPercentage(newValue);
	})

	$("#mainContent").show();
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
			if (!DEMOMODE)
				viewModel.robotState(data);
			startSensorPolling();
		},
		error: function (data) {
			startSensorPolling();
		}
	});
}

