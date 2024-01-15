using CounterStrikeSharp.API;
using KPSAllocator.Modules.Database;
using KPSAllocator.Modules.Player;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Cvars;
using Microsoft.Extensions.Localization;
namespace KPSAllocator;

public static class Utils
{
  public static void AddPlayerToList(ulong steamID, IStringLocalizer Localizer)
  {
    if (KPSAllocator.connectedPlayers.Any(x => x.GetSteamID() == steamID))
      return;
    var player = Query.GetPlayer(steamID);
    var allocatorPlayer = player?.ToAllocatorPlayer();
    if (allocatorPlayer == null)
    {
      var controller = Utilities.GetPlayerFromSteamId(steamID);
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
      KPSAllocator.connectedPlayers.Add(allocatorPlayer);
      allocatorPlayer.PrintToChat(Localizer["welcomeMessage"]);
    }

  }

  internal static readonly Random Random = new Random();

  public static bool IsValidPlayer(CCSPlayerController player)
  {
    return player != null && player.IsValid;
  }
  public static void GivePlayerDefuseKit(CCSPlayerController player)
  {
    if (player.PlayerPawn.Value?.ItemServices?.Handle is null || !IsValidPlayer(player))
    {
      return;
    }
    new CCSPlayer_ItemServices(player.PlayerPawn.Value.ItemServices.Handle).HasDefuser = true;
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
}