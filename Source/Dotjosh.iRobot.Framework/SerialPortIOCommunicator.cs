using System;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;

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
			LastCommunicationWasSuccessful = true;
		}

		private void OnSerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			try
			{
				if (DataRecieved != null)
					DataRecieved(ReadLineOfData());
				LastCommunicationWasSuccessful = true;
			}			
			catch (IOException ex)
			{
				LastCommunicationWasSuccessful = false;
				Trace.WriteLine(ex);
			}
		}

		protected bool LastCommunicationWasSuccessful { get; set; }

		public bool IsConnected
		{
			get { return  _serialPort.IsOpen && LastCommunicationWasSuccessful; }
		}

		public void Write(byte[] bytes, int offset, int length)
		{
			try
			{
				_serialPort.Write(bytes, offset, length);
				LastCommunicationWasSuccessful = true;
			}
			catch (IOException ex)
			{
				LastCommunicationWasSuccessful = false;
				Trace.WriteLine(ex);
			}
		}

		private byte[] ReadLineOfData()
		{
			var headerByte = _serialPort.ReadByte();
			while(headerByte != 19 && _serialPort.IsOpen)
			{
				if(!_serialPort.IsOpen)
					throw new IOException();
				headerByte = _serialPort.ReadByte();
			}
			var bytesLeftToReadAndChecksum = _serialPort.ReadByte() + 1;
			var result = new byte[bytesLeftToReadAndChecksum];
			var bytesRead = 0;
			var indexInResult = 0;
			while(bytesRead < bytesLeftToReadAndChecksum)
			{
				byte[] buffer = new byte[1024];
				var thisBytesRead = _serialPort.Read(buffer, 0, bytesLeftToReadAndChecksum - bytesRead);
				bytesRead += thisBytesRead;
				if(thisBytesRead > 0)
				{
					Array.Copy(buffer, 0, result, indexInResult, thisBytesRead);
					indexInResult += thisBytesRead;
				}
			}
			return result.Take(bytesLeftToReadAndChecksum - 1).ToArray();
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
		bool IsConnected { get; }
		void Write(byte[] bytes, int offset, int length);
	}
}