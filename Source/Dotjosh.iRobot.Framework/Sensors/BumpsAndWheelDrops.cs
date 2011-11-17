namespace Dotjosh.iRobot.Framework.Sensors
{
	public class BumpsAndWheelDrops : OneByteSensor
	{
		public override byte PackedId
		{
			get { return Bumps_And_WheelDrops; }
		}

		public bool BumpRight
		{
			get { return IsBitSet(0); }
		}

		public bool BumpLeft
		{
			get { return IsBitSet(1); }
		}

		public bool WheelDropRight
		{
			get { return IsBitSet(2); }
		}

		public bool WheelDropLeft
		{
			get { return IsBitSet(3); }
		}

		public bool WheelDropCaster
		{
			get { return IsBitSet(4); }
		}
	}
}