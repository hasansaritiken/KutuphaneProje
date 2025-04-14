using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kutuphane.Migrations
{
    /// <inheritdoc />
    public partial class Baslangıc2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ogrenciler_Siniflar_SinifId",
                table: "Ogrenciler");

            migrationBuilder.DropIndex(
                name: "IX_Ogrenciler_SinifId",
                table: "Ogrenciler");

            migrationBuilder.RenameColumn(
                name: "OgrenciSoyad",
                table: "Ogrenciler",
                newName: "OkulNumarasi");

            migrationBuilder.RenameColumn(
                name: "OgrenciAd",
                table: "Ogrenciler",
                newName: "OgrenciSoyadi");

            migrationBuilder.AlterColumn<string>(
                name: "SinifId",
                table: "Ogrenciler",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OgrenciAdi",
                table: "Ogrenciler",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SinifId1",
                table: "Ogrenciler",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ogrenciler_SinifId1",
                table: "Ogrenciler",
                column: "SinifId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Ogrenciler_Siniflar_SinifId1",
                table: "Ogrenciler",
                column: "SinifId1",
                principalTable: "Siniflar",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ogrenciler_Siniflar_SinifId1",
                table: "Ogrenciler");

            migrationBuilder.DropIndex(
                name: "IX_Ogrenciler_SinifId1",
                table: "Ogrenciler");

            migrationBuilder.DropColumn(
                name: "OgrenciAdi",
                table: "Ogrenciler");

            migrationBuilder.DropColumn(
                name: "SinifId1",
                table: "Ogrenciler");

            migrationBuilder.RenameColumn(
                name: "OkulNumarasi",
                table: "Ogrenciler",
                newName: "OgrenciSoyad");

            migrationBuilder.RenameColumn(
                name: "OgrenciSoyadi",
                table: "Ogrenciler",
                newName: "OgrenciAd");

            migrationBuilder.AlterColumn<int>(
                name: "SinifId",
                table: "Ogrenciler",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ogrenciler_SinifId",
                table: "Ogrenciler",
                column: "SinifId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ogrenciler_Siniflar_SinifId",
                table: "Ogrenciler",
                column: "SinifId",
                principalTable: "Siniflar",
                principalColumn: "Id");
        }
    }
}
