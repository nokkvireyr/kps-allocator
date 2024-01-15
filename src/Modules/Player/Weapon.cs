namespace KPSAllocator.Modules.Player;

using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using System.Reflection;

public static class TWeapon
{
  // Primary Weapons
  public class Primary
  {
    public static CsItem AK47 = CsItem.AK47;
    public static CsItem Galil = CsItem.Galil;
    public static CsItem SG553 = CsItem.SG553;
    // public static CsItem G3SG1 = CsItem.G3SG1;

    public static List<CsItem> ToList()
    {
      Primary Bluff = new Primary();
      return Bluff.GetType().GetRuntimeFields().Select(x => (CsItem)x.GetValue(Bluff)!).Concat(SharedWeapon.Primary.ToList()).ToList();
    }
  }

  public class SMG
  {
    public static CsItem SawedOff = CsItem.SawedOff;
    public static CsItem Mac10 = CsItem.Mac10;
    public static List<CsItem> ToList()
    {
      var Bluff = new SMG();
      return Bluff.GetType().GetRuntimeFields().Select(x => (CsItem)x.GetValue(Bluff)!).Concat(SharedWeapon.SMG.ToList()).ToList();
    }
  }

  public class Secondary
  {
    public static CsItem Glock = CsItem.Glock;
    public static CsItem Tec9 = CsItem.Tec9;
    public static List<CsItem> ToList()
    {
      var Bluff = new Secondary();
      return Bluff.GetType().GetRuntimeFields().Select(x => (CsItem)x.GetValue(Bluff)!).Concat(SharedWeapon.Secondary.ToList()).ToList();
    }
  }
}
public static class CTWeapon
{
  // Primary Weapons
  public class Primary
  {
    public static CsItem M4A1S = CsItem.M4A1S;
    public static CsItem M4A4 = CsItem.M4A4;
    public static CsItem Famas = CsItem.Famas;
    public static CsItem AUG = CsItem.AUG;
    // public static CsItem SCAR20 = CsItem.SCAR20;
    public static List<CsItem> ToList()
    {
      Primary Bluff = new Primary();
      return Bluff.GetType().GetRuntimeFields().Select(x => (CsItem)x.GetValue(Bluff)!).Concat(SharedWeapon.Primary.ToList()).ToList();
    }
  }

  public class SMG
  {
    public static CsItem Mag7 = CsItem.MAG7;
    public static CsItem MP9 = CsItem.MP9;
    public static List<CsItem> ToList()
    {
      var Bluff = new SMG();
      return Bluff.GetType().GetRuntimeFields().Select(x => (CsItem)x.GetValue(Bluff)!).Concat(SharedWeapon.SMG.ToList()).ToList();
    }
  }

  public class Secondary
  {
    public static CsItem USP = CsItem.USP;
    public static CsItem P2000 = CsItem.P2000;
    public static CsItem FiveSeven = CsItem.FiveSeven;
    public static List<CsItem> ToList()
    {
      var Bluff = new Secondary();
      return Bluff.GetType().GetRuntimeFields().Select(x => (CsItem)x.GetValue(Bluff)!).Concat(SharedWeapon.Primary.ToList()).ToList();
    }
  }
}

public static class SharedWeapon
{
  public class Primary
  {
    public static CsItem M249 = CsItem.M249;
    public static CsItem Negev = CsItem.Negev;
    public static List<CsItem> ToList()
    {
      Primary Bluff = new Primary();
      return Bluff.GetType().GetRuntimeFields().Select(x => (CsItem)x.GetValue(Bluff)!).ToList();
    }
  }

  public class SMG
  {
    public static CsItem P90 = CsItem.P90;
    public static CsItem Nova = CsItem.Nova;
    public static CsItem XM1014 = CsItem.XM1014;
    public static CsItem MP5 = CsItem.MP5;
    public static CsItem MP7 = CsItem.MP7;
    public static CsItem Bizon = CsItem.Bizon;
    public static CsItem UMP = CsItem.UMP;
    public static List<CsItem> ToList()
    {
      var Bluff = new SMG();
      return Bluff.GetType().GetRuntimeFields().Select(x => (CsItem)x.GetValue(Bluff)!).ToList();
    }

  }

  public class Secondary
  {
    public static CsItem Deagle = CsItem.Deagle;
    public static CsItem DualBerettas = CsItem.DualBerettas;
    public static CsItem CZ75 = CsItem.CZ75;
    public static CsItem P250 = CsItem.P250;
    public static List<CsItem> ToList()
    {
      var Bluff = new Secondary();
      return Bluff.GetType().GetRuntimeFields().Select(x => (CsItem)x.GetValue(Bluff)!).ToList();
    }
  }
}

public partial class AllocatorPlayer
{
  public CsItem PrimaryWeapon()
  {
    return GetTeam() == CsTeam.Terrorist ? PrimaryWeaponT : PrimaryWeaponCT;
  }
  public CsItem SecondaryWeapon()
  {
    return GetTeam() == CsTeam.Terrorist ? SecondaryWeaponT : SecondaryWeaponCT;
  }
  public CsItem SMGWeapon()
  {
    return GetTeam() == CsTeam.Terrorist ? SMGT : SMGCT;
  }
}