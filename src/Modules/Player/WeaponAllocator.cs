using CounterStrikeSharp.API.Modules.Entities.Constants;

namespace KPSAllocator.Modules.Player;

public partial class AllocatorPlayer
{

  public void WeaponAllocation(bool giveSniper)
  {
    RoundType roundType = KPSAllocator.AllocatorManager.CurrentRoundType;

    Controller.GiveNamedItem(SecondaryWeapon());
    Controller.GiveNamedItem(CsItem.Knife);
    if (roundType == RoundType.FullBuy || roundType == RoundType.SmallBuy)
    {
      Controller.GiveNamedItem(CsItem.KevlarHelmet);
      Utils.GivePlayerDefuseKit(Controller);
    }
    else
    {
      Controller.GiveNamedItem(CsItem.Kevlar);
    }
    switch (roundType)
    {
      case RoundType.Pistol:
        // Not used yet.
        break;
      case RoundType.SmallBuy:
        if (giveSniper)
          Controller.GiveNamedItem(CsItem.Scout);
        else
          Controller.GiveNamedItem(SMGWeapon());

        break;
      default:
        if (giveSniper)
          Controller.GiveNamedItem(CsItem.AWP);
        else
          Controller.GiveNamedItem(PrimaryWeapon());
        break;
    }
    UtilAllocator();
  }

}