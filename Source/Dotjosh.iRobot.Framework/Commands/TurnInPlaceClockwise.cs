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
				return Bytes(_velocity)
					.Concat(Bytes(ushort.MaxValue))
					.ToList();
			}
		}
	}
}