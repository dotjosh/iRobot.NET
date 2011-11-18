using System.Data.Entity;

namespace Dotjosh.iRobot.Server
{
	public class SqlCompactDataContext : DbContext
	{
		public DbSet<SensorState> SensorStates { get; set; }
	}
}