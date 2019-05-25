﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using test5_xzn;

namespace test5_xzn.Migrations
{
    [DbContext(typeof(MainWindow.BloggingContext))]
    [Migration("20190523143027_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("test5_xzn.MainWindow+Blog", b =>
                {
                    b.Property<string>("Url")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<string>("BuildingName");

                    b.Property<string>("Category");

                    b.Property<string>("ClientName");

                    b.Property<string>("OrganizationName");

                    b.Property<string>("Organizationdescription");

                    b.Property<string>("ProjectAdress");

                    b.Property<string>("ProjectIssueDate");

                    b.Property<string>("ProjectName");

                    b.Property<string>("ProjectNumber");

                    b.Property<string>("ProjectStatus");

                    b.HasKey("Url");

                    b.ToTable("Blogs");
                });
#pragma warning restore 612, 618
        }
    }
}