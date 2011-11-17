namespace Dotjosh.iRobot.Framework.Commands
{
	public interface ICommand
	{
		void Execute(IOCommunicator ioCommunicator);
	}
}