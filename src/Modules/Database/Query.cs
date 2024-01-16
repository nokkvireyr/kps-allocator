using KPSAllocator.Modules.Player;
using Microsoft.EntityFrameworkCore;

namespace KPSAllocator.Modules.Database;

public class Query
{

  public static PlayerDatabase? GetPlayer(ulong steamID)
  {
    return KPSAllocator.Database?.Players.FirstOrDefault(player => player.steamID == steamID);
  }

  public static void UpdateOrAddPlayer(AllocatorPlayer player)
  {
    if (KPSAllocator.Database is null)
      return;
    var dbPlayer = GetPlayer(player.GetSteamID());
    bool isNew = false;
    if (dbPlayer is null)
    {
      dbPlayer = player.ToPlayerDatabase();
      KPSAllocator.Database.Players.Add(dbPlayer);
      isNew = true;
    }
    if (!isNew)
      dbPlayer.UpdateFromAllocatorPlayer(player);
    KPSAllocator.Database.Players.Entry(dbPlayer).State = isNew ? EntityState.Added : EntityState.Modified;
    KPSAllocator.Database.SaveChanges();
    KPSAllocator.Database.Entry(dbPlayer).State = EntityState.Detached;
  }
}