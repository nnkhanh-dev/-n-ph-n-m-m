using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLThuCung.Migrations
{
    /// <inheritdoc />
    public partial class AddGiuong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdGiuong",
                table: "HoaDonDichVu",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Giuong",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaGiuong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Giuong", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDichVu_IdGiuong",
                table: "HoaDonDichVu",
                column: "IdGiuong");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDichVu_Giuong_IdGiuong",
                table: "HoaDonDichVu",
                column: "IdGiuong",
                principalTable: "Giuong",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDichVu_Giuong_IdGiuong",
                table: "HoaDonDichVu");

            migrationBuilder.DropTable(
                name: "Giuong");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDichVu_IdGiuong",
                table: "HoaDonDichVu");

            migrationBuilder.DropColumn(
                name: "IdGiuong",
                table: "HoaDonDichVu");
        }
    }
}
