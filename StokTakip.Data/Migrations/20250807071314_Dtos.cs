using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StokTakip.Data.Migrations
{
    /// <inheritdoc />
    public partial class Dtos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdSoyad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cariler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    unvan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    telefon = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    adres = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    vergiNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    vergiDairesi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedByName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cariler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Depolar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    depoAd = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    rafBilgisi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    aciklama = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    konumBilgisi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedByName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Depolar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "kullanicilar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    adSoyad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    olusturulmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedByName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kullanicilar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Malzemeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    malzemeAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    birim = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    kategori = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    minStokMiktar = table.Column<int>(type: "int", nullable: false),
                    barkodNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    aktifPasif = table.Column<bool>(type: "bit", nullable: false),
                    aciklama = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedByName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Malzemeler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "logTakipler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tabloAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    islemTipi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    islemTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    detay = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedByName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logTakipler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_logTakipler_AppUser_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "depoTransferleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    transferNo = table.Column<int>(type: "int", nullable: false),
                    kaynakDepoId = table.Column<int>(type: "int", nullable: false),
                    hedefDepoId = table.Column<int>(type: "int", nullable: false),
                    transferTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    aciklama = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    seriNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedByName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_depoTransferleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_depoTransferleri_Depolar_hedefDepoId",
                        column: x => x.hedefDepoId,
                        principalTable: "Depolar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_depoTransferleri_Depolar_kaynakDepoId",
                        column: x => x.kaynakDepoId,
                        principalTable: "Depolar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "irsaliyeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    irsaliyeNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    carId = table.Column<int>(type: "int", nullable: false),
                    irsaliyeTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    toplamTutar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    irsaliyeTipi = table.Column<int>(type: "int", nullable: false),
                    aciklama = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    durum = table.Column<bool>(type: "bit", nullable: false),
                    depoId = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedByName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_irsaliyeler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_irsaliyeler_Cariler_carId",
                        column: x => x.carId,
                        principalTable: "Cariler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_irsaliyeler_Depolar_depoId",
                        column: x => x.depoId,
                        principalTable: "Depolar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Stoklar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MalzemeId = table.Column<int>(type: "int", nullable: false),
                    DepoId = table.Column<int>(type: "int", nullable: false),
                    HareketTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Miktar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HareketTipi = table.Column<int>(type: "int", nullable: false),
                    ReferansId = table.Column<int>(type: "int", nullable: true),
                    Aciklama = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    carId = table.Column<int>(type: "int", nullable: true),
                    SeriNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedByName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stoklar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stoklar_Cariler_carId",
                        column: x => x.carId,
                        principalTable: "Cariler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Stoklar_Depolar_DepoId",
                        column: x => x.DepoId,
                        principalTable: "Depolar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Stoklar_Malzemeler_MalzemeId",
                        column: x => x.MalzemeId,
                        principalTable: "Malzemeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "depoTransferDetaylari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    transferId = table.Column<int>(type: "int", nullable: false),
                    malzemeId = table.Column<int>(type: "int", nullable: false),
                    miktar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedByName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_depoTransferDetaylari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_depoTransferDetaylari_Malzemeler_malzemeId",
                        column: x => x.malzemeId,
                        principalTable: "Malzemeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_depoTransferDetaylari_depoTransferleri_transferId",
                        column: x => x.transferId,
                        principalTable: "depoTransferleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "irsaliyeDetaylari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    irsaliyeId = table.Column<int>(type: "int", nullable: false),
                    malzemeId = table.Column<int>(type: "int", nullable: false),
                    miktar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    birimFiyat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    araToplam = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    seriNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedByName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_irsaliyeDetaylari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_irsaliyeDetaylari_Malzemeler_malzemeId",
                        column: x => x.malzemeId,
                        principalTable: "Malzemeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_irsaliyeDetaylari_irsaliyeler_irsaliyeId",
                        column: x => x.irsaliyeId,
                        principalTable: "irsaliyeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_depoTransferDetaylari_malzemeId",
                table: "depoTransferDetaylari",
                column: "malzemeId");

            migrationBuilder.CreateIndex(
                name: "IX_depoTransferDetaylari_transferId",
                table: "depoTransferDetaylari",
                column: "transferId");

            migrationBuilder.CreateIndex(
                name: "IX_depoTransferleri_hedefDepoId",
                table: "depoTransferleri",
                column: "hedefDepoId");

            migrationBuilder.CreateIndex(
                name: "IX_depoTransferleri_kaynakDepoId",
                table: "depoTransferleri",
                column: "kaynakDepoId");

            migrationBuilder.CreateIndex(
                name: "IX_irsaliyeDetaylari_irsaliyeId",
                table: "irsaliyeDetaylari",
                column: "irsaliyeId");

            migrationBuilder.CreateIndex(
                name: "IX_irsaliyeDetaylari_malzemeId",
                table: "irsaliyeDetaylari",
                column: "malzemeId");

            migrationBuilder.CreateIndex(
                name: "IX_irsaliyeler_carId",
                table: "irsaliyeler",
                column: "carId");

            migrationBuilder.CreateIndex(
                name: "IX_irsaliyeler_depoId",
                table: "irsaliyeler",
                column: "depoId");

            migrationBuilder.CreateIndex(
                name: "IX_logTakipler_AppUserId",
                table: "logTakipler",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Stoklar_carId",
                table: "Stoklar",
                column: "carId");

            migrationBuilder.CreateIndex(
                name: "IX_Stoklar_DepoId",
                table: "Stoklar",
                column: "DepoId");

            migrationBuilder.CreateIndex(
                name: "IX_Stoklar_MalzemeId",
                table: "Stoklar",
                column: "MalzemeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "depoTransferDetaylari");

            migrationBuilder.DropTable(
                name: "irsaliyeDetaylari");

            migrationBuilder.DropTable(
                name: "kullanicilar");

            migrationBuilder.DropTable(
                name: "logTakipler");

            migrationBuilder.DropTable(
                name: "Stoklar");

            migrationBuilder.DropTable(
                name: "depoTransferleri");

            migrationBuilder.DropTable(
                name: "irsaliyeler");

            migrationBuilder.DropTable(
                name: "AppUser");

            migrationBuilder.DropTable(
                name: "Malzemeler");

            migrationBuilder.DropTable(
                name: "Cariler");

            migrationBuilder.DropTable(
                name: "Depolar");
        }
    }
}
