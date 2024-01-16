namespace KPSAllocator.Modules.Config;

public class DatabaseConfigData : IBaseConfigData
{
  public string engine { set; get; } = "sql";
  public string Host { set; get; } = "localhost";
  public int Port { set; get; } = 3306;
  public string User { set; get; } = "user";
  public string Pass { set; get; } = "pass";
  public string Database { set; get; } = "database";
  public string Version { set; get; } = "1.0.0";
}

public class DatabaseConfig : BaseConfig<DatabaseConfigData>
{

  public DatabaseConfig(string dir) : base(dir, new DatabaseConfigData(), "database.json")
  {
  }

}