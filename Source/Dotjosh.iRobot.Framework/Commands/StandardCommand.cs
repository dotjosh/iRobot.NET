using System.Collections.Generic;
using Dotjosh.iRobot.Framework.Core;

namespace Dotjosh.iRobot.Framework.Commands
{
	public abstract class StandardCommand : ICommand
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