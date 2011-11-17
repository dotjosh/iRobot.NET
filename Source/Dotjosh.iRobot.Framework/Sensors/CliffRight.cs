namespace Dotjosh.iRobot.Framework.Sensors
{
	public class CliffRight : OneByteSensor
	{
		public override byte PackedId
		{
			get { return Cliff_Right; }
		}

		public bool IsCliff
		{
			get { return IsBitSet(0); }
		}
	}
}