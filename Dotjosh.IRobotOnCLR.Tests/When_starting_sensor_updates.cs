using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dotjosh.IRobotOnCLR.Core;
using Dotjosh.IRobotOnCLR.Sensors;
using Moq;
using NUnit.Framework;

namespace Dotjosh.IRobotOnCLR.Tests
{
	[TestFixture]
	public class When_starting_sensor_updates
	{
		private RobotController _robotController;
		private Mock<IOCommunicator> _mockIoCommunicator;
		private Expression<Action<IOCommunicator>> _writeToIoExpectation;

		[TestFixtureSetUp]
		public void SetUp()
		{
			_mockIoCommunicator = new Mock<IOCommunicator>();

			var startStreamBytes = new byte[] { 148, 1, PacketIds.Bumps_And_WheelDrops };
			_writeToIoExpectation = ioCommunicator => ioCommunicator.Write(
														It.Is<byte[]>(bytes => BytesAreEqual(startStreamBytes, bytes)), 
														It.Is<int>(offset => offset == 0),
														It.Is<int>(length => length == startStreamBytes.Length)
													);
			_mockIoCommunicator.Setup(_writeToIoExpectation);

			var sensors = new List<ISensor>
			              	{
			              		new BumpsAndWheelDrops()
			              	};
			_robotController = new RobotController(_mockIoCommunicator.Object);
			_robotController.StartStreamingSensorUpdates(sensors);
		}

		[Test]
		public void StartStreamCommand_bytes_are_written_to_IO_Communicator()
		{
			_mockIoCommunicator.Verify(_writeToIoExpectation);
		}

		private static bool BytesAreEqual(byte[] a, byte[] b)
		{
			if (a.Length != b.Length)
				return false;

			for (var i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
					return false;
			}

			return true;
		}
	}
}