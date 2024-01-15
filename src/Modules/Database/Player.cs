using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using KPSAllocator.Modules.Player;
using Microsoft.EntityFrameworkCore;

namespace KPSAllocator.Modules.Database
{
  [Table("kps_allocator_players")]
  public class PlayerDatabase
  {
    [Key, Required, Column(TypeName = "bigint"), DatabaseGenerated(DatabaseGeneratedOption.None)]
    public ulong steamID { set; get; }
    [DefaultValue(CsItem.AK47)]
    public string primaryWeaponTerrorist { set; get; } = CsItem.AK47.ToString();
    [DefaultValue(CsItem.M4A1S)]
    public string primaryWeaponCounterTerrorist { set; get; } = CsItem.M4A1S.ToString();
    [DefaultValue(CsItem.Glock)]
    public string secondaryWeaponTerrorist { set; get; } = CsItem.Glock.ToString();
    [DefaultValue(CsItem.USP)]
    public string secondaryWeaponCounterTerrorist { set; get; } = CsItem.USP.ToString();
    [DefaultValue(CsItem.Mac10)]
    public string smgWeaponTerrorist { set; get; } = CsItem.Mac10.ToString();
    [DefaultValue(CsItem.MP9)]
    public string smgWeaponCounterTerrorist { set; get; } = CsItem.MP9.ToString();
    [DefaultValue(false)]
    public bool enableSniperTerrorist { set; get; } = false;
    [DefaultValue(false)]
    public bool enableSniperCounterTerrorist { set; get; } = false;

    public AllocatorPlayer? ToAllocatorPlayer()
    {
      var controller = Utilities.GetPlayerFromSteamId(steamID);
      if (controller is null)
      {
        return null;
      }
      return new AllocatorPlayer()
      {
        Controller = controller,
        PrimaryWeaponT = (CsItem)System.Enum.Parse(typeof(CsItem), primaryWeaponTerrorist),
        PrimaryWeaponCT = (CsItem)System.Enum.Parse(typeof(CsItem), primaryWeaponCounterTerrorist),
        SecondaryWeaponT = (CsItem)System.Enum.Parse(typeof(CsItem), secondaryWeaponTerrorist),
        SecondaryWeaponCT = (CsItem)System.Enum.Parse(typeof(CsItem), secondaryWeaponCounterTerrorist),
        SMGT = (CsItem)System.Enum.Parse(typeof(CsItem), smgWeaponTerrorist),
        SMGCT = (CsItem)System.Enum.Parse(typeof(CsItem), smgWeaponCounterTerrorist),
        SniperEnabledT = enableSniperTerrorist,
        SniperEnabledCT = enableSniperCounterTerrorist
      };
    }

    public void UpdateFromAllocatorPlayer(AllocatorPlayer player)
    {
      primaryWeaponTerrorist = player.PrimaryWeaponT.ToString();
      primaryWeaponCounterTerrorist = player.PrimaryWeaponCT.ToString();
      secondaryWeaponTerrorist = player.SecondaryWeaponT.ToString();
      secondaryWeaponCounterTerrorist = player.SecondaryWeaponCT.ToString();
      smgWeaponTerrorist = player.SMGT.ToString();
      smgWeaponCounterTerrorist = player.SMGCT.ToString();
      enableSniperTerrorist = player.SniperEnabledT;
      enableSniperCounterTerrorist = player.SniperEnabledCT;
    }

  }
}