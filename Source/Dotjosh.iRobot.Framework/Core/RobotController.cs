using System;
using System.Collections.Generic;
using Dotjosh.iRobot.Framework.Commands;

namespace Dotjosh.iRobot.Framework.Core
{
	public class RobotController : IDisposable
	{
		private readonly IOCommunicator _ioCommunicator;
		private readonly IEnumerable<ISensor> _sensors;

		public RobotController(IOCommunicator ioCommunicator, IEnumerable<ISensor> sensors)
		{
			_ioCommunicator = ioCommunicator;
			_ioCommunicator.DataRecieved += IO_DataRecieved;
			_sensors = sensors;
		}

		private void IO_DataRecieved(byte[] newBytes)
		{
			var sensorResponse = new SensorStatusData(newBytes);
			sensorResponse.UpdateApplicableSensors(_sensors);
			OnSensorsUpdated();
		}

		public event Action SensorsUpdated;
		private void OnSensorsUpdated()
		{
			if (SensorsUpdated != null)
				SensorsUpdated();
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


		public void Dispose()
		{
			_ioCommunicator.DataRecieved -= IO_DataRecieved;
			_ioCommunicator.Dispose();
		}
	}
}