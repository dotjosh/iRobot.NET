using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;

namespace Dotjosh.IRobotOnCLR.ExtensionMethods
{
	public static class SerialPortExtensions
	{
		public static byte[] ReadAll(this SerialPort serialPort)
		{
			var result = new List<byte>();
			byte[] buffer = new byte[16*1024];
			int bytesRead;
			while((bytesRead = serialPort.Read(buffer, 0, buffer.Length)) > 0)
			{
				result.AddRange(buffer.ToList().GetRange(0, bytesRead));
			}
			return result.ToArray();
		}
	}
}