using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmirCoaching.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCoachProfileToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoachExperience",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoachStory",
                table: "AspNetUsers",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoachTitle",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoachVideoCoverPath",
                table: "AspNetUsers",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoachVideoUrl",
                table: "AspNetUsers",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCoach",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_IsCoach",
                table: "AspNetUsers",
                column: "IsCoach",
                unique: true,
                filter: "[IsCoach] = 1");

            // Mevcut kurulumda koç bölümü ilk kullanıcıdan (admin) gelir; metinler panelden değiştirilir
            migrationBuilder.Sql(
                "UPDATE AspNetUsers SET IsCoach = 1, " +
                "CoachTitle = N'Online Fitness & Beslenme Koçu', CoachExperience = N'8+ yıl', " +
                "CoachVideoCoverPath = N'/images/hakkimda-video.png', " +
                "CoachStory = N'Gerçek değişim geçici motivasyonlarla değil, sürdürülebilir sistemlerle başlar. Emircoaching olarak, " +
                "fiziksel potansiyelinizi ortaya çıkarmak için bilimsel temelli bir yaklaşım sunuyoruz.' + CHAR(10) + CHAR(10) + " +
                "N'Buraya 2-3 satırlık kişisel hikaye gelecek: nasıl başladın, neden online koçluk, danışana neyi farklı veriyorsun.' " +
                "WHERE Id = (SELECT MIN(Id) FROM AspNetUsers);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_IsCoach",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CoachExperience",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CoachStory",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CoachTitle",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CoachVideoCoverPath",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CoachVideoUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsCoach",
                table: "AspNetUsers");
        }
    }
}
