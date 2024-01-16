

using System.Net.NetworkInformation;
using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Modules.Utils;

namespace KPSAllocator;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RoundType
{
  Pistol = 3,
  SmallBuy = 2,
  FullBuy = 1,
}

public static class Constant
{
  public static string DATABASE_NAME = "kps_allocator";
  public static string CHAT_PREFIX = $"[{ChatColors.Green}Retakes{ChatColors.White}] ";
  public static string LOG_PREFIX = $"[KPS Allocator] ";

  public static void SetChatPrefix(string? prefix)
  {
    if (prefix is not null)
      CHAT_PREFIX = $"[{ChatColors.Green}{prefix}{ChatColors.White}] ";
  }
}
