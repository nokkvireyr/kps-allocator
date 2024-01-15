
using KPSAllocator.Modules.Config;
using Microsoft.EntityFrameworkCore;
namespace KPSAllocator.Modules.Database;

public partial class Database : DbContext
{
  public DbSet<PlayerDatabase> Players { set; get; }
  // public static Database? In { set; get; }

  // public static Database Instance()
  // {
  //   if (In is null)
  //   {
  //     In = new Database();
  //   return In;
  // }


  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    try
    {
      var config = KPSAllocator.DBConfig?.ConfigData;
      if (config is null)
      {
        throw new Exception("Config data is null");
      }
      optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
      if (config.engine != "mysql")
      {
        Utils.SqlLiteSetup(optionsBuilder);
      }
      else
      {
        Utils.MySQLSetup(config, optionsBuilder);
        return;
      }
    }
    catch (Exception ex)
    {
      Utils.Log($"Databaser error. Something went wrong. {ex.Message}");
    }
  }

  public void Close()
  {
    Dispose();
  }

}

