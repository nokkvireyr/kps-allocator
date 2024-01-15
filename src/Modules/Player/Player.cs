using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Utils;
using KPSAllocator.Modules.Database;

namespace KPSAllocator.Modules.Player;

public partial class AllocatorPlayer
{
  public required CCSPlayerController Controller { get; set; }

  /*
  * User Weapons
  */
  // Terrorist Weapons
  public CsItem PrimaryWeaponT { get; set; } = TWeapon.Primary.AK47;
  public CsItem SecondaryWeaponT { get; set; } = TWeapon.Secondary.Glock;
  public CsItem SMGT { get; set; } = TWeapon.SMG.Mac10;
  public bool SniperEnabledT = false;

  // Counter Terrorist Weapons 
  public CsItem PrimaryWeaponCT { get; set; } = CTWeapon.Primary.M4A1S;
  public CsItem SecondaryWeaponCT { get; set; } = CTWeapon.Secondary.USP;
  public CsItem SMGCT { get; set; } = CTWeapon.SMG.MP9;
  public bool SniperEnabledCT = false;

  private Random r = new Random();

  /**
  ** METHODS
  */
  public ulong GetSteamID()
  {
    return Controller.SteamID;
  }
  public CsTeam GetTeam()
  {
    return (CsTeam)Controller.TeamNum;
  }
  public bool IsValid()
  {
    return Utils.IsValidPlayer(Controller) && (GetTeam() == CsTeam.Terrorist || GetTeam() == CsTeam.CounterTerrorist);
  }

  public void Save()
  {
    Query.UpdateOrAddPlayer(this);
  }

  public PlayerDatabase ToPlayerDatabase()
  {
    return new PlayerDatabase()
    {
      steamID = Controller.SteamID,
      primaryWeaponTerrorist = PrimaryWeaponT.ToString(),
      primaryWeaponCounterTerrorist = PrimaryWeaponCT.ToString(),
      secondaryWeaponTerrorist = SecondaryWeaponT.ToString(),
      secondaryWeaponCounterTerrorist = SecondaryWeaponCT.ToString(),
      smgWeaponTerrorist = SMGT.ToString(),
      smgWeaponCounterTerrorist = SMGCT.ToString(),
      enableSniperTerrorist = SniperEnabledT,
      enableSniperCounterTerrorist = SniperEnabledCT
    };
  }

  public void PrintToChat(string message)
  {
    Controller.PrintToChat($"{Constant.CHAT_PREFIX} {message}");
  }

  public static AllocatorPlayer? GetFromSteamID(ulong steamid)
  {
    return KPSAllocator.connectedPlayers.Find(x => x.GetSteamID() == steamid);
  }

  public static AllocatorPlayer? GetFromController(CCSPlayerController controller)
  {
    return KPSAllocator.connectedPlayers.Find(x => x.Controller == controller);
  }
}