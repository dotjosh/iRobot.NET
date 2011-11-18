using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using Nancy.Hosting.Self;

namespace Dotjosh.iRobot.Server
{
	public class Server
	{
		private NancyHost _nancyHost;

		public void Start()
		{
			Console.WriteLine("Start");

			Database.DefaultConnectionFactory = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");
			Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SqlCompactDataContext>());

			int port = 6687;
			_nancyHost = new NancyHost(new Uri("http://localhost:" + port), new Uri("http://127.0.0.1:" + port));
			_nancyHost.Start();

			}

		public void Stop()
		{
			Console.WriteLine("End");
			if (_nancyHost == null) 
				return;
			_nancyHost.Stop();
			_nancyHost = null;
		}
	}
}