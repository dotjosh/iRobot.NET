using System;
using System.Collections;
using System.Collections.Generic;
using Dotjosh.iRobot.Framework.ExtensionMethods;

namespace Dotjosh.iRobot.Framework.Commands
{
	public class LEDs : RobotCommand
	{
		private readonly bool _play;
		private readonly bool _advance;
		private readonly byte _color;
		private readonly byte _intensity;

		public LEDs(bool play, bool advance, byte color, byte intensity)
		{
			_play = play;
			_advance = advance;
			_color = color;
			_intensity = intensity;
		}

		protected override byte Opcode
		{
			get { return 139; }
		}

		protected override IList<byte> SubsequentBytes
		{
			get
			{
				var ledByte = new BitArray(new [] {false, _play, false, _advance, false, false, false, false }).ToByte();
				return new[]
				       	{
				       		ledByte,
				       		_color,
				       		_intensity
				       	};
			}
		}
	}
}