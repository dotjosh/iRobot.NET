using System;

namespace Dotjosh.iRobot.Framework.Sensors
{
	public class BatteryCharge : UnsignedTwoByteSensor
	{
		public override byte PackedId
		{
			get { return Battery_Charge; }
		}

		public int Percentage
		{
			get { return (int) Math.Floor( ((decimal)Value/(decimal)UInt16.MaxValue)*100); }
		}
	}
}