using System.Collections.Generic;

namespace Dotjosh.iRobot.Framework.Commands
{
	public class PauseSensorStream : RobotCommand
	{
		protected override byte Opcode
		{
			get { return 150; }
		}

		protected override IList<byte> SubsequentBytes
		{
			get { return new List<byte>{ 0 };}
		}
	}
}