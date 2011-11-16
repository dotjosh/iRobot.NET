using System;

namespace Dotjosh.IRobotOnCLR.Sensors
{
	public class BumpsAndWheelDrops : StandardSensor
	{
		public const int BitPosition_BumpRight = 0;
		public const int BitPosition_BumpLeft = 1;
		public const int BitPosition_WheeldropRight = 2;
		public const int BitPosition_WheeldropLeft = 3;
		public const int BitPosition_WheeldropCaster = 4;

		public override byte PackedId
		{
			get { return PacketIds.Bumps_And_WheelDrops; }
		}

		public override int DataByteCount
		{
			get { return 1; }
		}

		public override int Max
		{
			get { return 31; }
		}

		public bool BumpLeft
		{
			get { return IsBitSet(BitPosition_BumpLeft); }
		}

		public bool BumpRight
		{
			get { return IsBitSet(BitPosition_BumpRight); }
		}

		public bool WheelDropRight
		{
			get { return IsBitSet(BitPosition_WheeldropRight); }
		}

		public bool WheelDropLeft
		{
			get { return IsBitSet(BitPosition_WheeldropLeft); }
		}

		public bool WheelDropCaster
		{
			get { return IsBitSet(BitPosition_WheeldropCaster); }
		}
	}
}