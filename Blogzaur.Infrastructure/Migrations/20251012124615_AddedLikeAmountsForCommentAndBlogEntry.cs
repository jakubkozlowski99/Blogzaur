using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blogzaur.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedLikeAmountsForCommentAndBlogEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LikeAmount",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LikeAmount",
                table: "BlogEntries",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LikeAmount",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "LikeAmount",
                table: "BlogEntries");
        }
    }
}
