using System.Collections.Specialized;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Utils;
using KPSAllocator.Modules.Config;

namespace KPSAllocator.Modules.Player;


public partial class AllocatorPlayer
{
  public void UtilAllocator()
  {
    var config = KPSAllocator.GameConfig?.ConfigData;
    if (config is null)
      return;
    int value = 0;
    var roundTypeValue = config.RoundUtilValue.Find(x => x.RoundType == KPSAllocator.AllocatorManager.CurrentRoundType);

    if (roundTypeValue is not null)
    {
      value = r.Next(roundTypeValue.Min, roundTypeValue.Max);
    }
    else
    {
      value = r.Next(0, 100);
    }

    if (value > 100)
    {
      var nades = new List<CsItem>();
      int fail = 0;

      var Grenades = new List<UtilValueProperty>(config.UtilValues);
      while (value >= 100 && fail < 3)
      {
        int randomIndex = r.Next(0, Grenades.Count);
        var grenade = Grenades[randomIndex];
        if (grenade.Item == CsItem.Molotov && GetTeam() == CsTeam.CounterTerrorist)
        {
          grenade.Item = CsItem.Incendiary;
        }
        if (value >= grenade.Value && nades.Count(x => x == grenade.Item) < grenade.Amount)
        {
          value -= grenade.Value;
          nades.Add(grenade.Item);
        }
        else
        {
          fail++;
        }
      }
      foreach (var nade in nades)
      {
        Controller.GiveNamedItem(nade);
      }
    }
  }
}