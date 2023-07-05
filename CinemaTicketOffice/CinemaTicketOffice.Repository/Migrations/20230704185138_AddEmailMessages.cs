using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CinemaTicketOffice.Repository.Migrations
{
    public partial class AddEmailMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_UserId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketInOrder_Order_OrderId",
                table: "TicketInOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketInOrder_TicketSet_TicketId",
                table: "TicketInOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketInOrder",
                table: "TicketInOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.RenameTable(
                name: "TicketInOrder",
                newName: "TicketInOrderSet");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Orders");

            migrationBuilder.RenameIndex(
                name: "IX_TicketInOrder_TicketId",
                table: "TicketInOrderSet",
                newName: "IX_TicketInOrderSet_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketInOrder_OrderId",
                table: "TicketInOrderSet",
                newName: "IX_TicketInOrderSet_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_UserId",
                table: "Orders",
                newName: "IX_Orders_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketInOrderSet",
                table: "TicketInOrderSet",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "EmailMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MailTo = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailMessages", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketInOrderSet_Orders_OrderId",
                table: "TicketInOrderSet",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketInOrderSet_TicketSet_TicketId",
                table: "TicketInOrderSet",
                column: "TicketId",
                principalTable: "TicketSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketInOrderSet_Orders_OrderId",
                table: "TicketInOrderSet");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketInOrderSet_TicketSet_TicketId",
                table: "TicketInOrderSet");

            migrationBuilder.DropTable(
                name: "EmailMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketInOrderSet",
                table: "TicketInOrderSet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "TicketInOrderSet",
                newName: "TicketInOrder");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Order");

            migrationBuilder.RenameIndex(
                name: "IX_TicketInOrderSet_TicketId",
                table: "TicketInOrder",
                newName: "IX_TicketInOrder_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketInOrderSet_OrderId",
                table: "TicketInOrder",
                newName: "IX_TicketInOrder_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_UserId",
                table: "Order",
                newName: "IX_Order_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketInOrder",
                table: "TicketInOrder",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_UserId",
                table: "Order",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketInOrder_Order_OrderId",
                table: "TicketInOrder",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketInOrder_TicketSet_TicketId",
                table: "TicketInOrder",
                column: "TicketId",
                principalTable: "TicketSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
