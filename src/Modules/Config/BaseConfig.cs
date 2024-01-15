using System.Text.Json;
using System.Text.Json.Serialization;

namespace KPSAllocator.Modules.Config;


public interface IBaseConfigData
{
  string Version { get; set; }
}

public class BaseConfig<TConfigData> where TConfigData : IBaseConfigData
{
  public TConfigData? ConfigData { set; get; }
  public TConfigData DefaultConfigData { set; get; }
  private readonly string _configPath;
  public BaseConfig(string dir, TConfigData defaultConfigData, string configName = "config.json")
  {
    DefaultConfigData = defaultConfigData;
    if (!Directory.Exists(Path.Combine(dir, "configs")))
    {
      Directory.CreateDirectory(Path.Combine(dir, "configs"));
    }
    _configPath = Path.Combine(Path.Combine(dir, "configs"), configName);
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

      if (ConfigData.Version != DefaultConfigData!.Version)
      {
        UpdateConfig();
        throw new Exception("Outdated Version. Trying to update");
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

  public void UpdateConfig()
  {
    if (ConfigData is null || DefaultConfigData is null)
      return;
    ConfigData.Version = DefaultConfigData.Version;
    Save();
  }
}