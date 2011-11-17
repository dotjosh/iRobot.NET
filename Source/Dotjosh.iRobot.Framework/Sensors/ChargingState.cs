namespace Dotjosh.iRobot.Framework.Sensors
{
	public class ChargingState : OneByteSensor
	{
		public override byte PackedId
		{
			get { return Charging_State; }
		}

		public ChargingStateState State
		{
			get
			{
				return (ChargingStateState) Value;
			}
		}

		public enum ChargingStateState
		{
			NotCharging = 0,
			ReconditioningCharging = 1,
			FullCharging = 2,
			TrickleCharging = 3,
			Waiting = 4,
			ChargingFaultCondition = 5
		}
	}
}