using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using KPSAllocator.Modules.Menu;

namespace KPSAllocator.Modules.Config;

public class RoundPercentage
{
  public bool IsFixed { set; get; } = false;
  // If IsFixed is true then this will be used as the fixed number of rounds (If all rounds are completed, then it will always be full buy)
  public float FullBuy { set; get; } = 83;
  public float SmallBuy { set; get; } = 7;
  public float Pistol { set; get; } = 10;
}

public class RoundUtilValueProperty
{
  public required int Min { set; get; }
  public required int Max { set; get; }
  [JsonConverter(typeof(JsonStringEnumConverter))]
  public required RoundType RoundType { set; get; }
}

public class UtilValueProperty
{
  [JsonConverter(typeof(JsonStringEnumConverter))]
  public required CsItem Item { set; get; }
  public required int Value { set; get; }
  public required int Amount { set; get; }
}

public class GameConfigData : IBaseConfigData
{
  // Settings this to true will make the plugin randomly set the round type (Still based on the percentage below).
  // Default value is false (So it will always start with a pistol round then a half buy then a full buy)
  public string Version { set; get; } = "1.0.2";
  public string ChatPrefix { set; get; } = "Retakes";
  public bool RandomRound { set; get; } = false;
  public List<MenuType> Menus { set; get; } = new List<MenuType>() {
    MenuType.PRIMAY,
    MenuType.SECONDARY,
    MenuType.SMG,
    MenuType.SNIPER
  };
  // Full buy, half buy, eco
  // You can Either use Percentage or fixed number
  // Default value is 83% full buy, 7% half buy, 10% eco (So In A 30 Round game, 25 full buy, 2 half buy, 3 eco)
  // Fixed Number example: *,2,3 (* means the rest of the rounds)
  // Using fixed number is gives us (25 full buy, 2 half buy, 3 eco) in a 30 round game (But if it's longer then it would still only give us 2 half buy and 3 eco rest will always be full buy)
  public RoundPercentage RoundPercentage { set; get; } = new RoundPercentage();
  public List<RoundUtilValueProperty> RoundUtilValue { set; get; } = new List<RoundUtilValueProperty>() {
    new RoundUtilValueProperty() { Min = 0, Max = 100, RoundType = RoundType.Pistol },
    new RoundUtilValueProperty() { Min = 300, Max = 400, RoundType = RoundType.SmallBuy },
    new RoundUtilValueProperty() { Min = 400, Max = 700, RoundType = RoundType.FullBuy }

  };
  public List<UtilValueProperty> UtilValues { set; get; } = new List<UtilValueProperty>() {
    new UtilValueProperty() { Item = CsItem.Flashbang, Value = 100, Amount = 1 },
    new UtilValueProperty() { Item = CsItem.SmokeGrenade, Value = 300, Amount = 1 },
    new UtilValueProperty() { Item = CsItem.Molotov, Value = 500, Amount = 1 },
    new UtilValueProperty() { Item = CsItem.HE, Value = 200, Amount = 1 }
  };
}

public class GameConfig : BaseConfig<GameConfigData>
{

  public GameConfig(string dir) : base(dir, new GameConfigData(), "game.json")
  {
  }

  // public override dynamic ManipulateBeforeSave()
  // {
  //   return (object)ConfigData.UtilValues.Select(x => (object)x.Item = x.Item.ToString());
  // }

}