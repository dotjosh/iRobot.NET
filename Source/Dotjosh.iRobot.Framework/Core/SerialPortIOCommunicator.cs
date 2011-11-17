using System;
using System.IO.Ports;
using Dotjosh.iRobot.Framework.ExtensionMethods;

namespace Dotjosh.iRobot.Framework.Core
{
	public class SerialPortIOCommunicator : IOCommunicator
	{
		public event DataRecievedHandler DataRecieved;
		private readonly SerialPort _serialPort;

		public SerialPortIOCommunicator(SerialPort serialPort)
		{
			_serialPort = serialPort;
			_serialPort.DataReceived += OnSerialPortDataReceived;
		}

		private void OnSerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			if (DataRecieved != null)
				DataRecieved(_serialPort.ReadAll());
		}

		public void Write(byte[] bytes, int offset, int length)
		{
			_serialPort.Write(bytes, offset, length);
		}

		public void Dispose()
		{
			if (_serialPort == null) 
				return;

			_serialPort.DataReceived -= OnSerialPortDataReceived;
			if (_serialPort.IsOpen)
				_serialPort.Close();
			_serialPort.Dispose();
		}
	}
}