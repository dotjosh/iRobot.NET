using System.Collections.Generic;
using System.Linq;
using Dotjosh.iRobot.Framework.Core;

namespace Dotjosh.iRobot.Framework.Commands
{
	public class StartStreamCommand : StandardCommand
	{
		private readonly IEnumerable<ISensor> _sensors;

		public StartStreamCommand(IEnumerable<ISensor> sensors)
		{
			_sensors = sensors;
		}

		protected override byte OpCode
		{
			get { return 148; }
		}

		protected override IList<byte> SubsequentBytes
		{
			get
			{
				var sensorCount = new[]{ (byte) _sensors.Count() };
				var sensorPacketIds = _sensors.Select(sensor => sensor.PackedId);
				return sensorCount
						.Union(sensorPacketIds)
						.ToList();
			}
		}
	}
}