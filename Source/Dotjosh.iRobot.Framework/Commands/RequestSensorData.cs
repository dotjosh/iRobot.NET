using System;
using System.Collections.Generic;

namespace Dotjosh.iRobot.Framework.Commands
{
	public class RequestSensorData : RobotCommand
	{
		protected override byte OpCode
		{
			get { return 142; }
		}

		protected override IList<byte> SubsequentBytes
		{
			get { return BitConverter.GetBytes(6); }
		}
	}
}