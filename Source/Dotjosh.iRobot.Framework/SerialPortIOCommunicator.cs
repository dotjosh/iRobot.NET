using System;
using System.IO.Ports;
using System.Timers;
using Dotjosh.iRobot.Framework.ExtensionMethods;

namespace Dotjosh.iRobot.Framework
{
	public class SerialPortIOCommunicator : IOCommunicator
	{
		public event DataRecievedHandler DataRecieved;
		private SerialPort _serialPort;

		public SerialPortIOCommunicator(string portName)
		{
			var serialPort = new SerialPort
			                 	{
			                 		PortName = portName,
			                 		BaudRate = 57600,  //Pretty much the only supported baud rate, page 5 of "Documents/Create Open Interface.pdf"
			                 		DataBits = 8,
			                 		DtrEnable = false,
			                 		StopBits = StopBits.One,
			                 		Handshake = Handshake.None,
			                 		Parity = Parity.None,
			                 		RtsEnable = false
			                 	};
			Init(serialPort);
		}

		public SerialPortIOCommunicator(SerialPort serialPort)
		{
			Init(serialPort);
		}

		private void Init(SerialPort serialPort)
		{
			_serialPort = serialPort;
			_serialPort.DataReceived += OnSerialPortDataReceived;
			_serialPort.Open();
		}

		private void OnSerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			if (DataRecieved != null)
				DataRecieved(_serialPort.ReadSample());
		}

		public void Write(byte[] bytes, int offset, int length)
		{
			if(!_serialPort.IsOpen)
				return;
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

	public delegate void DataRecievedHandler(byte[] newBytes);

	//This abstraction could be useful if proxying bytes over http, etc. while 
	//reusing this libraries nice command/sensor model. In all honesty, this is for testing purposes.
	public interface IOCommunicator : IDisposable  
	{
		event DataRecievedHandler DataRecieved;
		void Write(byte[] bytes, int offset, int length);
	}
}