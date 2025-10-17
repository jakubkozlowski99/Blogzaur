using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blogzaur.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Views",
                table: "BlogEntries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserBlogEntryLikes",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BlogEntryId = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LikedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBlogEntryLikes", x => new { x.UserId, x.BlogEntryId });
                    table.ForeignKey(
                        name: "FK_UserBlogEntryLikes_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserBlogEntryLikes_BlogEntries_BlogEntryId",
                        column: x => x.BlogEntryId,
                        principalTable: "BlogEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCommentLikes",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LikedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCommentLikes", x => new { x.UserId, x.CommentId });
                    table.ForeignKey(
                        name: "FK_UserCommentLikes_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserCommentLikes_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBlogEntryLikes_AuthorId",
                table: "UserBlogEntryLikes",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBlogEntryLikes_BlogEntryId",
                table: "UserBlogEntryLikes",
                column: "BlogEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCommentLikes_AuthorId",
                table: "UserCommentLikes",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCommentLikes_CommentId",
                table: "UserCommentLikes",
                column: "CommentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBlogEntryLikes");

            migrationBuilder.DropTable(
                name: "UserCommentLikes");

            migrationBuilder.DropColumn(
                name: "Views",
                table: "BlogEntries");
        }
    }
}
