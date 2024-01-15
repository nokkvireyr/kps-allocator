using System.Text.Json;
using System.Text.Json.Serialization;

namespace KPSAllocator.Modules.Config;

public class BaseConfigData
{
  public string? Version { set; get; } = "1.0.0";
}

public class BaseConfig<TConfigData> where TConfigData : BaseConfigData
{
  public TConfigData? ConfigData { set; get; }
  public TConfigData? DefaultConfigData;
  private readonly string _configPath;
  public BaseConfig(string dir, TConfigData defaultConfigData, string configName = "config.json")
  {
    if (!Directory.Exists(Path.Combine(dir, "configs")))
    {
      Directory.CreateDirectory(Path.Combine(dir, "configs"));
    }
    _configPath = Path.Combine(Path.Combine(dir, "configs"), configName);
    DefaultConfigData = defaultConfigData;
    ConfigData = null;
  }

  public void Load()
  {
    try
    {
      if (!File.Exists(_configPath))
      {
        throw new FileNotFoundException();
      }
      var json = File.ReadAllText(_configPath);
      var options = new JsonSerializerOptions
      {
        AllowTrailingCommas = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
      };

      ConfigData = JsonSerializer.Deserialize<TConfigData>(json, options);

      if (ConfigData == null)
      {
        throw new Exception("Config data is null");
      }
    }
    catch (FileNotFoundException)
    {
      Utils.Log("Config file not found, creating new one...");
      ConfigData = DefaultConfigData;
      Save();
    }
    catch (Exception ex)
    {
      Utils.Log($"Error while loading config: {ex.Message}");
    }
  }

  public virtual dynamic ManipulateBeforeSave()
  {
    return ConfigData!;
  }

  public void Save()
  {
    var manipulated = ManipulateBeforeSave();
    var options = new JsonSerializerOptions { WriteIndented = true };
    options.Converters.Add(new JsonStringEnumConverter());
    var stringyFied = JsonSerializer.Serialize(manipulated, options);

    try
    {
      File.WriteAllText(_configPath, stringyFied);
    }
    catch (Exception ex)
    {
      Utils.Log($"Error while saving config: {ex.Message}");
    }
  }
}