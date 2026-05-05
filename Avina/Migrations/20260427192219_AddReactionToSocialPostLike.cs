using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Avina.Migrations
{
    /// <inheritdoc />
    public partial class AddReactionToSocialPostLike : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Reaction",
                table: "SocialPostLikes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reaction",
                table: "SocialPostLikes");
        }
    }
}
