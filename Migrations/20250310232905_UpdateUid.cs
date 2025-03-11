using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bangazon.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                column: "UserId",
                value: "c7O2PDuPP4XuAIeGC24V9LE64IJ3");

            migrationBuilder.UpdateData(
                table: "PaymentTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CustomerId",
                value: "c7O2PDuPP4XuAIeGC24V9LE64IJ3");

            migrationBuilder.UpdateData(
                table: "Sellers",
                keyColumn: "Id",
                keyValue: 1,
                column: "SellerId",
                value: "c7O2PDuPP4XuAIeGC24V9LE64IJ3");

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 1,
                column: "SellerId",
                value: "c7O2PDuPP4XuAIeGC24V9LE64IJ3");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Uid",
                value: "c7O2PDuPP4XuAIeGC24V9LE64IJ3");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                column: "UserId",
                value: "yZ123AbC456DeFgHijKlmN789Opq");

            migrationBuilder.UpdateData(
                table: "PaymentTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CustomerId",
                value: "yZ123AbC456DeFgHijKlmN789Opq");

            migrationBuilder.UpdateData(
                table: "Sellers",
                keyColumn: "Id",
                keyValue: 1,
                column: "SellerId",
                value: "yZ123AbC456DeFgHijKlmN789Opq");

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 1,
                column: "SellerId",
                value: "yZ123AbC456DeFgHijKlmN789Opq");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Uid",
                value: "yZ123AbC456DeFgHijKlmN789Opq");
        }
    }
}
