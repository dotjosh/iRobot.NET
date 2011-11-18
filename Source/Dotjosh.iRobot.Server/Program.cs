using Topshelf;

namespace Dotjosh.iRobot.Server
{
	class Program
	{
		static void Main()
		{
			HostFactory.Run(x =>
			{
				x.SetDescription("iRobot.NET Web host and and Serial Port communicator");
				x.SetDisplayName("iRobot.NET Server");
				const string serviceName = "irobotdotnet";
				x.SetServiceName(serviceName);

				x.Service<Server>(c =>
				{
					c.SetServiceName(serviceName);
					c.ConstructUsing(() => new Server());
					c.WhenStarted(d => d.Start());
					c.WhenStopped(d => d.Stop());
				});
			});
		}
	}
}
