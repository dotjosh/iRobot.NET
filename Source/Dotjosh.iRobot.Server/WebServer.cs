using System;
using System.IO;
using Nancy.Hosting.Self;
using Nancy.Responses;

namespace Dotjosh.iRobot.Server
{
	public class WebServer
	{
		private NancyHost _nancyHost;

		public void Start()
		{
			Console.WriteLine("Start");
			StartHttpServer();
		}

		public void Stop()
		{
			Console.WriteLine("End");
			StopHttpServer();
		}

		private void StartHttpServer()
		{
			int port = 6687;
			Nancy.StaticConfiguration.DisableCaches = true;
#if DEBUG
			var thisExeFullName = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
			var thisProjectRootDirectory = thisExeFullName.Directory.Parent.Parent;
			GenericFileResponse.SafePaths.Clear();
			GenericFileResponse.SafePaths.Add(thisProjectRootDirectory.FullName);
#endif
			_nancyHost = new NancyHost(new Uri("http://localhost:" + port), new Uri("http://127.0.0.1:" + port), new Uri("http://joshslaptop:6687"));
			_nancyHost.Start();
		}

		private void StopHttpServer()
		{
			if (_nancyHost == null) 
				return;
			_nancyHost.Stop();
			_nancyHost = null;
		}
	}
}