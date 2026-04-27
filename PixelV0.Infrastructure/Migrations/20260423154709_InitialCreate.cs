using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PixelV0.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "catalog");

            migrationBuilder.EnsureSchema(
                name: "social");

            migrationBuilder.EnsureSchema(
                name: "identity");

            migrationBuilder.CreateTable(
                name: "Catalog",
                schema: "catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Game_Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Publisher = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    AvatarUrl = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Presets",
                schema: "social",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    User_ID = table.Column<Guid>(type: "uuid", nullable: true),
                    Game_ID = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    Values = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Presets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Presets_Catalog_Game_ID",
                        column: x => x.Game_ID,
                        principalSchema: "catalog",
                        principalTable: "Catalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Presets_Users_User_ID",
                        column: x => x.User_ID,
                        principalSchema: "identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                schema: "social",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    User_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Game_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Preset_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Image_URL = table.Column<string>(type: "text", nullable: false),
                    Caption = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Catalog_Game_ID",
                        column: x => x.Game_ID,
                        principalSchema: "catalog",
                        principalTable: "Catalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Posts_Presets_Preset_ID",
                        column: x => x.Preset_ID,
                        principalSchema: "social",
                        principalTable: "Presets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Posts_Users_User_ID",
                        column: x => x.User_ID,
                        principalSchema: "identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Likes",
                schema: "social",
                columns: table => new
                {
                    User_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Post_ID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => new { x.User_ID, x.Post_ID });
                    table.ForeignKey(
                        name: "FK_Likes_Posts_Post_ID",
                        column: x => x.Post_ID,
                        principalSchema: "social",
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Likes_Users_User_ID",
                        column: x => x.User_ID,
                        principalSchema: "identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Likes_Post_ID",
                schema: "social",
                table: "Likes",
                column: "Post_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CreatedAt",
                schema: "social",
                table: "Posts",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Game_ID",
                schema: "social",
                table: "Posts",
                column: "Game_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Preset_ID",
                schema: "social",
                table: "Posts",
                column: "Preset_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_User_ID",
                schema: "social",
                table: "Posts",
                column: "User_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Presets_Game_ID",
                schema: "social",
                table: "Presets",
                column: "Game_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Presets_User_ID",
                schema: "social",
                table: "Presets",
                column: "User_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "identity",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                schema: "identity",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Likes",
                schema: "social");

            migrationBuilder.DropTable(
                name: "Posts",
                schema: "social");

            migrationBuilder.DropTable(
                name: "Presets",
                schema: "social");

            migrationBuilder.DropTable(
                name: "Catalog",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "identity");
        }
    }
}
