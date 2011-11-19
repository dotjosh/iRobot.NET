using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dotjosh.iRobot.Framework.Exceptions;

namespace Dotjosh.iRobot.Framework.Sensors
{
	public class SensorStatusData
	{
		private readonly byte[] _bytes;

		public SensorStatusData(byte[] bytes)
		{
			_bytes = bytes;
		}

		public void UpdateApplicableSensors(IEnumerable<ISensor> sensors)
		{
			for (var currentByteIndex = 0; currentByteIndex < _bytes.Length; )
			{
				var packetId = _bytes[currentByteIndex];
				var sensor = sensors.FirstOrDefault(s => s.PackedId == packetId);
				if (sensor == null)
				{
					Trace.WriteLine("Could not find sensor with packet id " + packetId);
					return;
				}
				const int packedIdSize = 1;
				var sensorDataBytes = _bytes
										.Skip(currentByteIndex + packedIdSize)
										.Take(sensor.DataByteCount)
										.ToArray();
				sensor.Bytes = sensorDataBytes;
				currentByteIndex += packedIdSize + sensor.DataByteCount;  //Advance to the end of the last read so we can continue to find more sensor updates if more than one came in this recieve 
			}
		}
	}
}