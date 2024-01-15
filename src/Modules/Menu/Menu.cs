using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;
using KPSAllocator.Modules.Player;
using Microsoft.Extensions.Localization;

namespace KPSAllocator.Modules.Menu;

public class AllocatorMenu
{
  public readonly IStringLocalizer Localizer;

  public static List<AllocatorPlayer> InMenu = new List<AllocatorPlayer>();

  public AllocatorMenu(IStringLocalizer localizer)
  {
    Localizer = localizer;
  }

  public ChatMenu BaseMenu(string title, CsTeam team)
  {
    var menu = new ChatMenu($"{Constant.CHAT_PREFIX} {title} ({Localizer[$"team.{team}"]})");
    return menu;
  }

  public void CloseMenu(CCSPlayerController player, ChatMenuOption? _)
  {
    var po = InMenu.Find(x => x.Controller == player);
    if (po is not null)
      InMenu.Remove(po);
    AllocatorPlayer.GetFromController(player)?.PrintToChat(Localizer["menu.nextRound"]);
  }

  public ChatMenu? PrimaryWeapon(AllocatorPlayer player, CsTeam team = CsTeam.Terrorist)
  {

    if (InMenu.Contains(player))
    {
      player.PrintToChat(Localizer["menu.alreadyOpen"]);
      return null;
    }
    InMenu.Add(player);
    var menu = BaseMenu(Localizer["menu.primary"], team);
    var menuItems = team == CsTeam.Terrorist ? TWeapon.Primary.ToList() : CTWeapon.Primary.ToList();

    // Next Menu
    var nextMenu = SecondaryWeapon(player, team);

    menuItems.ForEach(x => menu.AddMenuOption(Localizer[$"weapon.{x}"], (CCSPlayerController _, ChatMenuOption option) => MenuHandler(player, x, team, RoundType.FullBuy, nextMenu)));
    menu.AddMenuOption(Localizer["menu.exit"], CloseMenu);

    return menu;
  }

  public ChatMenu SecondaryWeapon(AllocatorPlayer player, CsTeam team = CsTeam.Terrorist)
  {
    var menu = BaseMenu(Localizer["menu.secondary"], team);
    var menuItems = team == CsTeam.Terrorist ? TWeapon.Secondary.ToList() : CTWeapon.Secondary.ToList();

    // Next Menu
    var nextMenu = SMGWeapon(player, team);
    menuItems.ForEach(x => menu.AddMenuOption(Localizer[$"weapon.{x}"], (CCSPlayerController _, ChatMenuOption option) => MenuHandler(player, x, team, RoundType.Pistol, nextMenu)));
    menu.AddMenuOption(Localizer["menu.exit"], CloseMenu);
    return menu;
  }

  public ChatMenu SMGWeapon(AllocatorPlayer player, CsTeam team = CsTeam.Terrorist)
  {
    var menu = BaseMenu(Localizer["menu.smg"], team);
    var menuItems = team == CsTeam.Terrorist ? TWeapon.SMG.ToList() : CTWeapon.SMG.ToList();
    // Next Menu
    var nextMenu = AllowSniper(player, team);
    menuItems.ForEach(x => menu.AddMenuOption(Localizer[$"weapon.{x}"], (CCSPlayerController _, ChatMenuOption option) => MenuHandler(player, x, team, RoundType.SmallBuy, nextMenu)));
    menu.AddMenuOption(Localizer["menu.exit"], CloseMenu);
    return menu;
  }

  public ChatMenu AllowSniper(AllocatorPlayer player, CsTeam team = CsTeam.Terrorist)
  {
    var menu = BaseMenu(Localizer["menu.allowSniper"], team);
    menu.AddMenuOption(Localizer[$"menu.yes"], (CCSPlayerController _, ChatMenuOption option) => MenuHandler(player, null, team, RoundType.FullBuy, null, true, true));
    menu.AddMenuOption(Localizer[$"menu.no"], (CCSPlayerController _, ChatMenuOption option) => MenuHandler(player, null, team, RoundType.FullBuy, null, true));
    return menu;
  }
  public void MenuHandler(AllocatorPlayer player, CsItem? weapon, CsTeam team, RoundType type, ChatMenu? nextMenu, bool isSettingSniper = false, bool allowSniper = false)
  {
    if (isSettingSniper)
    {
      if (team == CsTeam.Terrorist)
        player.SniperEnabledT = allowSniper;
      else if (team == CsTeam.CounterTerrorist)
        player.SniperEnabledCT = allowSniper;
    }
    if (weapon != null)
    {
      var item = (CsItem)weapon;
      switch (team)
      {
        case CsTeam.CounterTerrorist:
          HandleCTWeapon(player, item, type);
          break;
        case CsTeam.Terrorist:
          HandleTWeapon(player, item, type);
          break;
        default:
          Utils.Log("Not A Valid Team");
          break;
      }
    }
    if (nextMenu != null)
      ChatMenus.OpenMenu(player.Controller, nextMenu);
    else
      CloseMenu(player.Controller, null);
    player.Save();
  }

  public void HandleTWeapon(AllocatorPlayer player, CsItem wepaon, RoundType type)
  {
    switch (type)
    {
      case RoundType.FullBuy:
        player.PrimaryWeaponT = wepaon;
        break;
      case RoundType.Pistol:
        player.SecondaryWeaponT = wepaon;
        break;
      case RoundType.SmallBuy:
        player.SMGT = wepaon;
        break;
    }
  }

  public void HandleCTWeapon(AllocatorPlayer player, CsItem wepaon, RoundType type)
  {
    switch (type)
    {
      case RoundType.FullBuy:
        player.PrimaryWeaponCT = wepaon;
        break;
      case RoundType.Pistol:
        player.SecondaryWeaponCT = wepaon;
        break;
      case RoundType.SmallBuy:
        player.SMGCT = wepaon;
        break;
    }
  }

}