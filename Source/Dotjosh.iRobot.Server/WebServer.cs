using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Dotjosh.iRobot.Framework;
using Dotjosh.iRobot.Framework.Sensors;
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
			_nancyHost = new NancyHost(new Uri("http://localhost:" + port), new Uri("http://127.0.0.1:" + port));
			_nancyHost.Start();
		}

		private void StopHttpServer()
		{
			if (_nancyHost == null) 
				return;
			_nancyHost.Stop();
			_nancyHost = null;
		}

		public class FakeIOCommunicator : IOCommunicator
		{
			public void Dispose()
			{
			
			}

			public event DataRecievedHandler DataRecieved;
			public void Write(byte[] bytes, int offset, int length)
			{
			}
		}
	}
}