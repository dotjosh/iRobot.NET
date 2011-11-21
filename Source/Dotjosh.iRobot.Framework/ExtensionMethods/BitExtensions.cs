using System;
using System.Collections;

namespace Dotjosh.iRobot.Framework.ExtensionMethods
{
	public static class BitExtensions
	{		
		public static byte ToByte(this BitArray bits)
		{
			if (bits.Count != 8)
				throw new Exception("A byte has 8 bits, wtf you only passed in " + bits.Count);
			byte[] bytes = new byte[1];
			bits.CopyTo(bytes, 0);
			return bytes[0];
		} 
	}
}