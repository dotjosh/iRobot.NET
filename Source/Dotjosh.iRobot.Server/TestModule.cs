using System;
using System.Collections.Generic;
using System.IO;
using Nancy;

namespace Dotjosh.iRobot.Server
{
   public class TestModule : NancyModule
    {
   	private static IDictionary<string, Stream> _cache;

   	public TestModule()
        {
            Get["/"] = _ => "Hello!";

//            Get["/testing"] = parameters =>
//            {
//                return Response.FromStream()["staticview", Nancy.Request.Url];
//            };
        }
//
//		private Stream Stream(string path)
//		{
//			var fileName = "Dotjosh.iRobot.Server.Web." + GetFileNameFromPath(path);
//
//			if (!_cache.ContainsKey(fileName))
//			{
//				Stream manifestResourceStream = null;
//				try
//				{
//					manifestResourceStream = _assembly.GetManifestResourceStream(fileName);
//				}
//				catch (NullReferenceException)
//				{
//				}
//				_cache[fileName] = manifestResourceStream;
//			}
//
//			Stream stream = _cache[fileName];
//			return stream;
//		}
    }

}