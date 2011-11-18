using System.Collections.Generic;

namespace Dotjosh.iRobot.Framework.Commands
{
	public abstract class RobotCommand : IRobotCommand
	{
		protected abstract byte OpCode { get; }
		protected abstract IList<byte> SubsequentBytes { get; }

		public virtual void Execute(IOCommunicator ioCommunicator)
		{
			var bytes = GetBytes();
			ioCommunicator.Write(bytes, 0, bytes.Length);
		}

		protected byte[] GetBytes()
		{
			var finalBytes = new byte[SubsequentBytes.Count + 1];
			finalBytes[0] = OpCode;

			var i = 0;
			foreach (var currentByte in SubsequentBytes)
			{
				finalBytes[++i] = currentByte;
			}

			return finalBytes;
		}

		public override string ToString()
		{
			return GetType().Name;
		}
	}
}