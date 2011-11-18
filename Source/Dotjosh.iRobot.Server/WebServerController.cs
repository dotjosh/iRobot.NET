using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using Dotjosh.iRobot.Framework;
using Dotjosh.iRobot.Framework.Commands;
using Dotjosh.iRobot.Framework.Sensors;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;

namespace Dotjosh.iRobot.Server
{
	public class WebServerController : NancyModule
	{
		public static RobotController RobotController { get; set; }


		public WebServerController()
		{
			Get[@"/"] = x =>
			            	{
								return new GenericFileResponse("Web/index.html");
			            	};

			Get[@"/(?<fileName>[a-zA-Z0-9-\.]+(?<extension>[.]js|[.]css|[.]png|[.]html))"] = x =>
			                                               	{
			                                               		var fileName = ((string) x.fileName);
			                                               		var relativeFileName = String.Format("Web/{0}",fileName);
		                                               			return new GenericFileResponse(relativeFileName);
			                                               	};

			Get[@"/API/GetPortNames"] = x =>
			                                            	{
																string[] ports = SerialPort.GetPortNames();
																return Response.AsJson(ports);
			                                             	};

			Get[@"/API/Start"] = x =>
			                                            	{
																if(RobotController != null)
																	throw new Exception("Robot is already running.  Please use /API/Stop");

																RobotController = RobotController.CreateWithAllSensors((string)Request.Query.portName);
																RobotController.CommandExecuted += command =>
			                                   														{
			                                   															Console.WriteLine(command.ToString());
			                                   														};
																RobotController.RequestSensorUpdates();
																return new Response();
			                                             	};

			Get[@"/API/Stop"] = x =>
			                                            	{
																if(RobotController != null)
																{
																	RobotController.Dispose();
																	RobotController = null;
																}

																return new Response();
			                                             	};

			Get[@"/API/ExecuteCommand/{commandName}"] = x =>
			                                            	{
																if(RobotController == null)
																	throw new Exception("Robot must be started first.  Please use /API/Start");

			                                            		IRobotCommand command = this.Bind();
																RobotController.Execute(command);
			                                            		return new Response();
			                                            	};
		}
	}
}