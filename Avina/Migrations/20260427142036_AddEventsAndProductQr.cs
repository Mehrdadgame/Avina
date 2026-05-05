using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Avina.Migrations
{
    /// <inheritdoc />
    public partial class AddEventsAndProductQr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QrCode",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RelatedPathId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RelatedSkillId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnlockBonusCoin",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnlockBonusXP",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GrowthEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    RelatedPathId = table.Column<int>(type: "int", nullable: true),
                    RequiredLevel = table.Column<int>(type: "int", nullable: false),
                    CoinCost = table.Column<int>(type: "int", nullable: false),
                    RequiresAdminApproval = table.Column<bool>(type: "bit", nullable: false),
                    RewardXP = table.Column<int>(type: "int", nullable: false),
                    RewardCoin = table.Column<int>(type: "int", nullable: false),
                    PostEventMissionId = table.Column<int>(type: "int", nullable: true),
                    CoverImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrowthEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrowthEvents_GrowthPaths_RelatedPathId",
                        column: x => x.RelatedPathId,
                        principalTable: "GrowthPaths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_GrowthEvents_Missions_PostEventMissionId",
                        column: x => x.PostEventMissionId,
                        principalTable: "Missions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ProductQrUnlocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    QrCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnlockedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GrantedCoin = table.Column<int>(type: "int", nullable: false),
                    GrantedXP = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductQrUnlocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductQrUnlocks_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductQrUnlocks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventRegistrations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AttendedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RewardGranted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventRegistrations_GrowthEvents_EventId",
                        column: x => x.EventId,
                        principalTable: "GrowthEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventRegistrations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "QrCode", "RelatedPathId", "RelatedSkillId", "UnlockBonusCoin", "UnlockBonusXP" },
                values: new object[] { null, null, null, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "QrCode", "RelatedPathId", "RelatedSkillId", "UnlockBonusCoin", "UnlockBonusXP" },
                values: new object[] { null, null, null, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "QrCode", "RelatedPathId", "RelatedSkillId", "UnlockBonusCoin", "UnlockBonusXP" },
                values: new object[] { null, null, null, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "QrCode", "RelatedPathId", "RelatedSkillId", "UnlockBonusCoin", "UnlockBonusXP" },
                values: new object[] { null, null, null, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "QrCode", "RelatedPathId", "RelatedSkillId", "UnlockBonusCoin", "UnlockBonusXP" },
                values: new object[] { null, null, null, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "QrCode", "RelatedPathId", "RelatedSkillId", "UnlockBonusCoin", "UnlockBonusXP" },
                values: new object[] { null, null, null, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "QrCode", "RelatedPathId", "RelatedSkillId", "UnlockBonusCoin", "UnlockBonusXP" },
                values: new object[] { null, null, null, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "QrCode", "RelatedPathId", "RelatedSkillId", "UnlockBonusCoin", "UnlockBonusXP" },
                values: new object[] { null, null, null, 0, 0 });

            migrationBuilder.CreateIndex(
                name: "IX_Products_QrCode",
                table: "Products",
                column: "QrCode",
                unique: true,
                filter: "[QrCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Products_RelatedPathId",
                table: "Products",
                column: "RelatedPathId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_RelatedSkillId",
                table: "Products",
                column: "RelatedSkillId");

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistrations_EventId_UserId",
                table: "EventRegistrations",
                columns: new[] { "EventId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistrations_UserId",
                table: "EventRegistrations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GrowthEvents_PostEventMissionId",
                table: "GrowthEvents",
                column: "PostEventMissionId");

            migrationBuilder.CreateIndex(
                name: "IX_GrowthEvents_RelatedPathId",
                table: "GrowthEvents",
                column: "RelatedPathId");

            migrationBuilder.CreateIndex(
                name: "IX_GrowthEvents_Status_StartAt",
                table: "GrowthEvents",
                columns: new[] { "Status", "StartAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductQrUnlocks_ProductId",
                table: "ProductQrUnlocks",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductQrUnlocks_UserId_ProductId",
                table: "ProductQrUnlocks",
                columns: new[] { "UserId", "ProductId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_GrowthPaths_RelatedPathId",
                table: "Products",
                column: "RelatedPathId",
                principalTable: "GrowthPaths",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Skills_RelatedSkillId",
                table: "Products",
                column: "RelatedSkillId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_GrowthPaths_RelatedPathId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Skills_RelatedSkillId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "EventRegistrations");

            migrationBuilder.DropTable(
                name: "ProductQrUnlocks");

            migrationBuilder.DropTable(
                name: "GrowthEvents");

            migrationBuilder.DropIndex(
                name: "IX_Products_QrCode",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_RelatedPathId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_RelatedSkillId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "QrCode",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RelatedPathId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RelatedSkillId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UnlockBonusCoin",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UnlockBonusXP",
                table: "Products");
        }
    }
}
