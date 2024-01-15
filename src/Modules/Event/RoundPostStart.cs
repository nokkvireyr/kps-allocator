using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Utils;

namespace KPSAllocator;

public partial class KPSAllocator
{
  public Random r = new Random();

  [GameEventHandler]
  public HookResult RoundPostStart(EventRoundPoststart _, GameEventInfo __)
  {
    AddTimer(0.05f, () =>
    {
      var connectedPlayersT = connectedPlayers.Where((player) => player.GetTeam() == CsTeam.Terrorist && player.SniperEnabledT);
      var connectedPlayersCT = connectedPlayers.Where((player) => player.GetTeam() == CsTeam.CounterTerrorist && player.SniperEnabledCT);
      var sniperTPlayer = Utils.RandomPlayer(connectedPlayersT);
      var sniperCTPlayer = Utils.RandomPlayer(connectedPlayersCT);
      connectedPlayers.ForEach(x => x.WeaponAllocation(x == sniperCTPlayer || x == sniperTPlayer));
    });
    return HookResult.Continue;
  }

}