using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Dotjosh.IRobotOnCLR.Core;
using Dotjosh.IRobotOnCLR.Sensors;

namespace Dotjosh.IRobotOnCLR.HostService
{
	class Program
	{
		private static TcpListener _tcpListener;
		private static readonly List<Socket> _clients = new List<Socket>();
		private static BumpsAndWheelDrops _bumpsAndWheelDrops;

		static void Main()
		{
			_robotController = CreateRobotController();
			_robotController.SensorsUpdated += robotController_SensorsUpdated;
			_bumpsAndWheelDrops = new BumpsAndWheelDrops();
			var sensors = new List<ISensor>
			                         	{
			                         		_bumpsAndWheelDrops
			                         	};

			_tcpListener = new TcpListener(IPAddress.Any, 4502);
			_tcpListener.Start();
			BeginAcceptClient();
			_robotController.StartStreamingSensorUpdates(sensors);
			Console.ReadLine();
		}

		private static void BeginAcceptClient()
		{
			_tcpListener.BeginAcceptSocket(OnBeginAccept, null);
		}

		private static void OnBeginAccept(IAsyncResult ar)
		{
			Console.WriteLine("Accepting new client connection.");
			var socket = _tcpListener.EndAcceptSocket(ar);
			lock(_clients)
			{
				_clients.Add(socket);
			}
			if (_lastSensorStatus != null)
				SendSensorStatus(_lastSensorStatus);
			BeginAcceptRecieve(socket);
			BeginAcceptClient();
		}

		public class StateObject
		{
			public Socket Socket;
			public const int BUFFER_SIZE = 1024 * 32;
			public byte[] Buffer = new byte[BUFFER_SIZE];
			public StringBuilder sb = new StringBuilder();
		}

		private static void BeginAcceptRecieve(Socket socket)
		{
			var so = new StateObject();
			so.Socket = socket;
			socket.BeginReceive(so.Buffer, 0, so.Buffer.Length, 0, new AsyncCallback(AcceptRecieveComplete), so);
		}

		private static void AcceptRecieveComplete(IAsyncResult ar)
		{
			var so = (StateObject)ar.AsyncState;
			var socket = so.Socket;
			var read = socket.EndReceive(ar);
			using (var stream = new MemoryStream(so.Buffer, 0, read))
			{
				var document = new XmlDocument();
				document.Load(stream);
				stream.Position = 0;
				var xmlReader = XmlReader.Create(stream);
				var commandType = Type.GetType("Dotjosh.IRobotOnCLR.HostService." + document.DocumentElement.Name);
				var serializer = new DataContractSerializer(commandType);
				var command = (ClientToServerCommand)serializer.ReadObject(xmlReader);
				Console.WriteLine("Executing command " + command);
				command.Execute(_robotController);
			}
			BeginAcceptRecieve(socket);
		}

		private static SensorStatus _lastSensorStatus;
		private static RobotController _robotController;

		private static void robotController_SensorsUpdated()
		{
			var sensorStatus = CreateSensorStatus();
			if(!Equals(_lastSensorStatus, sensorStatus))
			{
				_lastSensorStatus = sensorStatus;
				SendSensorStatus(sensorStatus);
			}
		}

		private static void SendSensorStatus(SensorStatus sensorStatus)
		{
			var serializer = new DataContractSerializer(typeof(SensorStatus));
			using(var stream = new MemoryStream())
			{
				serializer.WriteObject(stream, sensorStatus);
				SendData(Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int)stream.Position));
			}
		}

		private static SensorStatus CreateSensorStatus()
		{
			var status = new SensorStatus();
			status.BumpLeft	= _bumpsAndWheelDrops.BumpLeft;
			status.BumpRight = _bumpsAndWheelDrops.BumpRight;
			return status;
		}

		private static void SendData(string data)
		{
			foreach (var socket in _clients)
			{
				socket.Send(StringToByteArray(data));
			}
		}

		private static byte[] StringToByteArray(string text)
		{
			return new UTF8Encoding().GetBytes(text);
		}

		private static RobotController CreateRobotController()
		{
			var fakeIOCommunicator = new FakeIOCommunicator();
			var controller = new RobotController(fakeIOCommunicator);
			return controller;
		}
	}
}
