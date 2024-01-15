using CounterStrikeSharp.API.Core;
using KPSAllocator.Modules.Config;
using KPSAllocator.Modules.Database;
using KPSAllocator.Modules.Manager;
using KPSAllocator.Modules.Menu;
using KPSAllocator.Modules.Player;

namespace KPSAllocator;

public partial class KPSAllocator : BasePlugin
{
  // This is the entry point for the plugin
  public override string ModuleName => "KPS Allocator";
  public override string ModuleVersion => "0.1.1";
  public override string ModuleAuthor => "Nokkvi Reyr";
  // Variables
  public static DatabaseConfig? DBConfig { get; set; } = null;
  public static GameConfig? GameConfig { get; set; } = null;
  public static List<AllocatorPlayer> connectedPlayers = new List<AllocatorPlayer>();
  public static Manager AllocatorManager = new Manager();
  public static AllocatorMenu? Menus { get; set; }
  public override void Load(bool hotReload)
  {
    DBConfig = new DatabaseConfig(ModuleDirectory);
    DBConfig.Load();
    GameConfig = new GameConfig(ModuleDirectory);
    Menus = new AllocatorMenu(Localizer);
    GameConfig.Load();
    if (!hotReload)
      Query.Migrate();

    // Register listeners
    SetupListeners();
  }

}
