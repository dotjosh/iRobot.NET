using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Threading;
using OpenRasta.Codecs;
using OpenRasta.Configuration;
using OpenRasta.DI;
using OpenRasta.Hosting;
using OpenRasta.Hosting.HttpListener;
using OpenRasta.IO;
using OpenRasta.Web;

namespace Dotjosh.iRobot.Server
{
	public class Server
	{
		private AppDomainHost<ConfigurationExecutorHost> _http;

		public void Start()
		{
			Console.WriteLine("Start");

			Database.DefaultConnectionFactory = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");
			Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SqlCompactDataContext>());

			int port = 6687;
            _http = new AppDomainHost<ConfigurationExecutorHost>(new[] { string.Format("http://+:{0}/", port) }, "/",null);
            _http.Initialize();
			_http.Listener.Configure(() =>
			{
//			    ResourceSpace.Has.ResourcesOfType<Test>()
//			        .AtUri("/test/{id}")
//			        .HandledBy<CustomerHandler>()
//			        .TranscodedBy<JsonDataContractCodec>(null);

				ResourceSpace.Has
					.ResourcesOfType<Stream>()
					.AtUri("/{path}")
					.HandledBy<EmbeddedResourceHandler>()
					.TranscodedBy<ApplicationOctetStreamCodec>().ForMediaType("text/html").ForExtension(".html")
					.And.TranscodedBy<ApplicationOctetStreamCodec>().ForMediaType("text/javascript").ForExtension(".js")
					.And.TranscodedBy<ApplicationOctetStreamCodec>().ForMediaType("text/css").ForExtension(".css")
					.And.TranscodedBy<ApplicationOctetStreamCodec>().ForMediaType("image/png").ForExtension(".png");
			});
			_http.StartListening();
		}

		public void Stop()
		{
			Console.WriteLine("End");
			if (_http == null) 
				return;
			_http.StopListening();
			_http = null;
		}
	}

	public class ConfigurationExecutorHost : HttpListenerHost
	{
		private Action t;

		public void Configure(Action t)
		{
			this.t = t;
		}

		public override bool ConfigureLeafDependencies(IDependencyResolver resolver)
		{
			using (OpenRastaConfiguration.Manual)
				t();
			return true;
		}
	}
}