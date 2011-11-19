using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;

namespace Dotjosh.iRobot.Framework.ExtensionMethods
{
	public static class SerialPortExtensions
	{
		public static byte[] ReadSample(this SerialPort serialPort)
		{
			int starterByte = serialPort.ReadByte();
			while(starterByte != 19 && serialPort.IsOpen)
			{
				if(!serialPort.IsOpen)
					return new byte[]{};
				try
				{
					starterByte = serialPort.ReadByte();
				}
				catch (IOException ex)
				{
					return new byte[] {};
				}
			}
			int bytesLeftToReadAndChecksum = serialPort.ReadByte() + 1;

			byte[] result = new byte[bytesLeftToReadAndChecksum];

			int bytesRead = 0;
			int indexInResult = 0;
			while(bytesRead < bytesLeftToReadAndChecksum)
			{
				byte[] buffer = new byte[1024];
				var thisBytesRead = serialPort.Read(buffer, 0, bytesLeftToReadAndChecksum - bytesRead);
				bytesRead += thisBytesRead;
				if(thisBytesRead > 0)
				{
					Array.Copy(buffer, 0, result, indexInResult, thisBytesRead);
					indexInResult += thisBytesRead;
				}
			}
			return result.Take(bytesLeftToReadAndChecksum - 1).ToArray();
		}
	}
}