using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dotjosh.iRobot.Framework.Commands
{
	public class StartDemo : RobotCommand
	{
		private readonly RobotDemo _demo;

		public StartDemo(int demo)
		{
			_demo = (RobotDemo) demo;
		}

		protected override byte Opcode
		{
			get { return 136; }
		}

		protected override IList<byte> SubsequentBytes
		{
			get { return new[] { (byte) _demo }; }
		}

		public enum RobotDemo
		{
			Abort = -1,
			Cover = 0,
			CoverAndDock = 1,
			SpotCover = 2,
			Mouse = 3,
			DriveFigureEight = 4,
			Wimp = 5,
			Home = 6,
			Tag = 7,
			Pachelbel = 8,
			Banjo = 9
		}
	}
}
