using System.Collections.Generic;
using System.Linq;
using Dotjosh.iRobot.Framework.Exceptions;

namespace Dotjosh.iRobot.Framework.Core
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
			get { return _bytes.Skip(2).Take(BodyLength).ToArray(); }
		}

		public void UpdateSensors(IEnumerable<ISensor> sensors)
		{
			for (var currentByteIndex = 0; currentByteIndex < Body.Length; )
			{
				var packetId = Body[currentByteIndex];
				var sensor = sensors.FirstOrDefault(s => s.PackedId == packetId);
				if (sensor == null)
				{
					throw new UnknownSensorException(packetId);
				}
				const int packedIdSize = 1;
				var sensorDataBytes = Body
										.Skip(currentByteIndex + packedIdSize)
										.Take(sensor.DataByteCount)
										.ToArray();
				sensor.Handle(sensorDataBytes);
				currentByteIndex += packedIdSize + sensor.DataByteCount;
			}
		}
	}
}