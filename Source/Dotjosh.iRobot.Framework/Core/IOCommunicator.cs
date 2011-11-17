﻿using System;

namespace Dotjosh.iRobot.Tests.Core
{
	public delegate void DataRecievedHandler(byte[] newBytes);

	public interface IOCommunicator : IDisposable
	{
		event DataRecievedHandler DataRecieved;
		void Write(byte[] bytes, int offset, int length);
	}
}