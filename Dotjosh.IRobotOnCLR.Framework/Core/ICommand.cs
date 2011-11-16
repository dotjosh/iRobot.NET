namespace Dotjosh.IRobotOnCLR.Core
{
	public interface ICommand
	{
		void Execute(IOCommunicator ioCommunicator);
	}
}