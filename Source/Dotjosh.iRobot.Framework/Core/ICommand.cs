namespace Dotjosh.iRobot.Framework.Core
{
	public interface ICommand
	{
		void Execute(IOCommunicator ioCommunicator);
	}
}