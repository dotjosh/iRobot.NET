using System;

namespace Dotjosh.iRobot.Framework.Sensors
{
	public abstract class OneByteSensor : SensorBase
	{
		public override int Value
		{
			get { return Bytes[0]; }
		}

		public override int DataByteCount
		{
			get { return 1; }
		}
	}

	public abstract class SignedTwoByteSensor : SensorBase
	{
		public override int Value
		{
			get { return BitConverter.ToInt16(Bytes, 0); }
		}

		public override int DataByteCount
		{
			get { return 2; }
		}
	}

	public abstract class UnsignedTwoByteSensor : SensorBase
	{
		public override int Value
		{
			get { return BitConverter.ToUInt16(Bytes, 0); }
		}

		public override int DataByteCount
		{
			get { return 2; }
		}
	}

	public abstract class SensorBase : ISensor
	{
		public abstract byte PackedId { get; }
		public abstract int DataByteCount { get; }

		public byte[] Bytes { protected get; set; }
		public abstract int Value { get; }

		protected bool IsBitSet(int bitPosition)
		{
			return (Value & (int)Math.Pow(2, bitPosition)) != 0;
		}

		#region Sensor Packet Codes
		public const byte All = 6;
		public const byte Bumps_And_WheelDrops = 7;
		public const byte Wall = 8;
		public const byte Cliff_Left = 9;
		public const byte Cliff_FrontLeft = 10;
		public const byte Cliff_FrontRight = 11;
		public const byte Cliff_Right = 12;
		public const byte Virtual_Wall = 13;
		public const byte LowSideDriver_And_WheelOvercurrents = 14;
		public const byte UnusedByte1 = 15;
		public const byte UnusedByte2 = 16;
		public const byte Infrared_Byte = 17;
		public const byte Buttons = 18;
		public const byte Distance = 19;
		public const byte Angle = 20;
		public const byte Charging_State = 21;
		public const byte Voltage = 22;
		public const byte Current = 23;
		public const byte Battery_Temperature = 24;
		public const byte Battery_Charge = 25;
		public const byte Battery_Capacity = 26;
		public const byte WallSignal = 27;
		public const byte CliffLeft_Signal = 28;
		public const byte CliffFrontLeft_Signal = 29;
		public const byte CliffFrontRight_Signal = 30;
		public const byte CliffRight_Signal = 31;
		public const byte CargoBay_DigitalInputs = 32;
		public const byte CargoBay_AnalogSignal = 33;
		public const byte ChargingSources_Available = 34;
		public const byte OI_Mode = 35; //expect quite a bit of use out of this one. This one is always very helpful to know
		public const byte Song_Number = 36;
		public const byte Song_Playing = 37;
		public const byte Number_Of_StreamPackets = 38;
		public const byte Requested_Velocity = 39;
		public const byte Requested_Radius = 40;
		public const byte Requested_Right_Velocity = 41;
		public const byte Requested_Left_Velocity = 42;

		#endregion

	}
}