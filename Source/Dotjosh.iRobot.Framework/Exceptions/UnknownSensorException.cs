using System;

namespace Dotjosh.iRobot.Tests.Exceptions
{
	public class UnknownSensorException : Exception
	{
		public UnknownSensorException(byte packetId) : base("Could not find sensor with packetId " + packetId){}
	}
}