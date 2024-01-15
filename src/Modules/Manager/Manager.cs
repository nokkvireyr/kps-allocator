using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Cvars;

namespace KPSAllocator.Modules.Manager;

public class Manager
{

  public RoundType CurrentRoundType { set; get; } = RoundType.Pistol;

  public int FullBuyPlayed = 0;
  public int SmallBuyPlayed = 0;
  public int PistolPlayed = 0;
  private Random R = new Random();
  // Methods
  public void CalculateRoundType()
  {
    var config = KPSAllocator.GameConfig?.ConfigData;
    if (config == null)
      return;
    if (config.RandomRound)
    {
      if (config.RoundPercentage.IsFixed)
      {
        float fullBuyLeft = config.RoundPercentage.FullBuy - FullBuyPlayed;
        float smallBuyLeft = config.RoundPercentage.SmallBuy - SmallBuyPlayed;
        float pistolLeft = config.RoundPercentage.Pistol - PistolPlayed;
        if (pistolLeft <= 0 && smallBuyLeft <= 0 && fullBuyLeft <= 0)
        {
          CurrentRoundType = RoundType.FullBuy;
          return;
        }

        RoundType? round = null;
        while (round == null)
        {
          var radom = R.Next(0, (int)Math.Round(config.RoundPercentage.FullBuy + config.RoundPercentage.SmallBuy + config.RoundPercentage.Pistol));
          if (radom <= config.RoundPercentage.FullBuy)
          {
            if (fullBuyLeft > 0)
            {
              round = RoundType.FullBuy;
              FullBuyPlayed++;
            }
          }
          else if (radom <= config.RoundPercentage.FullBuy + config.RoundPercentage.SmallBuy)
          {
            if (smallBuyLeft > 0)
            {
              round = RoundType.SmallBuy;
              SmallBuyPlayed++;
            }
          }
          else
          {
            if (pistolLeft > 0)
            {
              round = RoundType.Pistol;
              PistolPlayed++;
            }
          }
        }
        CurrentRoundType = (RoundType)round;
      }
      else
      {
        int random = R.Next(0, 100);
        if (random < config.RoundPercentage.FullBuy)
        {
          CurrentRoundType = RoundType.FullBuy;
        }
        else if (random < config.RoundPercentage.FullBuy + config.RoundPercentage.SmallBuy)
        {
          CurrentRoundType = RoundType.SmallBuy;
        }
        else
        {
          CurrentRoundType = RoundType.Pistol;
        }
      }
    }
    else
    {
      if (config.RoundPercentage.IsFixed)
      {
        var currentRound = Utils.GetGameRules().TotalRoundsPlayed;
        if (currentRound >= config.RoundPercentage.Pistol)
        {
          CurrentRoundType = RoundType.Pistol;
          return;
        }
        else if (currentRound >= config.RoundPercentage.Pistol + config.RoundPercentage.SmallBuy)
        {
          CurrentRoundType = RoundType.SmallBuy;
          return;
        }
        else
        {
          CurrentRoundType = RoundType.FullBuy;
          return;
        }
      }
      else
      {
        var currentRound = Utils.GetGameRules().TotalRoundsPlayed;
        var totalRounds = Utils.GetTotalRounds();
        if (currentRound < totalRounds * (config.RoundPercentage.Pistol / 100))
        {
          CurrentRoundType = RoundType.Pistol;
          return;
        }
        else if (currentRound < totalRounds * ((config.RoundPercentage.Pistol + config.RoundPercentage.SmallBuy) / 100))
        {
          CurrentRoundType = RoundType.SmallBuy;
          return;
        }
        else
        {
          CurrentRoundType = RoundType.FullBuy;
          return;
        }
      }
    }
  }

  public void Rest()
  {
    FullBuyPlayed = 0;
    SmallBuyPlayed = 0;
    PistolPlayed = 0;
  }

}