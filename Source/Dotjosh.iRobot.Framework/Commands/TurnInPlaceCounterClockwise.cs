using System.Collections.Generic;
using System.Linq;

namespace Dotjosh.iRobot.Framework.Commands
{
	public class TurnInPlaceCounterClockwise : Drive
	{
		private readonly short _velocity;

		public TurnInPlaceCounterClockwise(short velocity)
		{
			AssertVelocityRange(velocity);
			_velocity = velocity;
		}

		protected override IList<byte> SubsequentBytes
		{
			get
			{
				return System.BitConverter.GetBytes(_velocity)
					.Union(System.BitConverter.GetBytes((short)1))
					.ToList();
			}
		}
	}
}