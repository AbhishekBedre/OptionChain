using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptionChain.Migrations.UpStoxDb
{
    public partial class optionExpirySummary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "optionExpirySummaries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotOICE = table.Column<double>(type: "float", nullable: false),
                    TotOIPE = table.Column<double>(type: "float", nullable: false),
                    TotVolCE = table.Column<double>(type: "float", nullable: false),
                    TotVolPE = table.Column<double>(type: "float", nullable: false),
                    CEPEOIDiff = table.Column<double>(type: "float", nullable: false),
                    CEPEVolDiff = table.Column<double>(type: "float", nullable: false),
                    CEPEOIPrevDiff = table.Column<double>(type: "float", nullable: false),
                    CEPEVolPrevDiff = table.Column<double>(type: "float", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_optionExpirySummaries", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "optionExpirySummaries");
        }
    }
}
