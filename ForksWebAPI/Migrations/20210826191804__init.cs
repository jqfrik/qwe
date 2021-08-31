using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class _init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bet",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Bookmaker = table.Column<int>(nullable: false),
                    Coef = table.Column<decimal>(nullable: false),
                    Direction = table.Column<int>(nullable: false),
                    BetType = table.Column<int>(nullable: false),
                    Sport = table.Column<int>(nullable: false),
                    Parametr = table.Column<double>(nullable: false),
                    BetValue = table.Column<string>(nullable: true),
                    ForksCount = table.Column<int>(nullable: false),
                    EvId = table.Column<string>(nullable: true),
                    OtherData = table.Column<string>(nullable: true),
                    Team = table.Column<string>(nullable: true),
                    MatchData = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    IsReq = table.Column<bool>(nullable: false),
                    IsInitiator = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestAnotherBets",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(nullable: false),
                    CridId = table.Column<string>(nullable: false),
                    Key = table.Column<string>(nullable: false),
                    AnotherBetNumber = table.Column<int>(nullable: false),
                    DateCreation = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestAnotherBets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestBetDatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(nullable: false),
                    CridId = table.Column<string>(nullable: false),
                    Key = table.Column<string>(nullable: false),
                    DateCreation = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestBetDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResponseAnotherBets",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(nullable: false),
                    CridId = table.Column<string>(nullable: false),
                    Key = table.Column<string>(nullable: false),
                    AnotherBetNumber = table.Column<int>(nullable: false),
                    DateCreation = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponseAnotherBets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResponseBetDatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(nullable: false),
                    CridId = table.Column<string>(nullable: false),
                    DateCreation = table.Column<DateTime>(nullable: false),
                    Key = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponseBetDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Key = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Hash = table.Column<string>(nullable: false),
                    Roles = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Forks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    ForkId = table.Column<int>(nullable: false),
                    UpdateCount = table.Column<int>(nullable: false),
                    Time = table.Column<TimeSpan>(nullable: false),
                    Profit = table.Column<decimal>(nullable: false),
                    Sport = table.Column<int>(nullable: false),
                    BetType = table.Column<int>(nullable: false),
                    OneBetId = table.Column<long>(nullable: true),
                    TwoBetId = table.Column<long>(nullable: true),
                    CridId = table.Column<string>(nullable: true),
                    K1 = table.Column<string>(nullable: true),
                    K2 = table.Column<string>(nullable: true),
                    Elid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Forks_Bet_OneBetId",
                        column: x => x.OneBetId,
                        principalTable: "Bet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Forks_Bet_TwoBetId",
                        column: x => x.TwoBetId,
                        principalTable: "Bet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Forks_OneBetId",
                table: "Forks",
                column: "OneBetId");

            migrationBuilder.CreateIndex(
                name: "IX_Forks_TwoBetId",
                table: "Forks",
                column: "TwoBetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Forks");

            migrationBuilder.DropTable(
                name: "RequestAnotherBets");

            migrationBuilder.DropTable(
                name: "RequestBetDatas");

            migrationBuilder.DropTable(
                name: "ResponseAnotherBets");

            migrationBuilder.DropTable(
                name: "ResponseBetDatas");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Bet");
        }
    }
}
