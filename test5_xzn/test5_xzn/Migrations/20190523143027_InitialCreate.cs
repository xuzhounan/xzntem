using Microsoft.EntityFrameworkCore.Migrations;

namespace test5_xzn.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Url = table.Column<string>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    BuildingName = table.Column<string>(nullable: true),
                    Organizationdescription = table.Column<string>(nullable: true),
                    OrganizationName = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    ClientName = table.Column<string>(nullable: true),
                    ProjectAdress = table.Column<string>(nullable: true),
                    ProjectIssueDate = table.Column<string>(nullable: true),
                    ProjectName = table.Column<string>(nullable: true),
                    ProjectNumber = table.Column<string>(nullable: true),
                    ProjectStatus = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Url);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blogs");
        }
    }
}
