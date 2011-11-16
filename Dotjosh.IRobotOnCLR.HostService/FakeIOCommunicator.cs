using System;
using System.Collections;
using Dotjosh.IRobotOnCLR.Core;

namespace Dotjosh.IRobotOnCLR.HostService
{
	public class FakeIOCommunicator : IOCommunicator
	{
		private bool _hasStarted;

		public void Dispose()
		{
			
		}

		public event DataRecievedHandler DataRecieved;

		public void Write(byte[] bytes, int offset, int length)
		{
			if(!_hasStarted)
			{
				_hasStarted = true;
				StartFakePushing();
			}
		}

		public void StartFakePushing()
		{
			var timer = new System.Timers.Timer(20);
			timer.Elapsed += (sender, args) =>
			                 {
			                 	DataRecieved(CreateFakeSensorUpdate());
			                 };
			timer.Start();
		}

		private static bool _frontRight;
		private static bool _frontLeft;
		private byte[] CreateFakeSensorUpdate()
		{
			var random = new Random();
			if (random.Next(50) == 17)
				_frontRight = !_frontRight;
			if (random.Next(50) == 33)
				_frontLeft = !_frontLeft;
			byte headerByte = 19;
			byte numOfBytesInBody = 2;
			byte dataByte = BitsToByte(new BitArray(new[] { _frontRight, _frontLeft, false, false, false, false, false, false }));
			var sensorUpdateBytes = new[]
			                        	{
			                        		headerByte, 
			                        		numOfBytesInBody,
			                        		PacketIds.Bumps_And_WheelDrops,
			                        		dataByte
			                        	};
			return sensorUpdateBytes;
		}

		private static byte BitsToByte(BitArray bits)
		{
			if (bits.Count != 8)
				throw new Exception("A byte has 7 bits, you only passed in " + bits.Count);
			byte[] bytes = new byte[1];
			bits.CopyTo(bytes, 0);
			return bytes[0];
		}
	}
}