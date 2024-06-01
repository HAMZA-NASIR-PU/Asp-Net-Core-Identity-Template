using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityApp.Migrations
{
    /// <inheritdoc />
    public partial class AddOtpRelatedInfoToAppUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FailedOtpAttempts",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Otp",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "OtpExpiration",
                table: "AspNetUsers",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OtpLockoutEnd",
                table: "AspNetUsers",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FailedOtpAttempts",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Otp",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OtpExpiration",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OtpLockoutEnd",
                table: "AspNetUsers");
        }
    }
}
