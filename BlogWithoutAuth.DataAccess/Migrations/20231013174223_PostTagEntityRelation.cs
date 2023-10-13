using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogWithoutAuth.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class PostTagEntityRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PostTag",
                table: "PostTag");

            migrationBuilder.AddColumn<Guid>(
                name: "PostId",
                table: "PostTag",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostTag",
                table: "PostTag",
                columns: new[] { "PostId", "TagsId" });

            migrationBuilder.CreateIndex(
                name: "IX_PostTag_PostsId",
                table: "PostTag",
                column: "PostsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PostTag",
                table: "PostTag");

            migrationBuilder.DropIndex(
                name: "IX_PostTag_PostsId",
                table: "PostTag");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "PostTag");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostTag",
                table: "PostTag",
                columns: new[] { "PostsId", "TagsId" });
        }
    }
}
