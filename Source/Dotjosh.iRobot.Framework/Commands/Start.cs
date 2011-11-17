using System.Collections.Generic;

namespace Dotjosh.iRobot.Framework.Commands
{
	public class Start : CompositeCommand
	{
		public override IEnumerable<ICommand> Commands
		{
			get
			{
				yield return new InitialStart();
				yield return new Baud();
			}
		}
	}

	public class InitialStart : Command
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