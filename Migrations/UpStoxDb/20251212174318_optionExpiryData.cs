using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptionChain.Migrations.UpStoxDb
{
    public partial class optionExpiryData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OptionExpiryDatas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockMetaDataId = table.Column<long>(type: "bigint", nullable: false),
                    Expiry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StrikePrice = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    StrikePCR = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    SpotPrice = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    CallOI = table.Column<long>(type: "bigint", nullable: false),
                    CallLTP = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    CallVolume = table.Column<long>(type: "bigint", nullable: false),
                    CallPrevOI = table.Column<long>(type: "bigint", nullable: false),
                    PutOI = table.Column<long>(type: "bigint", nullable: false),
                    PutLTP = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    PutVolume = table.Column<long>(type: "bigint", nullable: false),
                    PutPrevOI = table.Column<long>(type: "bigint", nullable: false),
                    OpenContractChange = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionExpiryDatas", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OptionExpiryDatas");
        }
    }
}
