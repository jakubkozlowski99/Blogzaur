using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blogzaur.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LikeEntitiesFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBlogEntryLikes_AspNetUsers_AuthorId",
                table: "UserBlogEntryLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCommentLikes_AspNetUsers_AuthorId",
                table: "UserCommentLikes");

            migrationBuilder.DropIndex(
                name: "IX_UserCommentLikes_AuthorId",
                table: "UserCommentLikes");

            migrationBuilder.DropIndex(
                name: "IX_UserBlogEntryLikes_AuthorId",
                table: "UserBlogEntryLikes");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "UserCommentLikes");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "UserBlogEntryLikes");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBlogEntryLikes_AspNetUsers_UserId",
                table: "UserBlogEntryLikes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCommentLikes_AspNetUsers_UserId",
                table: "UserCommentLikes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBlogEntryLikes_AspNetUsers_UserId",
                table: "UserBlogEntryLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCommentLikes_AspNetUsers_UserId",
                table: "UserCommentLikes");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "UserCommentLikes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "UserBlogEntryLikes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCommentLikes_AuthorId",
                table: "UserCommentLikes",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBlogEntryLikes_AuthorId",
                table: "UserBlogEntryLikes",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBlogEntryLikes_AspNetUsers_AuthorId",
                table: "UserBlogEntryLikes",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCommentLikes_AspNetUsers_AuthorId",
                table: "UserCommentLikes",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
