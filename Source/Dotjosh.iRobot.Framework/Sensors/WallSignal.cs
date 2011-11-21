namespace Dotjosh.iRobot.Framework.Sensors
{
	public class WallSignal : UnsignedTwoByteSensor
	{
		public override byte PackedId
		{
			get { return WallSignal; }
		}

		public int Range
		{
			get { return Value; }
		}
	}
}