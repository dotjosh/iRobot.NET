using System;
using System.Collections.Generic;
using Dotjosh.iRobot.Framework.Commands;

namespace Dotjosh.iRobot.Framework.Core
{
	public class RobotController : IDisposable
	{
		public event Action SensorsUpdated;

		private readonly IOCommunicator _ioCommunicator;
		private readonly IEnumerable<ISensor> _sensors;

		public RobotController(IOCommunicator ioCommunicator, IEnumerable<ISensor> sensors)
		{
			_ioCommunicator = ioCommunicator;
			_sensors = sensors;
			_ioCommunicator.DataRecieved += IO_DataRecieved;
		}

		public void StartStreamingSensorUpdates()
		{
			var startStreamCommand = new StartStreamCommand(_sensors);
			Execute(startStreamCommand);
		}

		public void Execute(ICommand command)
		{
			command.Execute(_ioCommunicator);
		}

		private void IO_DataRecieved(byte[] newBytes)
		{
			var sensorResponse = new SensorResponse(newBytes);
			sensorResponse.UpdateSensors(_sensors);
			OnSensorsUpdated();
		}

		private void OnSensorsUpdated()
		{
			if (SensorsUpdated != null)
				SensorsUpdated();
		}

		public void Dispose()
		{
			_ioCommunicator.DataRecieved -= IO_DataRecieved;
			_ioCommunicator.Dispose();
		}
	}
}