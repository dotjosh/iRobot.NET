using System.Linq;

namespace Dotjosh.iRobot.Tests.Core
{
	public class SensorResponse
	{
		private readonly byte[] _bytes;

		public SensorResponse(byte[] bytes)
		{
			_bytes = bytes;
		}

		public byte Header 
		{
			get { return _bytes[0]; } 
		}

		public int BodyLength
		{
			get { return _bytes[1]; }
		}

		public byte[] Body
		{
			get { return _bytes.ToList().GetRange(2, BodyLength).ToArray(); }
		}
	}
}