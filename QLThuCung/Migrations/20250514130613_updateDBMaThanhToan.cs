using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLThuCung.Migrations
{
    /// <inheritdoc />
    public partial class updateDBMaThanhToan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MaThanhToan",
                table: "HoaDonSanPham",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaThanhToan",
                table: "HoaDonDichVu",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaThanhToan",
                table: "HoaDonSanPham");

            migrationBuilder.DropColumn(
                name: "MaThanhToan",
                table: "HoaDonDichVu");
        }
    }
}
