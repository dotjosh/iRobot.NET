using System.Collections.Generic;

namespace Dotjosh.iRobot.Framework.Commands
{
	public class SwitchToSafeMode : Command
	{
		protected override byte OpCode
		{
			get { return 131; }
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