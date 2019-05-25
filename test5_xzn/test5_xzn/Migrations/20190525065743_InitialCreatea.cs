using Microsoft.EntityFrameworkCore.Migrations;

namespace test5_xzn.Migrations
{
    public partial class InitialCreatea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "BuildingName",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "ClientName",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "OrganizationName",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Organizationdescription",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "ProjectAdress",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "ProjectIssueDate",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "ProjectName",
                table: "Blogs");

            migrationBuilder.RenameColumn(
                name: "ProjectStatus",
                table: "Blogs",
                newName: "Sitevalue");

            migrationBuilder.RenameColumn(
                name: "ProjectNumber",
                table: "Blogs",
                newName: "Sitekey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Sitevalue",
                table: "Blogs",
                newName: "ProjectStatus");

            migrationBuilder.RenameColumn(
                name: "Sitekey",
                table: "Blogs",
                newName: "ProjectNumber");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Blogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BuildingName",
                table: "Blogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Blogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientName",
                table: "Blogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationName",
                table: "Blogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Organizationdescription",
                table: "Blogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectAdress",
                table: "Blogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectIssueDate",
                table: "Blogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectName",
                table: "Blogs",
                nullable: true);
        }
    }
}
