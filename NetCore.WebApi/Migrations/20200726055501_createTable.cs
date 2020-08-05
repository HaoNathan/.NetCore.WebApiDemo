using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCore.WebApi.Migrations
{
    public partial class createTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(36)", maxLength: 36, nullable: false),
                    Name = table.Column<string>(maxLength: 12, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Industry = table.Column<string>(nullable: false),
                    Product = table.Column<string>(nullable: false),
                    Introduction = table.Column<string>(nullable: false),
                    BankruptTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(36)", maxLength: 36, nullable: false),
                    CompanyId = table.Column<string>(type: "char(36)", maxLength: 36, nullable: false),
                    EmployeeNo = table.Column<string>(maxLength: 12, nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Company",
                columns: new[] { "Id", "BankruptTime", "Country", "Industry", "Introduction", "Name", "Product" },
                values: new object[,]
                {
                    { "bbdee09c-089b-4d30-bece-44df5923716c", null, "USA", "Software", "Great Company", "Microsoft", "Software" },
                    { "bbdee09c-089b-4d30-bece-44df59237144", null, "USA", "Internet", "Not Exists?", "AOL", "Website" },
                    { "5efc910b-2f45-43df-afae-620d40542833", null, "USA", "ECommerce", "Store", "Amazon", "Books" },
                    { "6fb600c1-9011-4fd7-9234-881379716433", null, "China", "Internet", "Music?", "NetEase", "Songs" },
                    { "bbdee09c-089b-4d30-bece-44df59237133", null, "China", "ECommerce", "Brothers", "Jingdong", "Goods" },
                    { "5efc910b-2f45-43df-afae-620d40542822", null, "China", "Security", "- -", "360", "Security Product" },
                    { "6fb600c1-9011-4fd7-9234-881379716422", null, "USA", "Internet", "Blocked", "Youtube", "Videos" },
                    { "bbdee09c-089b-4d30-bece-44df59237122", null, "USA", "Internet", "Blocked", "Twitter", "Tweets" },
                    { "5efc910b-2f45-43df-afae-620d40542811", null, "China", "ECommerce", "From Jiangsu", "Suning", "Goods" },
                    { "6fb600c1-9011-4fd7-9234-881379716411", null, "Italy", "Football", "Football Club", "AC Milan", "Football Match" },
                    { "bbdee09c-089b-4d30-bece-44df59237111", null, "USA", "Technology", "Wow", "SpaceX", "Rocket" },
                    { "5efc910b-2f45-43df-afae-620d40542800", null, "USA", "Software", "Photoshop?", "Adobe", "Software" },
                    { "6fb600c1-9011-4fd7-9234-881379716400", null, "China", "Internet", "From Beijing", "Baidu", "Software" },
                    { "bbdee09c-089b-4d30-bece-44df59237100", null, "China", "ECommerce", "From Shenzhen", "Tencent", "Software" },
                    { "5efc910b-2f45-43df-afae-620d40542853", null, "China", "Internet", "Fubao Company", "Alipapa", "Software" },
                    { "6fb600c1-9011-4fd7-9234-881379716440", null, "USA", "Internet", "Don't be evil", "Google", "Software" },
                    { "6fb600c1-9011-4fd7-9234-881379716444", null, "USA", "Internet", "Who?", "Yahoo", "Mail" },
                    { "5efc910b-2f45-43df-afae-620d40542844", null, "USA", "Internet", "Is it a company?", "Firefox", "Browser" }
                });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "Id", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[,]
                {
                    { "4b501cb3-d168-4cc0-b375-48fb33f318a4", "bbdee09c-089b-4d30-bece-44df5923716c", new DateTime(1976, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "MSFT231", "Nick", 0, "Carter" },
                    { "7eaa532c-1be5-472c-a738-94fd26e5fad6", "bbdee09c-089b-4d30-bece-44df5923716c", new DateTime(1981, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "MSFT245", "Vince", 0, "Carter" },
                    { "72457e73-ea34-4e02-b575-8d384e82a481", "6fb600c1-9011-4fd7-9234-881379716440", new DateTime(1986, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "G003", "Mary", 1, "King" },
                    { "7644b71d-d74e-43e2-ac32-8cbadd7b1c3a", "6fb600c1-9011-4fd7-9234-881379716440", new DateTime(1977, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "G097", "Kevin", 0, "Richardson" },
                    { "679dfd33-32e4-4393-b061-f7abb8956f53", "5efc910b-2f45-43df-afae-620d40542853", new DateTime(1967, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "A009", "卡", 1, "里" },
                    { "1861341e-b42b-410c-ae21-cf11f36fc574", "5efc910b-2f45-43df-afae-620d40542853", new DateTime(1957, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "A404", "Not", 0, "Man" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_CompanyId",
                table: "Employee",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Company");
        }
    }
}
