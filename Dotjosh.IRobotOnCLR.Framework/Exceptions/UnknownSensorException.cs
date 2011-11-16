using System;

namespace Dotjosh.IRobotOnCLR.Exceptions
{
	public class UnknownSensorException : Exception
	{
		public UnknownSensorException(byte packetId) : base("Could not find sensor with packetId " + packetId){}
	}
}