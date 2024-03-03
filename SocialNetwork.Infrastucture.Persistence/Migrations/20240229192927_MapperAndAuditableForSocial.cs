using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetwork.Infrastucture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MapperAndAuditableForSocial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "SocialLinks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "SocialLinks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "SocialLinks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "SocialLinks",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "SocialLinks");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SocialLinks");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "SocialLinks");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "SocialLinks");
        }
    }
}
