using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using KPSAllocator.Modules.Config;
using KPSAllocator.Modules.Database;
using KPSAllocator.Modules.Manager;
using KPSAllocator.Modules.Menu;
using KPSAllocator.Modules.Player;
using Microsoft.EntityFrameworkCore;

namespace KPSAllocator;

[MinimumApiVersion(147)]
public partial class KPSAllocator : BasePlugin
{
  // This is the entry point for the plugin
  public override string ModuleName => "KPS Allocator";
  public override string ModuleVersion => "0.2.0";
  public override string ModuleAuthor => "Nokkvi Reyr";
  // Variables
  public static DatabaseConfig? DBConfig { get; set; } = null;
  public static GameConfig? GameConfig { get; set; } = null;
  public static List<AllocatorPlayer> connectedPlayers = new List<AllocatorPlayer>();
  public static Manager AllocatorManager = new Manager();
  public static AllocatorMenu? Menus { get; set; }
  public static Database? Database { get; set; }

  public override void Load(bool hotReload)
  {
    DBConfig = new DatabaseConfig(ModuleDirectory);
    DBConfig.Load();
    GameConfig = new GameConfig(ModuleDirectory);
    GameConfig.Load();

    Database = new Database();

    if (hotReload)
    {
      Utilities.GetPlayers().ForEach((player) =>
      {
        if (player.IsValid && !player.IsBot)
        {
          Utils.AddPlayerToList(player.SteamID, Localizer);
        }
      });
    }
    Database.Database.Migrate();
    // Register listeners
    SetupListeners();
    Menus = new AllocatorMenu(Localizer);
  }

  public override void Unload(bool hotReload)
  {
    connectedPlayers.Clear();
    Database?.Close();
    Thread.Sleep(200);
  }

}
