using System.Collections.Generic;

namespace Dotjosh.IRobotOnCLR.Commands
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
				byte byAngleHi = (byte)(_angle >> 8);
				byte byAngleLo = (byte)(_angle & 255);
				byte bySpeedHi = (byte)(_speed >> 8);
				byte bySpeedLo = (byte)(_speed & 255);

				return new List<byte>
				       	{
				       		byAngleHi,
							byAngleLo,
							bySpeedHi,
							bySpeedLo
				       	};
			}
		}
	}
}