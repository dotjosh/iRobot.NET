namespace Dotjosh.iRobot.Framework.Sensors
{
	public class Wall : OneByteSensor
	{
		public override byte PackedId
		{
			get { return Wall; }
		}

		public bool IsWallSeen
		{
			get { return IsBitSet(0); }
		}
	}
}