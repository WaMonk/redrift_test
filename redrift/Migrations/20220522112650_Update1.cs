using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RedRift.Migrations
{
    public partial class Update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Lobbies",
                columns: table => new
                {
                    LobbyId = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Owner = table.Column<uint>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    OpponentId = table.Column<uint>(type: "INTEGER", nullable: true),
                    MatchId = table.Column<uint>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lobbies", x => x.LobbyId);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    MatchId = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerOneId = table.Column<uint>(type: "INTEGER", nullable: false),
                    PlayerTwoId = table.Column<uint>(type: "INTEGER", nullable: false),
                    PlayerOneHP = table.Column<uint>(type: "INTEGER", nullable: false),
                    PlayerTwoHP = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.MatchId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lobbies");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "Users");
        }
    }
}
