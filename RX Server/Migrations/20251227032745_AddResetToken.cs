using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RX_Server.Migrations
{
    /// <inheritdoc />
    public partial class AddResetToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Users.Email
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Users]') AND name = 'Email')
                BEGIN
                    ALTER TABLE [Users] ADD [Email] nvarchar(max) NULL;
                END");

            // Users.ProfileImageUrl
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Users]') AND name = 'ProfileImageUrl')
                BEGIN
                    ALTER TABLE [Users] ADD [ProfileImageUrl] nvarchar(max) NULL;
                END");

            // Users.ResetToken
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Users]') AND name = 'ResetToken')
                BEGIN
                    ALTER TABLE [Users] ADD [ResetToken] nvarchar(max) NULL;
                END");

            // Users.ResetTokenExpiry
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Users]') AND name = 'ResetTokenExpiry')
                BEGIN
                    ALTER TABLE [Users] ADD [ResetTokenExpiry] datetime2 NULL;
                END");

            // Songs.CoverImageUrl
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Songs]') AND name = 'CoverImageUrl')
                BEGIN
                    ALTER TABLE [Songs] ADD [CoverImageUrl] nvarchar(max) NULL;
                END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ResetToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ResetTokenExpiry",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CoverImageUrl",
                table: "Songs");
        }
    }
}
