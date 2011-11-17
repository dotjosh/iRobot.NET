namespace Dotjosh.iRobot.Framework.Sensors
{
	public interface ISensor
	{
		byte PackedId { get; }
		int DataByteCount { get; }
		byte[] Bytes { set; }
	}
}