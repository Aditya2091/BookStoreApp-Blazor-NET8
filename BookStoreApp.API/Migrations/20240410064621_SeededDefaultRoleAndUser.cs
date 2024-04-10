using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStoreApp.API.Migrations
{
    /// <inheritdoc />
    public partial class SeededDefaultRoleAndUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "079f8246-fc0c-47a3-bc46-ff2bcd833ae4", null, "Administrator", "ADMINISTRATOR" },
                    { "1f405c02-2d7e-4f1b-8fe0-0a72615a1195", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "5fd7beae-40b3-45f2-8dd6-8eebd57feaec", 0, "cf32c72e-5c42-4cc7-a675-756cda9391fe", "user@bookstore.com", false, "System", "User", false, null, "USER@BOOKSTORE.COM", "USER@BOOKSTORE.COM", "AQAAAAIAAYagAAAAEACT52VulHuMBJ01DvKmJFjaWSn/9r6eUQ/R2veUltrubx24C+wNKg5yk8l/x6vCnQ==", null, false, "367c3e48-78cc-49c9-8dc1-d0290bd457e2", false, "user@bookstore.com" },
                    { "c8b4522a-6024-4c5d-bad4-12fb0fb0028d", 0, "a48bde7a-1dd6-4c3a-ad82-cd8cdcdb0d8d", "admin@bookstore.com", false, "System", "Admin", false, null, "ADMIN@BOOKSTORE.COM", "ADMIN@BOOKSTORE.COM", "AQAAAAIAAYagAAAAEFoul6tZa18JaOXfhvyaO0yv62fk75WwZ92aqm3BdtbW0BIPTyccUccetgsHwGS2lg==", null, false, "e2e40c22-a6a0-4f30-b21d-22c713e76da2", false, "admin@bookstore.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "1f405c02-2d7e-4f1b-8fe0-0a72615a1195", "5fd7beae-40b3-45f2-8dd6-8eebd57feaec" },
                    { "079f8246-fc0c-47a3-bc46-ff2bcd833ae4", "c8b4522a-6024-4c5d-bad4-12fb0fb0028d" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1f405c02-2d7e-4f1b-8fe0-0a72615a1195", "5fd7beae-40b3-45f2-8dd6-8eebd57feaec" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "079f8246-fc0c-47a3-bc46-ff2bcd833ae4", "c8b4522a-6024-4c5d-bad4-12fb0fb0028d" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "079f8246-fc0c-47a3-bc46-ff2bcd833ae4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1f405c02-2d7e-4f1b-8fe0-0a72615a1195");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5fd7beae-40b3-45f2-8dd6-8eebd57feaec");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c8b4522a-6024-4c5d-bad4-12fb0fb0028d");
        }
    }
}
