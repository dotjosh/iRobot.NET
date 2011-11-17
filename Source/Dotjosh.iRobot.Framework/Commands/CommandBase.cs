using System.Collections.Generic;

namespace Dotjosh.iRobot.Framework.Commands
{
	public abstract class CommandBase : ICommand
	{
		protected abstract byte OpCode { get; }
		protected abstract IList<byte> SubsequentBytes { get; }

		public void Execute(IOCommunicator ioCommunicator)
		{
			var finalBytes = new byte[SubsequentBytes.Count + 1];
			finalBytes[0] = OpCode;

			var i = 0;
			foreach (var currentByte in SubsequentBytes)
			{
				finalBytes[++i] = currentByte;
			}

			ioCommunicator.Write(finalBytes, 0, finalBytes.Length);
		}
	}
}