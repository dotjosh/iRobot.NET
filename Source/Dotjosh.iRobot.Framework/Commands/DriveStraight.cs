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
				return Bytes(_velocity)
					.Concat(Bytes((short)32767))
					.ToList();
			}
		}
	}
}