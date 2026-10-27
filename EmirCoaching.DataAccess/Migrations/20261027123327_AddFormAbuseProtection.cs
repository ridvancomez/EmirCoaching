using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmirCoaching.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddFormAbuseProtection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "NewsletterSubscribers",
                type: "nvarchar(45)",
                maxLength: 45,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_NewsletterSubscribers_IpAddress_CreatedAt",
                table: "NewsletterSubscribers",
                columns: new[] { "IpAddress", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_DietFormSubmissions_Email",
                table: "DietFormSubmissions",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_DietFormSubmissions_PhoneNumber",
                table: "DietFormSubmissions",
                column: "PhoneNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NewsletterSubscribers_IpAddress_CreatedAt",
                table: "NewsletterSubscribers");

            migrationBuilder.DropIndex(
                name: "IX_DietFormSubmissions_Email",
                table: "DietFormSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_DietFormSubmissions_PhoneNumber",
                table: "DietFormSubmissions");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "NewsletterSubscribers");
        }
    }
}
