using System;
using System.Collections.Generic;
using System.Linq;
using Dotjosh.IRobotOnCLR.Exceptions;

namespace Dotjosh.IRobotOnCLR.Core
{
	public class SensorUpdater
	{
		private IDictionary<byte, ISensor> _sensorDictionary;

		public IEnumerable<ISensor> Sensors
		{
			set
			{
				_sensorDictionary = new Dictionary<byte, ISensor>();
				foreach (var sensor in value)
					_sensorDictionary.Add(sensor.PackedId, sensor);				
			}
		}

		public void Handle(SensorResponse sensorResponse)
		{
			for (var i = 0; i < sensorResponse.Body.Length; )
			{
				var packetId = sensorResponse.Body[i];
				if (!_sensorDictionary.ContainsKey(packetId))
				{
					throw new UnknownSensorException(packetId);
				}
				var sensor = _sensorDictionary[packetId];
				var sensorDataBytes = sensorResponse
										.Body
										.Skip(i + 1)
										.Take(sensor.DataByteCount)
										.ToArray();
				sensor.Handle(sensorDataBytes);
				i++;
				i += sensor.DataByteCount;
			}
		}
	}
}