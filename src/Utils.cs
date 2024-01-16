using CounterStrikeSharp.API;
using KPSAllocator.Modules.Database;
using KPSAllocator.Modules.Player;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Cvars;
using Microsoft.Extensions.Localization;
using CounterStrikeSharp.API.Modules.Utils;
using KPSAllocator.Modules.Config;
using Microsoft.EntityFrameworkCore;

namespace KPSAllocator;

public static class Utils
{
  public static void AddPlayerToList(ulong? steamID, IStringLocalizer Localizer)
  {
    if (steamID is null || KPSAllocator.connectedPlayers.Any(x => x.GetSteamID() == steamID))
      return;
    var newSteamId = (ulong)steamID;
    var player = Query.GetPlayer(newSteamId);
    var allocatorPlayer = player?.ToAllocatorPlayer();
    if (allocatorPlayer == null)
    {
      var controller = Utilities.GetPlayerFromSteamId(newSteamId);
      if (controller != null)
      {

        allocatorPlayer = new AllocatorPlayer()
        {
          Controller = controller,
        };
        Query.UpdateOrAddPlayer(allocatorPlayer);
      }
    }
    if (allocatorPlayer != null)
    {
      if (allocatorPlayer.Controller.IsValid)
      {
        KPSAllocator.connectedPlayers.Add(allocatorPlayer);
        allocatorPlayer.PrintToChat(Localizer["welcomeMessage"]);
      }
    }

  }

  internal static readonly Random Random = new Random();

  public static bool IsValidPlayer(CCSPlayerController player)
  {
    return player != null && player.IsValid;
  }
  public static void GivePlayerDefuseKit(CCSPlayerController player)
  {
    if (
        player.Team == CsTeam.CounterTerrorist
        && IsValidPlayer(player)
        && player.PlayerPawn.IsValid
        && player.PlayerPawn.Value != null
        && player.PlayerPawn.Value.IsValid
        && player.PlayerPawn.Value.ItemServices != null
    )
    {
      var itemServices = new CCSPlayer_ItemServices(player.PlayerPawn.Value.ItemServices.Handle);
      itemServices.HasDefuser = true;
    }
  }

  public static CCSGameRules GetGameRules()
  {
    var gameRulesEntities = Utilities.FindAllEntitiesByDesignerName<CCSGameRulesProxy>("cs_gamerules");
    var gameRules = gameRulesEntities.First().GameRules;

    if (gameRules == null)
    {
      throw new Exception($"{Constant.LOG_PREFIX} Game rules not found!");
    }

    return gameRules;
  }

  public static int GetTotalRounds()
  {
    int? totalRounds = ConVar.Find("mp_maxrounds")?.GetPrimitiveValue<int>();
    if (totalRounds is null)
      return 30;
    return (int)totalRounds;
  }
  public static AllocatorPlayer? RandomPlayer(IEnumerable<AllocatorPlayer>? players)
  {
    if (players == null)
    {
      return null;
    }
    int index = Random.Next(0, players.Count() + 2);
    if (index >= players.Count())
    {
      return null;
    }
    return players.ElementAt(index);
  }
  public static bool IsWarmup()
  {
    return GetGameRules().WarmupPeriod;
  }

  // Print to all
  public static void PrintToAll(string message)
  {
    Server.PrintToChatAll($"{Constant.CHAT_PREFIX} {message}");
  }

  // Console Writes
  public static void Log(string message)
  {
    Console.WriteLine($"{Constant.LOG_PREFIX} {message}");
  }

  public static void MySQLSetup(DatabaseConfigData config, DbContextOptionsBuilder optionsBuilder)
  {
    var connectionString = @$"Server={config.Host}; Database={config.Database}; Port={config.Port}; User ID={config.User}; Password={config.Pass};";
    optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(ServerVersion.AutoDetect(connectionString)));
  }

  public static void SqlLiteSetup(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlite($"Data Source=kps_allocator.db");
  }
}
