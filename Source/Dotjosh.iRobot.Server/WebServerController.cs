using System;
using System.IO.Ports;
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
		public WebServerController()
		{
			Get[@"/"] = x => new GenericFileResponse("Web/index.html");

			Get[@"/(?<fileName>[a-zA-Z0-9-\.]+(?<extension>[.]js|[.]css|[.]png|[.]html))"] = x =>
			{
			    var fileName = ((string) x.fileName);
			    var relativeFileName = String.Format("Web/{0}",fileName);
		        return new GenericFileResponse(relativeFileName);
			};
		}
	}

	public class APIController : NancyModule
	{
		public APIController() : base("/API")
		{
			Get[@"/State"] = x =>
			{
				return Response.AsJson(
					new
						{
							IsConnected = RobotController != null && RobotController.IsConnected,
							IsStreaming = RobotController != null && RobotController.SensorStreamIsRunning,
							Sensors = RobotController != null ? RobotController.Sensors : new ISensor[] {},
							Ports =  SerialPort.GetPortNames()
						}							
				);
			};

			Post[@"/Connect"] = x =>
			{
				if(RobotController != null)
					return "";

				RobotController = RobotController.CreateWithAllSensors((string)Request.Form.portName);
				RobotController.Execute(new Start());
				RobotController.Execute(new SwitchToFullMode());
				return new Response();
			};

			Post[@"/Disconnect"] = x =>
			{
				if(RobotController != null)
				{
					RobotController.Dispose();
					RobotController = null;
				}

				return new Response();
			};

			Post[@"/StartStream"] = x =>
			{
			    if (RobotController != null)
			    {
			        RobotController.StartSensorStream();
			    }
				return new Response();
			};

			Post[@"/StopStream"] = x =>
			{
			    if (RobotController != null)
			    {
			        RobotController.StopSensorStream();
			    }
				return new Response();
			};

			Post[@"/Commands/{commandName}"] = x =>
			{
				if(RobotController == null)
					return "";

			    IRobotCommand command = this.Bind();
				RobotController.Execute(command);
			    return new Response();
			};		
		}

		public static RobotController RobotController { get; set; }
	}
}