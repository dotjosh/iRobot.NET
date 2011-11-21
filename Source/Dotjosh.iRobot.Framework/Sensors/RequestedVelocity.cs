namespace Dotjosh.iRobot.Framework.Sensors
{
	public class RequestedVelocity : SignedTwoByteSensor
	{
		public override byte PackedId
		{
			get { return Requested_Velocity; }
		}

		public int Velocity
		{
			get { return Value; }
		}
	}
}