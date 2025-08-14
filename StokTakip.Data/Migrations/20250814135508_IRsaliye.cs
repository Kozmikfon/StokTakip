using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StokTakip.Data.Migrations
{
    /// <inheritdoc />
    public partial class IRsaliye : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "durum",
                table: "irsaliyeler",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "durum",
                table: "irsaliyeler",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
