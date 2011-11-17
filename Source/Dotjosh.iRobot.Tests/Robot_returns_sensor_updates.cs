using System;
using System.Collections;
using System.Collections.Generic;
using Dotjosh.iRobot.Framework;
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
			_robotController.RequestSensorUpdates();

			byte headerByte = 19;
			byte numOfBytesInBody = 2;
			byte dataByte = BitsToByte(new BitArray(new[] {true, true, false, false, false, false, false, false}));
			var sensorUpdateBytes = new[]
			                        	{
			                        		headerByte, 
											numOfBytesInBody,
											Sensor.Bumps_And_WheelDrops,
											dataByte
			                        	};
			_mockIoCommunicator.Raise(ioCommunicator => ioCommunicator.DataRecieved += null, sensorUpdateBytes);
		}

		private static byte BitsToByte(BitArray bits)
		{
			if (bits.Count != 8)
				throw new Exception("A byte has 8 bits, wtf you only passed in " + bits.Count);
			byte[] bytes = new byte[1];
			bits.CopyTo(bytes, 0);
			return bytes[0];
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