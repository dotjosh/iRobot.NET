using System.Collections.Generic;
using System.Linq;

namespace Dotjosh.iRobot.Framework.Commands
{
	public class TurnInPlaceClockwise : Drive
	{
		private readonly short _velocity;

		public TurnInPlaceClockwise(short velocity)
		{
			AssertVelocityRange(velocity);
			_velocity = velocity;
		}

		protected override IList<byte> SubsequentBytes
		{
			get
			{
				return System.BitConverter.GetBytes(_velocity)
					.Union(System.BitConverter.GetBytes(ushort.MaxValue))
					.ToList();
			}
		}
	}
}