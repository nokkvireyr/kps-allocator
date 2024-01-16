using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;
using KPSAllocator.Modules.Player;
using Microsoft.Extensions.Localization;

namespace KPSAllocator.Modules.Menu;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MenuType
{
  PRIMAY = 0,
  SECONDARY = 1,
  SMG = 2,
  SNIPER = 3
}

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

  public ChatMenu? PrimaryWeapon(AllocatorPlayer player, CsTeam team = CsTeam.Terrorist, int menuIndex = 0)
  {

    var menu = BaseMenu(Localizer["menu.primary"], team);
    var menuItems = team == CsTeam.Terrorist ? TWeapon.Primary.ToList() : CTWeapon.Primary.ToList();

    // Next Menu

    menuItems.ForEach(x => menu.AddMenuOption(Localizer[$"weapon.{x}"], (CCSPlayerController _, ChatMenuOption option) => MenuHandler(player, x, team, RoundType.FullBuy, menuIndex)));
    menu.AddMenuOption(Localizer["menu.exit"], CloseMenu);

    return menu;
  }

  public ChatMenu SecondaryWeapon(AllocatorPlayer player, CsTeam team = CsTeam.Terrorist, int menuIndex = 0)
  {
    var menu = BaseMenu(Localizer["menu.secondary"], team);
    var menuItems = team == CsTeam.Terrorist ? TWeapon.Secondary.ToList() : CTWeapon.Secondary.ToList();

    // Next Menu
    var nextMenu = SMGWeapon(player, team);
    menuItems.ForEach(x => menu.AddMenuOption(Localizer[$"weapon.{x}"], (CCSPlayerController _, ChatMenuOption option) => MenuHandler(player, x, team, RoundType.Pistol, menuIndex)));
    menu.AddMenuOption(Localizer["menu.exit"], CloseMenu);
    return menu;
  }

  public ChatMenu SMGWeapon(AllocatorPlayer player, CsTeam team = CsTeam.Terrorist, int menuIndex = 0)
  {
    var menu = BaseMenu(Localizer["menu.smg"], team);
    var menuItems = team == CsTeam.Terrorist ? TWeapon.SMG.ToList() : CTWeapon.SMG.ToList();
    // Next Menu
    var nextMenu = AllowSniper(player, team);
    menuItems.ForEach(x => menu.AddMenuOption(Localizer[$"weapon.{x}"], (CCSPlayerController _, ChatMenuOption option) => MenuHandler(player, x, team, RoundType.SmallBuy, menuIndex)));
    menu.AddMenuOption(Localizer["menu.exit"], CloseMenu);
    return menu;
  }

  public ChatMenu AllowSniper(AllocatorPlayer player, CsTeam team = CsTeam.Terrorist, int menuIndex = 0)
  {
    var menu = BaseMenu(Localizer["menu.allowSniper"], team);
    menu.AddMenuOption(Localizer[$"menu.yes"], (CCSPlayerController _, ChatMenuOption option) => MenuHandler(player, null, team, RoundType.FullBuy, menuIndex, true, true));
    menu.AddMenuOption(Localizer[$"menu.no"], (CCSPlayerController _, ChatMenuOption option) => MenuHandler(player, null, team, RoundType.FullBuy, menuIndex, true));
    return menu;
  }
  public void MenuHandler(AllocatorPlayer player, CsItem? weapon, CsTeam team, RoundType type, int nextMenuIndex, bool isSettingSniper = false, bool allowSniper = false)
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
    var nextMenu = GetNextMenu(nextMenuIndex, player, team);
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

  public ChatMenu? GetNextMenu(int menuIndex, AllocatorPlayer player, CsTeam team)
  {
    if (KPSAllocator.GameConfig?.ConfigData is null)
    {
      player.PrintToChat(Localizer["error.generic"]);
      return null;
    }
    if (KPSAllocator.GameConfig.ConfigData.Menus.Count <= 0)
    {
      player.PrintToChat(Localizer["menu.notAvailable"]);
      return null;
    }
    if (menuIndex >= KPSAllocator.GameConfig.ConfigData.Menus.Count)
      return null;
    var menuType = KPSAllocator.GameConfig.ConfigData.Menus.ElementAt(menuIndex);
    return GetMenuByType(menuType, player, team, menuIndex + 1);
  }

  public ChatMenu? GetMenuByType(MenuType menuType, AllocatorPlayer player, CsTeam team, int menuIndex = 0)
  {
    return menuType switch
    {
      MenuType.SECONDARY => SecondaryWeapon(player, team, menuIndex),
      MenuType.SMG => SMGWeapon(player, team, menuIndex),
      MenuType.SNIPER => AllowSniper(player, team, menuIndex),
      _ => PrimaryWeapon(player, team, menuIndex)
    };
  }

}