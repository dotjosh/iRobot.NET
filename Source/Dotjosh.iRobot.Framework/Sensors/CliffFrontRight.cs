namespace Dotjosh.iRobot.Framework.Sensors
{
	public class CliffFrontRight : OneByteSensor
	{
		public override byte PackedId
		{
			get { return Cliff_FrontRight; }
		}

		public bool IsCliff
		{
			get { return IsBitSet(0); }
		}
	}
}