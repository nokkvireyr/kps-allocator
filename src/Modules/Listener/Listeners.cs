using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Modules.Entities;

namespace KPSAllocator;

public partial class KPSAllocator : BasePlugin
{

  public void SetupListeners()
  {
    RegisterListener<Listeners.OnClientAuthorized>((int playerSlot, [CastFrom(typeof(ulong))] SteamID steamID) =>
    {
      Utils.AddPlayerToList(steamID.SteamId64, Localizer);
    });

    RegisterListener<Listeners.OnClientDisconnect>((int playerSlot) =>
    {
      var player = Utilities.GetPlayerFromSlot(playerSlot);
      var connectedPlayer = KPSAllocator.connectedPlayers.FirstOrDefault(x => x.GetSteamID() == player.SteamID);
      if (connectedPlayer != null)
        KPSAllocator.connectedPlayers.Remove(connectedPlayer);
    });

    RegisterListener<Listeners.OnMapEnd>(() =>
    {
      AllocatorManager.Rest();
    });
    RegisterListener<Listeners.OnMapStart>((_) =>
    {
      AllocatorManager.Rest();
    });
  }

}