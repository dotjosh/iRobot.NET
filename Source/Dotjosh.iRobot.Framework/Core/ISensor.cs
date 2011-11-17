namespace Dotjosh.iRobot.Framework.Core
{
	public interface ISensor
	{
		byte PackedId { get; }
		int DataByteCount { get; }
		void Handle(byte[] newNewDataBytes);
	}
}