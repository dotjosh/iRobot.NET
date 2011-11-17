using System.Collections.Generic;
using System.Linq;

namespace Dotjosh.iRobot.Framework.Commands
{
	public class DriveStraight : Drive
	{
		private readonly short _velocity;

		public DriveStraight(short velocity)
		{
			AssertVelocityRange(velocity);
			_velocity = velocity;
		}

		protected override IList<byte> SubsequentBytes
		{
			get
			{
				return System.BitConverter.GetBytes(_velocity)
					.Union(System.BitConverter.GetBytes(short.MaxValue))
					.ToList();
			}
		}
	}
}