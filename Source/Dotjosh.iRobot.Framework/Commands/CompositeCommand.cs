using System.Collections.Generic;

namespace Dotjosh.iRobot.Framework.Commands
{
	public abstract class CompositeCommand : IRobotCommand
	{
		public void Execute(IOCommunicator ioCommunicator)
		{
			foreach(var command in Commands)
			{
				command.Execute(ioCommunicator);
			}
		}

		public abstract IEnumerable<IRobotCommand> Commands { get; }
	}
}