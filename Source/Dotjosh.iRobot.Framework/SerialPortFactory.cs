using System.IO.Ports;

namespace Dotjosh.iRobot.Framework
{
	public class SerialPortFactory
	{
		public SerialPort Create(string portName)
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
			return serialPort;
		}
	}
}