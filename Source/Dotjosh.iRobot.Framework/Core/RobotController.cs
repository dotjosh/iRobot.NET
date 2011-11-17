using System;
using System.Collections.Generic;
using Dotjosh.iRobot.Framework.Commands;

namespace Dotjosh.iRobot.Framework.Core
{
	public class RobotController : IDisposable
	{
		public event Action SensorsUpdated;

		private readonly IOCommunicator _ioCommunicator;
		private readonly SensorUpdater _sensorUpdater;

		public RobotController(IOCommunicator ioCommunicator)
		{
			_ioCommunicator = ioCommunicator;
			_sensorUpdater = new SensorUpdater();
			_ioCommunicator.DataRecieved += IO_DataRecieved;
		}

		public void StartStreamingSensorUpdates(IEnumerable<ISensor> sensors)
		{
			_sensorUpdater.Sensors = sensors;
			var startStreamCommand = new StartStreamCommand(sensors);
			Execute(startStreamCommand);
		}

		public void Execute(ICommand command)
		{
			command.Execute(_ioCommunicator);
		}

		private void IO_DataRecieved(byte[] newBytes)
		{
			var sensorResponse = new SensorResponse(newBytes);
			_sensorUpdater.Handle(sensorResponse);
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