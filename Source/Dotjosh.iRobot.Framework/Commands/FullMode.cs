using System.Collections.Generic;

namespace Dotjosh.iRobot.Framework.Commands
{
	public class FullMode : CommandBase
	{
		protected override byte OpCode
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