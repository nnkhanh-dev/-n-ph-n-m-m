using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLThuCung.Migrations
{
    /// <inheritdoc />
    public partial class TaoBangGiuong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Giuong",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaGiuong = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KhuVuc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoaiGiuong = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DangSuDung = table.Column<bool>(type: "bit", nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThuCungId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Giuong", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Giuong_ThuCung_ThuCungId",
                        column: x => x.ThuCungId,
                        principalTable: "ThuCung",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Giuong_ThuCungId",
                table: "Giuong",
                column: "ThuCungId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Giuong");
        }
    }
}
