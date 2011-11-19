using System.Collections.Generic;
using System.Linq;

namespace Dotjosh.iRobot.Framework.Commands
{
	public class DriveStop : Drive
	{
		protected override IList<byte> SubsequentBytes
		{
			get { return new List<byte>{ 0, 0, 0, 0 }; }
		} 
	}
}