namespace Dotjosh.iRobot.Framework.Sensors
{
	public class CliffLeft : OneByteSensor
	{
		public override byte PackedId
		{
			get { return Cliff_Left; }
		}

		public bool IsCliff
		{
			get { return IsBitSet(0); }
		}
	}
}