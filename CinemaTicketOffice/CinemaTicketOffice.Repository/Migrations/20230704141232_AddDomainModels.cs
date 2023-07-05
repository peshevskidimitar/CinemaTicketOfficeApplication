using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CinemaTicketOffice.Repository.Migrations
{
    public partial class AddDomainModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCartSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OwnerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCartSet_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TicketSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MovieName = table.Column<string>(nullable: false),
                    MovieGenre = table.Column<string>(nullable: false),
                    MovieCoverImage = table.Column<string>(nullable: false),
                    MovieDescription = table.Column<string>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false),
                    Price = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketInOrder",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TicketId = table.Column<Guid>(nullable: false),
                    OrderId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketInOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketInOrder_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketInOrder_TicketSet_TicketId",
                        column: x => x.TicketId,
                        principalTable: "TicketSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketInShoppingCartSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TicketId = table.Column<Guid>(nullable: false),
                    ShoppingCartId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketInShoppingCartSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketInShoppingCartSet_ShoppingCartSet_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "ShoppingCartSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketInShoppingCartSet_TicketSet_TicketId",
                        column: x => x.TicketId,
                        principalTable: "TicketSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId",
                table: "Order",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartSet_OwnerId",
                table: "ShoppingCartSet",
                column: "OwnerId",
                unique: true,
                filter: "[OwnerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TicketInOrder_OrderId",
                table: "TicketInOrder",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketInOrder_TicketId",
                table: "TicketInOrder",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketInShoppingCartSet_ShoppingCartId",
                table: "TicketInShoppingCartSet",
                column: "ShoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketInShoppingCartSet_TicketId",
                table: "TicketInShoppingCartSet",
                column: "TicketId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketInOrder");

            migrationBuilder.DropTable(
                name: "TicketInShoppingCartSet");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "ShoppingCartSet");

            migrationBuilder.DropTable(
                name: "TicketSet");
        }
    }
}
