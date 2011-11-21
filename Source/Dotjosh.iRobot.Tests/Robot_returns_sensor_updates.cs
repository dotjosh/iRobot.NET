using System;
using System.Collections;
using System.Collections.Generic;
using Dotjosh.iRobot.Framework;
using Dotjosh.iRobot.Framework.ExtensionMethods;
using Dotjosh.iRobot.Framework.Sensors;
using Moq;
using NUnit.Framework;
using Should;

namespace Dotjosh.iRobot.Tests
{
	[TestFixture]
	public class Robot_returns_sensor_updates
	{
		private RobotController _robotController;
		private Mock<IOCommunicator> _mockIoCommunicator;
		private BumpsAndWheelDrops _bumpsAndWheelDropsSensor;

		[TestFixtureSetUp]
		public void SetUp()
		{
			_mockIoCommunicator = new Mock<IOCommunicator>();


			_bumpsAndWheelDropsSensor = new BumpsAndWheelDrops();
			var sensors = new List<ISensor>
			              	{
			              		_bumpsAndWheelDropsSensor
			              	};
			_robotController = new RobotController(_mockIoCommunicator.Object, sensors);
			_robotController.StartSensorStream();

			byte dataByte = new BitArray(new[] {true, true, false, false, false, false, false, false}).ToByte();
			var sensorUpdateBytes = new[]
			                        	{
											Sensor.Bumps_And_WheelDrops,
											dataByte
			                        	};
			_mockIoCommunicator.Raise(ioCommunicator => ioCommunicator.DataRecieved += null, sensorUpdateBytes);
		}


		[Test]
		public void Sensor_is_updated_as_expected()
		{
			_bumpsAndWheelDropsSensor.BumpLeft.ShouldBeTrue();
			_bumpsAndWheelDropsSensor.BumpRight.ShouldBeTrue();
			_bumpsAndWheelDropsSensor.WheelDropLeft.ShouldBeFalse();
			_bumpsAndWheelDropsSensor.WheelDropRight.ShouldBeFalse();
		}
	}
}