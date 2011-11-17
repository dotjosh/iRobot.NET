using System.Collections.Generic;
using System.Threading;

namespace Dotjosh.iRobot.Framework.Commands
{
	public class Baud : Command
	{
		protected override byte OpCode
		{
			get { return 129; }
		}

		protected override IList<byte> SubsequentBytes
		{
			get
			{
				const byte baud57600 = 10;
				return new [] { baud57600 };
			}
		}

		public override void Execute(IOCommunicator ioCommunicator)
		{
			base.Execute(ioCommunicator);
			Thread.Sleep(100);  //Docs say to wait 100ms before sending more commands after this one... so let's just enforce it this way
		}
	}
}