using System.Runtime.Serialization;

#if !SILVERLIGHT
using Dotjosh.IRobotOnCLR.Commands;
using Dotjosh.IRobotOnCLR.Core;
#endif

namespace Dotjosh.IRobotOnCLR.HostService
{
	[DataContract(Namespace = "Dotjosh.IRobotOnCLR.HostService")]
	[KnownType(typeof(DriveClientToServerCommand))]
	public abstract class ClientToServerCommand
	{
		#if !SILVERLIGHT
				public abstract void Execute(RobotController robotController);
		#endif
	}

	public class DriveClientToServerCommand : ClientToServerCommand
	{
		[DataMember]
		public int Velocity { get; set; }

		[DataMember]
		public int Angle { get; set; }

#if !SILVERLIGHT
		public override void Execute(RobotController robotController)
		{
			var driveCommand = new DriveCommand(Velocity, Angle);
			robotController.Execute(driveCommand);
		}
#endif
	}
}
