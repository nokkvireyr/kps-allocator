using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;

namespace KPSAllocator;

public partial class KPSAllocator : BasePlugin
{
  [GameEventHandler]
  public HookResult RoundEndEvent(EventRoundEnd _, GameEventInfo __)
  {
    AllocatorManager.CalculateRoundType();
    return HookResult.Continue;
  }
}