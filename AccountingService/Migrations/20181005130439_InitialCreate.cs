using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountingService.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account_groups",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_groups", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "account_types",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    group_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "organizations",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organizations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    account_type_id = table.Column<Guid>(nullable: false),
                    group_id = table.Column<Guid>(nullable: false),
                    organization_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.id);
                    table.ForeignKey(
                        name: "FK_accounts_account_types_account_type_id",
                        column: x => x.account_type_id,
                        principalTable: "account_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_accounts_organizations_organization_id",
                        column: x => x.organization_id,
                        principalTable: "organizations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    email = table.Column<string>(nullable: false),
                    password_hash = table.Column<string>(nullable: false),
                    organization_id = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_organizations_organization_id",
                        column: x => x.organization_id,
                        principalTable: "organizations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    transaction_type = table.Column<int>(nullable: false),
                    account_id = table.Column<Guid>(nullable: true),
                    amount = table.Column<double>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    transaction_date = table.Column<DateTime>(nullable: true),
                    organization_id = table.Column<Guid>(nullable: false),
                    note = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.id);
                    table.ForeignKey(
                        name: "FK_transactions_accounts_account_id",
                        column: x => x.account_id,
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "transaction_items",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    transaction_id = table.Column<Guid>(nullable: false),
                    type = table.Column<string>(nullable: false),
                    amount = table.Column<double>(nullable: false),
                    account_id = table.Column<Guid>(nullable: false),
                    organization_id = table.Column<Guid>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_transaction_items_transactions_transaction_id",
                        column: x => x.transaction_id,
                        principalTable: "transactions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transaction_items_transactions_transaction_id1",
                        column: x => x.transaction_id,
                        principalTable: "transactions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transaction_items_accounts_account_id",
                        column: x => x.account_id,
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "account_groups",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("06e3346c-7430-48e5-bec5-5049c3fc88a4"), "Assets" },
                    { new Guid("15d7775b-e017-4437-938f-4f264abd3d8a"), "Liabilities" },
                    { new Guid("8b27c505-29c3-4a90-a6cf-5b3abc30cfeb"), "Equity" }
                });

            migrationBuilder.InsertData(
                table: "account_types",
                columns: new[] { "id", "group_id", "name" },
                values: new object[,]
                {
                    { new Guid("b9ec0e7c-c17d-4edf-8f1d-7b8cdbcc39f4"), new Guid("06e3346c-7430-48e5-bec5-5049c3fc88a4"), "Cash/Bank" },
                    { new Guid("b4af6ec9-bbf8-46d6-8d46-e48fb614f5ce"), new Guid("06e3346c-7430-48e5-bec5-5049c3fc88a4"), "Money in Transit" },
                    { new Guid("9a12615f-f3dd-4edd-9af6-1171c998dd25"), new Guid("06e3346c-7430-48e5-bec5-5049c3fc88a4"), "Payments from Sales" },
                    { new Guid("1dcd2296-2c2f-4a28-9aba-93f148bc6a66"), new Guid("15d7775b-e017-4437-938f-4f264abd3d8a"), "Credit Card" },
                    { new Guid("4a2da79d-2ef7-47c8-b3c8-c43df9605fe2"), new Guid("15d7775b-e017-4437-938f-4f264abd3d8a"), "Loan and Line of Credit" },
                    { new Guid("129029e9-4505-4064-a230-715bae91f187"), new Guid("15d7775b-e017-4437-938f-4f264abd3d8a"), "Taxes" },
                    { new Guid("39431e66-cde1-4018-929d-ef4c3e56b907"), new Guid("8b27c505-29c3-4a90-a6cf-5b3abc30cfeb"), "Own Contribution" },
                    { new Guid("20e1fc3b-9ee8-43c4-8a71-6d39726e98bf"), new Guid("8b27c505-29c3-4a90-a6cf-5b3abc30cfeb"), "Drawing" }
                });

            migrationBuilder.InsertData(
                table: "organizations",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("a91917a6-3b46-4e58-926e-de9fe8fe497b"), "Organization 1" },
                    { new Guid("d61ffa8d-171e-4fcd-a1ca-2248c34687fe"), "Organization 2" },
                    { new Guid("b3520fc8-aa69-40a9-9008-5b3aa824ae36"), "Organization 3" }
                });

            migrationBuilder.InsertData(
                table: "accounts",
                columns: new[] { "id", "account_type_id", "description", "group_id", "name", "organization_id" },
                values: new object[] { new Guid("da8a6668-786b-4eb7-8579-b20281a11f4b"), new Guid("b9ec0e7c-c17d-4edf-8f1d-7b8cdbcc39f4"), null, new Guid("06e3346c-7430-48e5-bec5-5049c3fc88a4"), "Cash at Hand", new Guid("a91917a6-3b46-4e58-926e-de9fe8fe497b") });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "email", "name", "organization_id", "password_hash" },
                values: new object[,]
                {
                    { new Guid("eeff65d8-6ee3-4acc-bbcb-35abf13982fe"), "user1@Organization1.com", "User 1", new Guid("a91917a6-3b46-4e58-926e-de9fe8fe497b"), "AQAAAAEAACcQAAAAEMsaalGFShjtzERSdhrP8IF+VuQ4JRpJYnZodsPLW281BeIVwonqFxKojgb7fuBNnw==" },
                    { new Guid("9ee8f165-4582-48ae-b2b0-971c36061385"), "user2@Organization2.com", "User 2", new Guid("d61ffa8d-171e-4fcd-a1ca-2248c34687fe"), "AQAAAAEAACcQAAAAEM3aaLzrIRQCXO27CrSfeVdYmWhGvHZ4dIz5r37y9Av6AiXaxuQCX8CTgaqkESFHDQ==" },
                    { new Guid("7fb41a45-55ed-4920-bf66-4fe6c050a528"), "user3@Organization3.com", "User 3", new Guid("b3520fc8-aa69-40a9-9008-5b3aa824ae36"), "AQAAAAEAACcQAAAAEMjnyh00ppL0PlFKNbTJ0ZIW30OONT8ExIp6DyomCPqYe3OFcFDRkmM1yVAesmElzQ==" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_accounts_account_type_id",
                table: "accounts",
                column: "account_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_accounts_organization_id",
                table: "accounts",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_items_transaction_id",
                table: "transaction_items",
                column: "transaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_items_transaction_id1",
                table: "transaction_items",
                column: "transaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_items_account_id",
                table: "transaction_items",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_account_id",
                table: "transactions",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_organization_id",
                table: "users",
                column: "organization_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_groups");

            migrationBuilder.DropTable(
                name: "transaction_items");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "account_types");

            migrationBuilder.DropTable(
                name: "organizations");
        }
    }
}
