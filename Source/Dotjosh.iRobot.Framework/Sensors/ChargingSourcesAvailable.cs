namespace Dotjosh.iRobot.Framework.Sensors
{
	public class ChargingSourcesAvailable : OneByteSensor
	{
		public override byte PackedId
		{
			get { return ChargingSources_Available; }
		}

		public bool InternalCharger
		{
			get { return IsBitSet(0); }
		}

		public bool HomeBase
		{
			get { return IsBitSet(1); }
		}
	}
}