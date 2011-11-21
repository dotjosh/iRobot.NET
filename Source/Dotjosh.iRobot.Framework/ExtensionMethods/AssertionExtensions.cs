using System;

namespace Dotjosh.iRobot.Framework.ExtensionMethods
{
	public static class AssertionExtensions
	{
		public static void AssertRange(this short number, short min, short max)
		{
			if(number < min || number > max)
			{
				throw new ArgumentOutOfRangeException(string.Format("The number {0} is out of the range between {1} and {2}", number, min, max));
			}
		}
	}
}