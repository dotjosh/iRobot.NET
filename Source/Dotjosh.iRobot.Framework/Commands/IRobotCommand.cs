namespace Dotjosh.iRobot.Framework.Commands
{
	public interface IRobotCommand
	{
		void Execute(IOCommunicator ioCommunicator);
	}
}