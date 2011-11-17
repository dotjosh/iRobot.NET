using System;
using System.Collections.Generic;
using System.Diagnostics;
using Dotjosh.iRobot.Framework.Commands;
using Dotjosh.iRobot.Framework.Sensors;

namespace Dotjosh.iRobot.Framework
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

		public void RequestSensorUpdates()
		{
			var startStreamCommand = new RequestSensorStream(_sensors);
			Execute(startStreamCommand);
		}

		public void Execute(ICommand command)
		{
			command.Execute(_ioCommunicator);
			OnCommandExecuted(command);
		}

		public event Action<ICommand> CommandExecuted;
		private void OnCommandExecuted(ICommand command)
		{
			Debug.WriteLine("{0} sent to iRobot", command);
			if (CommandExecuted != null)
				CommandExecuted(command);
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

		public void Dispose()
		{
			_ioCommunicator.DataRecieved -= IO_DataRecieved;
			_ioCommunicator.Dispose();
		}
	}
}