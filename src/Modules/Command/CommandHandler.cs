using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;
using KPSAllocator.Modules.Player;

namespace KPSAllocator;

public partial class KPSAllocator : BasePlugin
{

  [ConsoleCommand("css_guns", "Opens up the gun menu, Can also open up menu for each team by using !guns <t/ct>")]
  [CommandHelper(minArgs: 0, usage: "[t/ct]", whoCanExecute: CommandUsage.CLIENT_ONLY)]
  public void OnGunsCommand(CCSPlayerController? player, CommandInfo info)
  {
    GunCommandHandler(player, info);
  }

  [ConsoleCommand("css_gun", "Opens up the gun menu, Can also open up menu for each team by using !gun <t/ct>")]
  [CommandHelper(minArgs: 0, usage: "[t/ct]", whoCanExecute: CommandUsage.CLIENT_ONLY)]
  public void OnGunCommand(CCSPlayerController? player, CommandInfo info)
  {
    GunCommandHandler(player, info);
  }

  public void GunCommandHandler(CCSPlayerController? player, CommandInfo info)
  {
    var po = connectedPlayers.Find(x => x.Controller == player);
    if (player is null || po is null || Menus is null)
      return;
    var team = info.GetArg(1) switch
    {
      "t" => CsTeam.Terrorist,
      "ct" => CsTeam.CounterTerrorist,
      _ => po.GetTeam(),
    };
    GunsMenu(po, team);
  }

  public void GunsMenu(AllocatorPlayer player, CsTeam team)
  {
    var primaryMenu = Menus?.PrimaryWeapon(player, team);
    if (primaryMenu != null)
      ChatMenus.OpenMenu(player.Controller, primaryMenu);
  }

}