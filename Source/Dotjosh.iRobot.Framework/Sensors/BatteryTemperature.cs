namespace Dotjosh.iRobot.Framework.Sensors
{
	public class BatteryTemperature : OneByteSensor
	{
		public override byte PackedId
		{
			get { return Battery_Temperature; }
		}

		public string Temperature
		{
			get { return string.Format("{0}(°C)", Value); }
		}
	}
}