using Topshelf;

namespace Dotjosh.iRobot.Server
{
	class Program
	{
		static void Main(string[] args)
		{
			var h = HostFactory.New(x =>
			{
				const string serviceName = "irobot";
				x.Service<WebServer>(c =>
				{
					c.SetServiceName("WebServer");
					c.ConstructUsing(() => new WebServer());
					c.WhenStarted(d => d.Start());
					c.WhenStopped(d => d.Stop());
				});

				x.RunAsLocalSystem();
				x.SetDescription("iRobot.NET Web host and and Serial Port communicator");
				x.SetDisplayName("iRobot.NET Server");
				x.SetServiceName(serviceName);
			});

			h.Run();
		}
	}
}
