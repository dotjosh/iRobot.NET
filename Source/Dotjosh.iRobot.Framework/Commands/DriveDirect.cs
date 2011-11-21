using System.Collections.Generic;
using System.Linq;
using Dotjosh.iRobot.Framework.ExtensionMethods;

namespace Dotjosh.iRobot.Framework.Commands
{
	public class DriveDirect : RobotCommand
	{
		private readonly short _leftVelocity;
		private readonly short _rightVelocity;

		public DriveDirect(short leftVelocity, short rightVelocity)
		{
			leftVelocity.AssertRange(-500, 500);
			rightVelocity.AssertRange(-500, 500);
			_leftVelocity = leftVelocity;
			_rightVelocity = rightVelocity;
		}

		protected override byte Opcode
		{
			get { return 145; }
		}

		protected override IList<byte> SubsequentBytes
		{
			get { 
				return Bytes(_rightVelocity)
							.Concat(Bytes(_leftVelocity))
							.ToArray(); 
			}
		}
	}
}