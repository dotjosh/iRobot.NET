using System.IO.Ports;

namespace Dotjosh.IRobotOnCLR.Core
{
	public class SerialPortFactory
	{
		public SerialPort Create(string portName, int baudRate)
		{
			var serialPort = new SerialPort
			                 	{
			                 		PortName = portName,
			                 		BaudRate = baudRate,
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