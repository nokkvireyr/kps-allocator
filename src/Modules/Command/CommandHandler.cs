using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;
using KPSAllocator.Modules.Menu;
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
    var team = info.GetArg(1).ToLower() switch
    {
      "t" => CsTeam.Terrorist,
      "ct" => CsTeam.CounterTerrorist,
      _ => po.GetTeam(),
    };
    GunsMenu(po, team);
  }

  public void GunsMenu(AllocatorPlayer player, CsTeam team)
  {
    if (GameConfig?.ConfigData is null || Menus is null)
    {
      player.PrintToChat($"{ChatColors.Red}Something went wrong.");
      return;
    }
    if (AllocatorMenu.InMenu.Contains(player))
    {
      player.PrintToChat(Localizer["menu.alreadyOpen"]);
      return;
    }
    var initialMenu = Menus.GetNextMenu(0, player, team);
    if (initialMenu is not null)
      ChatMenus.OpenMenu(player.Controller, initialMenu);
  }
}
