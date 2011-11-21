namespace Dotjosh.iRobot.Framework.Sensors
{
	public class Distance : SignedTwoByteSensor
	{
		public override byte PackedId
		{
			get { return Distance; }
		}

		protected override void OnBytesUpdated()
		{
			TotalMillimetersTraveled += Value;
		}

		public int TotalMillimetersTraveled { get; set; }
	}
}