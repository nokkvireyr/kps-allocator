
using Microsoft.EntityFrameworkCore;
namespace KPSAllocator.Modules.Database;

public partial class Database : DbContext
{
  public DbSet<PlayerDatabase> Players { set; get; }
  public static Database? In { set; get; }

  public static Database Instance()
  {
    if (In is null)
    {
      In = new Database();
    }
    return In;
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    var config = KPSAllocator.DBConfig?.ConfigData;
    if (config is null)
    {
      throw new Exception("Config data is null");
    }
    optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    switch (config.engine)
    {
      case "mysql":
        var connectionString = @$"Server={config.Host}; Database={config.Database}; Port={config.Port}; User ID={config.User}; Password={config.Pass};";
        optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(ServerVersion.AutoDetect(connectionString)));
        break;
      default:
        optionsBuilder.UseSqlite("Data Source=kps_allocator.db");
        break;
    }
  }
}