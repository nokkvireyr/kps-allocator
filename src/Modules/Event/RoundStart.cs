using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Utils;

namespace KPSAllocator;

public partial class KPSAllocator : BasePlugin
{
  [GameEventHandler]
  public HookResult RoundStartEvent(EventRoundStart _, GameEventInfo __)
  {
    if (!Utils.IsWarmup())
      Utils.PrintToAll(Localizer[$"roundType.{AllocatorManager.CurrentRoundType}"]);
    return HookResult.Continue;
  }
}