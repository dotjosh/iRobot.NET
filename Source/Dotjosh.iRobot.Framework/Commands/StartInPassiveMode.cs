using System.Collections.Generic;

namespace Dotjosh.iRobot.Framework.Commands
{
	public class StartInPassiveMode : CommandBase
	{
		protected override byte OpCode
		{
			get { return 128; }
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