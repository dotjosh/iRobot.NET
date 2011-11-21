using System;
using System.Collections.Generic;
using System.Linq;

namespace Dotjosh.iRobot.Framework.Commands
{
	public abstract class RobotCommand : IRobotCommand
	{
		protected abstract byte Opcode { get; }
		protected abstract IList<byte> SubsequentBytes { get; }

		public virtual void Execute(IOCommunicator ioCommunicator)
		{
			var bytes = GetBytes();
			ioCommunicator.Write(bytes, 0, bytes.Length);
		}

		protected byte[] GetBytes()
		{
			var finalBytes = new byte[SubsequentBytes.Count + 1];
			finalBytes[0] = Opcode;

			var i = 0;
			foreach (var currentByte in SubsequentBytes)
			{
				finalBytes[++i] = currentByte;
			}

			return finalBytes;
		}

		protected IEnumerable<Byte> Bytes(short val)
		{
			byte[] bytes = BitConverter.GetBytes(val);
			return bytes.Reverse();
		}		
		
		protected IEnumerable<Byte> Bytes(ushort val)
		{
			byte[] bytes = BitConverter.GetBytes(val);
			return bytes.Reverse();
		}

		public override string ToString()
		{
			return GetType().Name;
		}
	}
}