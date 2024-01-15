using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KPSAllocator.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "kps_allocator_players",
                columns: table => new
                {
                    steamID = table.Column<ulong>(type: "bigint", nullable: false),
                    primaryWeaponTerrorist = table.Column<string>(type: "TEXT", nullable: false),
                    primaryWeaponCounterTerrorist = table.Column<string>(type: "TEXT", nullable: false),
                    secondaryWeaponTerrorist = table.Column<string>(type: "TEXT", nullable: false),
                    secondaryWeaponCounterTerrorist = table.Column<string>(type: "TEXT", nullable: false),
                    smgWeaponTerrorist = table.Column<string>(type: "TEXT", nullable: false),
                    smgWeaponCounterTerrorist = table.Column<string>(type: "TEXT", nullable: false),
                    enableSniperTerrorist = table.Column<bool>(type: "INTEGER", nullable: false),
                    enableSniperCounterTerrorist = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kps_allocator_players", x => x.steamID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "kps_allocator_players");
        }
    }
}
