using KPSAllocator.Modules.Player;
using Microsoft.EntityFrameworkCore;

namespace KPSAllocator.Modules.Database;

public class Query
{

  public static PlayerDatabase? GetPlayer(ulong steamID)
  {
    return Database.Instance().Players.FirstOrDefault(player => player.steamID == steamID);
  }

  public static void UpdateOrAddPlayer(AllocatorPlayer player)
  {
    var dbPlayer = GetPlayer(player.GetSteamID());
    bool isNew = false;
    if (dbPlayer is null)
    {
      dbPlayer = player.ToPlayerDatabase();
      Database.Instance().Players.Add(dbPlayer);
      isNew = true;
    }
    if (!isNew)
      dbPlayer.UpdateFromAllocatorPlayer(player);
    Database.Instance().Players.Entry(dbPlayer).State = isNew ? EntityState.Added : EntityState.Modified;
    Database.Instance().SaveChanges();
    Database.Instance().Entry(dbPlayer).State = EntityState.Detached;
  }

  public static void Migrate()
  {
    Database.Instance().Database.Migrate();
  }
}