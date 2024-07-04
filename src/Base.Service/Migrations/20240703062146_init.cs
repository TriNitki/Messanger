using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Base.Service.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "features",
                columns: table => new
                {
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_features", x => x.name);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role", x => x.name);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    login = table.Column<string>(type: "text", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    is_blocked = table.Column<bool>(type: "boolean", nullable: false),
                    email = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                    table.UniqueConstraint("ak_users_login", x => x.login);
                });

            migrationBuilder.CreateTable(
                name: "role_to_feature",
                columns: table => new
                {
                    role_id = table.Column<string>(type: "text", nullable: false),
                    feature_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_to_feature", x => new { x.role_id, x.feature_id });
                    table.ForeignKey(
                        name: "fk_role_to_feature_features_feature_id",
                        column: x => x.feature_id,
                        principalTable: "features",
                        principalColumn: "name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_role_to_feature_role_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    token = table.Column<string>(type: "text", nullable: false),
                    expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_used = table.Column<bool>(type: "boolean", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_refresh_tokens", x => x.token);
                    table.ForeignKey(
                        name: "fk_refresh_tokens_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_to_user",
                columns: table => new
                {
                    role_id = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_to_user", x => new { x.role_id, x.user_id });
                    table.ForeignKey(
                        name: "fk_role_to_user_role_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_role_to_user_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_user_id",
                table: "refresh_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_role_to_feature_feature_id",
                table: "role_to_feature",
                column: "feature_id");

            migrationBuilder.CreateIndex(
                name: "ix_role_to_user_user_id",
                table: "role_to_user",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DropTable(
                name: "role_to_feature");

            migrationBuilder.DropTable(
                name: "role_to_user");

            migrationBuilder.DropTable(
                name: "features");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
