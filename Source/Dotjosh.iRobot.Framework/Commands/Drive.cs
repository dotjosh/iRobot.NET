using System.Collections.Generic;
using System.Linq;
using Dotjosh.iRobot.Framework.ExtensionMethods;

namespace Dotjosh.iRobot.Framework.Commands
{
	public class Drive : RobotCommand
	{
		private readonly short _velocity;
		private readonly short _radius;

		protected Drive(){}

		public Drive(short velocity, short radius)
		{
			AssertVelocityRange(velocity);
			radius.AssertRange(-2000, 2000);
			_velocity = velocity;
			_radius = radius;
		}

		protected override byte OpCode
		{
			get { return 137; }
		}

		protected override IList<byte> SubsequentBytes
		{
			get
			{
				return System.BitConverter.GetBytes(_velocity)
						.Union(System.BitConverter.GetBytes(_radius))
						.ToList();
			}
		}

		internal static void AssertVelocityRange(short velocity)
		{
			velocity.AssertRange(-500, 500);
		}
	}
}