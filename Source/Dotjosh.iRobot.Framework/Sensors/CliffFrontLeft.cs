namespace Dotjosh.iRobot.Framework.Sensors
{
	public class CliffFrontLeft : OneByteSensor
	{
		public override byte PackedId
		{
			get { return Cliff_FrontLeft; }
		}

		public bool IsCliff
		{
			get { return IsBitSet(0); }
		}
	}
}