using System.Collections.Generic;
using System.Linq;
using Dotjosh.iRobot.Framework.Sensors;

namespace Dotjosh.iRobot.Framework.Commands
{
	public class RequestSensorStream : RobotCommand
	{
		private readonly IEnumerable<ISensor> _sensors;

		public RequestSensorStream(IEnumerable<ISensor> sensors)
		{
			_sensors = sensors;
		}

		protected override byte Opcode
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
						.Concat(sensorPacketIds)
						.ToList();
			}
		}
	}
}