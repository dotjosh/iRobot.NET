using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

		public IEnumerable<ISensor> Sensors
		{
			get { return _sensors; }
		}

		public bool IsConnected
		{
			get { return _ioCommunicator.IsConnected; }
		}

		public void StartSensorStream()
		{
			var startStreamCommand = new RequestSensorStream(_sensors);
			Execute(startStreamCommand);
		}

		public void StopSensorStream()
		{
			var pauseStreamCommand = new PauseSensorStream();
			Execute(pauseStreamCommand);
		}

		public bool SensorStreamIsRunning
		{
			get { return DateTime.Now - TimeSpan.FromSeconds(1) < LastDataRecieved; }
		}

		public void Execute(IRobotCommand command)
		{
			command.Execute(_ioCommunicator);
			Console.WriteLine("{0} {1} sent to iRobot", DateTime.Now, command);
		}

		private void IO_DataRecieved(byte[] newBytes)
		{
			//Debug.WriteLine(newBytes.Aggregate(new StringBuilder(), (sb, b)=> sb.AppendFormat("[{0}]", b)));
			var sensorResponse = new SensorStatusData(newBytes);
			sensorResponse.UpdateApplicableSensors(_sensors);
			LastDataRecieved = DateTime.Now;
		}

		protected DateTime? LastDataRecieved { get; set; }

		public void Dispose()
		{
			_ioCommunicator.DataRecieved -= IO_DataRecieved;
			_ioCommunicator.Dispose();
		}

		public static RobotController CreateWithAllSensors(string portName)
		{
			var sensors =  typeof (ISensor).Assembly
										.GetTypes().Where(t => typeof(ISensor).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass)
										.Select(Activator.CreateInstance)
										.Cast<ISensor>();
			return new RobotController(new SerialPortIOCommunicator(portName), sensors.ToList());
		}
	}
}