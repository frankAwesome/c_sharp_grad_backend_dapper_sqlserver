using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace c_sharp_grad_backend.Migrations
{
    public partial class SQLServerInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TableDonations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ExtraOne = table.Column<string>(nullable: true),
                    ExtraTwo = table.Column<string>(nullable: true),
                    ExtraThree = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableDonations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TableUserProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvatarOne = table.Column<byte[]>(nullable: true),
                    AvatarTwo = table.Column<byte[]>(nullable: true),
                    AvatarThree = table.Column<byte[]>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Firstname = table.Column<string>(nullable: true),
                    Lastname = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    AddressOne = table.Column<string>(nullable: true),
                    AddressTwo = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Zip = table.Column<string>(nullable: true),
                    PaymentType = table.Column<string>(nullable: true),
                    NameOnCard = table.Column<string>(nullable: true),
                    CardNumber = table.Column<string>(nullable: true),
                    ExpirationOnCard = table.Column<DateTime>(nullable: false),
                    CVV = table.Column<string>(nullable: true),
                    ExtraPropOne = table.Column<string>(nullable: true),
                    ExtraPropTwo = table.Column<string>(nullable: true),
                    ExtraPropThree = table.Column<string>(nullable: true),
                    ExtraPropFour = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableUserProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TableUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(nullable: true),
                    ProfileImage = table.Column<byte[]>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableUsers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TableDonations");

            migrationBuilder.DropTable(
                name: "TableUserProfiles");

            migrationBuilder.DropTable(
                name: "TableUsers");
        }
    }
}
