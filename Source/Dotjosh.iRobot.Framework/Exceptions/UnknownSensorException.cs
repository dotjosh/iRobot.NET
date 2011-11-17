using System;

namespace Dotjosh.iRobot.Framework.Exceptions
{
	public class UnknownSensorException : Exception
	{
		public UnknownSensorException(byte packetId) : base(string.Format("Could not find sensor with packetId {0}", packetId)){}
	}
}