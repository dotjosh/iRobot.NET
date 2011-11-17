using System;
using Dotjosh.iRobot.Tests.Core;

namespace Dotjosh.iRobot.Tests.Sensors
{
	public abstract class StandardSensor : ISensor
	{
		public abstract byte PackedId { get; }
		public abstract int DataByteCount { get; }
		public abstract int Max { get; }
		public int Value { get; private set; }

		protected bool IsBitSet(int bitPosition)
		{
			return (Value & (int)Math.Pow(2, bitPosition)) != 0;
		}

		public void Handle(byte[] newDataBytes)
		{
            var result = 0;

            //We have to look at Max & Signed to know what type to cast this to.
            switch (Max)
            {
                case 255:
                case 65535:
                    result = (newDataBytes[0] << 8) | newDataBytes[1]; //(UInt16)

                    break;

                case 4095:
                    result = (newDataBytes[0] * 256) + newDataBytes[1];
                    break;

                case 1:
                case 3:
                case 5:
                case 31:
                case 43:
                    result = newDataBytes[0]; //No conversion needed here.
                    break;

                case 500:
                case 32767:
                    //Angle & Distance, Req Velocity..
                    result = (newDataBytes[0] << 8) | newDataBytes[1]; //(short)
                    break;

                case 127:
                    result = newDataBytes[0];
                    break;

                case 1023:
                    break;
            }

			Value = result;
		}
	}
}