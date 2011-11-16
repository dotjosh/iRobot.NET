using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using Dotjosh.IRobotOnCLR.HostService;

namespace Dotjosh.IRobotOnCLR.Client.Silverlight
{
	public class MainPageViewModel : INotifyPropertyChanged
	{
		private readonly Dispatcher _currentDispatcher;

		public MainPageViewModel(Dispatcher currentDispatcher)
		{
			_currentDispatcher = currentDispatcher;

			var endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4502);
			_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			SocketAsyncEventArgs args = new SocketAsyncEventArgs();
			args.UserToken = _socket;
			args.RemoteEndPoint = endPoint;
			args.Completed += OnSocketConnectCompleted;
			_socket.ConnectAsync(args);  
		}


		private SensorStatus _sensorStatus;
		private Socket _socket;

		public SensorStatus SensorStatus
		{
			get { return _sensorStatus; }
			set
			{
				_sensorStatus = value;
				PropertyHasChanged("SensorStatus");
			}
		}

		private void OnSocketConnectCompleted(object sender, SocketAsyncEventArgs e)
		{
			byte[] response = new byte[1024];
			e.SetBuffer(response, 0, response.Length);
			e.Completed -= OnSocketConnectCompleted;
			e.Completed += OnSocketReceive;
			Socket socket = (Socket)e.UserToken;
			socket.ReceiveAsync(e);
		}

		private void OnSocketReceive(object sender, SocketAsyncEventArgs e)
		{
			try
			{
				_currentDispatcher.BeginInvoke(() => {
					if (e.BytesTransferred == 0)
						return;
					var serializer = new DataContractSerializer(typeof (SensorStatus));
					using (var stream = new MemoryStream(e.Buffer, e.Offset, e.BytesTransferred))
					{
						SensorStatus = (SensorStatus) serializer.ReadObject(stream);
					}
				});
			}
			finally
			{
				Socket socket = (Socket)e.UserToken;
				socket.ReceiveAsync(e);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void PropertyHasChanged(string property)
		{
			if(PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(property));
		}

		public void SendData()
		{
			var socketAsyncEventArgs = new SocketAsyncEventArgs();
			DataContractSerializer serializer = new DataContractSerializer(typeof(DriveClientToServerCommand));

			using(var stream = new MemoryStream())
			{
				serializer.WriteObject(stream, new DriveClientToServerCommand());
				var bytes = stream.ToArray();
				socketAsyncEventArgs.SetBuffer(bytes, 0, bytes.Length);
			}
			_socket.SendAsync(socketAsyncEventArgs);
		}

		private static byte[] StringToByteArray(string text)
		{
			return new UTF8Encoding().GetBytes(text);
		}
	}
}