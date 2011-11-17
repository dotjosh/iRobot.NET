namespace Dotjosh.iRobot.Tests.Core
{
	public interface ICommand
	{
		void Execute(IOCommunicator ioCommunicator);
	}
}