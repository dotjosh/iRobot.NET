using System.Runtime.Serialization;

namespace Dotjosh.IRobotOnCLR.HostService
{
	[DataContract]
	public class SensorStatus
	{
		[DataMember]
		public bool BumpLeft { get; set; }
		[DataMember]
		public bool BumpRight { get; set; }

		public override bool Equals(object obj)
		{
			var other = obj as SensorStatus;
			if (other == null)
				return true;

			return 
				BumpLeft == other.BumpLeft
				&& BumpRight == other.BumpRight;
		}
	}
}