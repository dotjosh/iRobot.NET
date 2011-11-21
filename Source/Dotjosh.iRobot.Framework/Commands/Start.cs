using System.Collections.Generic;

namespace Dotjosh.iRobot.Framework.Commands
{
	public class Start : CompositeCommand
	{
		public override IEnumerable<IRobotCommand> Commands
		{
			get
			{
				yield return new InitialStart();
				yield return new Baud();
			}
		}
	}

	public class InitialStart : RobotCommand
	{
		protected override byte Opcode
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