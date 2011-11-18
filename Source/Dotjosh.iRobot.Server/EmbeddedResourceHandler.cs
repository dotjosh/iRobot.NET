using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using OpenRasta.Web;

namespace Dotjosh.iRobot.Server
{
	public class IndexEmbeddedResourceHandler  : EmbeddedResourceHandler
	{
		protected override string GetFileNameFromPath(string path)
		{
			return "index.html";
		}
	}

	public class EmbeddedResourceHandler
	{
		private Dictionary<string, Stream> _cache;
		private Assembly _assembly;

		public EmbeddedResourceHandler()
		{
			_assembly = GetType().Assembly;
			_cache = new Dictionary<string, Stream>();
		}

		public OperationResult Get(string path)
		{
			var fileName = "Dotjosh.iRobot.Server.Web." + GetFileNameFromPath(path);

			if (!_cache.ContainsKey(fileName))
			{
				Stream manifestResourceStream = null;
				try
				{
					manifestResourceStream = _assembly.GetManifestResourceStream(fileName);
				}
				catch (NullReferenceException){}
				_cache[fileName] = manifestResourceStream;
			}

			Stream stream = _cache[fileName];
			if(stream == null)
				return new OperationResult.NotFound();

			return new OperationResult.OK(stream);
		}

		protected virtual string GetFileNameFromPath(string path)
		{
			return path.Replace('/', '.');
		}
	}
}