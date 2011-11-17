using System.Collections.Generic;
using System.Linq;

namespace Dotjosh.iRobot.Framework.Commands
{
	public class DriveCommand : StandardCommand
	{
		private readonly int _speed;
		private readonly int _angle;

		protected DriveCommand(){}

		public DriveCommand(int speed, int angle)
		{
			_speed = speed;
			_angle = angle;
		}

		protected override byte OpCode
		{
			get { return 137; }
		}

		protected override IList<byte> SubsequentBytes
		{
			get
			{
				return System.BitConverter.GetBytes(_angle)
						.Union(System.BitConverter.GetBytes(_speed))
						.ToList();
			}
		}
	}
}