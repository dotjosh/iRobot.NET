using System.Collections.Generic;

namespace Dotjosh.iRobot.Framework.Commands
{
	public class SafeMode : CommandBase
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