using System.Collections.Generic;

namespace Dotjosh.iRobot.Framework.Commands
{
	public class SwitchToFullMode : RobotCommand
	{
		protected override byte Opcode
		{
			get { return 132; }
		}

		protected override IList<byte> SubsequentBytes
		{
			get
			{
				return new List<byte>();
			}
		}
	}
}