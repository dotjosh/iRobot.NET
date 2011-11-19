using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dotjosh.iRobot.Framework.Commands;
using Nancy;
using Nancy.ModelBinding;

namespace Dotjosh.iRobot.Server
{
	public class RobotCommandModelBinder : IModelBinder
	{
		private static readonly IEnumerable<Type> AvailableCommands = typeof (IRobotCommand).Assembly.GetTypes().Where(t => typeof(IRobotCommand).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass);

		public bool CanBind(Type modelType)
		{
			return modelType == typeof (IRobotCommand);
		}

		public object Bind(NancyContext context, Type modelType, params string[] blackList)
		{
			var commandType = AvailableCommands.FirstOrDefault(c => c.Name == context.Parameters.commandName);
			if(commandType == null)
			{
				return null;	
			}

			var expectedParams = commandType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).First().GetParameters();
			var sentParams = expectedParams.Select(p =>
			                                       	{
			                                       		if(p.ParameterType == typeof(Byte))
			                                       		{
			                                       			return Byte.Parse(context.Request.Form[p.Name]);
			                                       		}
			                                       		if(p.ParameterType == typeof(short))
			                                       		{
			                                       			return short.Parse(context.Request.Form[p.Name]);
			                                       		}
			                                       		if(p.ParameterType == typeof(ushort))
			                                       		{
			                                       			return ushort.Parse(context.Request.Form[p.Name]);
			                                       		}
			                                       		if(p.ParameterType == typeof(Int16))
			                                       		{
			                                       			return Int16.Parse(context.Request.Form[p.Name]);
			                                       		}
			                                       		if(p.ParameterType == typeof(UInt16))
			                                       		{
			                                       			return UInt16.Parse(context.Request.Form[p.Name]);
			                                       		}
			                                       		if(p.ParameterType == typeof(Int32))
			                                       		{
			                                       			return Int32.Parse(context.Request.Form[p.Name]);
			                                       		}
			                                       		if(p.ParameterType == typeof(UInt32))
			                                       		{
			                                       			return UInt32.Parse(context.Request.Form[p.Name]);
			                                       		}
			                                       		throw new Exception();
			                                       	}
				);
			return Activator.CreateInstance(commandType, sentParams.ToArray());
		}
	}
}