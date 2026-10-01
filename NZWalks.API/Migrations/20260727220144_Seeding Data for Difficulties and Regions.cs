using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZWalks.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDataforDifficultiesandRegions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("80b45334-5da6-4ac0-87ab-38f3aa21e028"), "Medium" },
                    { new Guid("b90bef48-ee7a-4642-a325-edfc1e937652"), "Easy" },
                    { new Guid("bfdad830-43e7-46fb-a747-606598e966a8"), "Hard" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Code", "Name", "RegionImageUrl" },
                values: new object[,]
                {
                    { new Guid("2f63bce6-6180-4e46-86a9-e8d520d361b0"), "AKL", "Auckland", "https://test-img.com" },
                    { new Guid("6e87f11f-f820-4420-9f4e-73b77f04598f"), "STL", "Southland", null },
                    { new Guid("8a50f623-e511-4d5d-aaa9-e13664b6bfe1"), "NSN", "Nelson", "https://nsn.jpg" },
                    { new Guid("ab64dc99-dcb0-44bf-96de-807c77ce3430"), "WGN", "Wellington", "https://wgn.jpg" },
                    { new Guid("c527d5da-5ff5-4357-b41a-39020a5dee03"), "BOP", "Bay of Plenty", "https://bop.jpg" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("80b45334-5da6-4ac0-87ab-38f3aa21e028"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("b90bef48-ee7a-4642-a325-edfc1e937652"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("bfdad830-43e7-46fb-a747-606598e966a8"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("2f63bce6-6180-4e46-86a9-e8d520d361b0"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("6e87f11f-f820-4420-9f4e-73b77f04598f"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("8a50f623-e511-4d5d-aaa9-e13664b6bfe1"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("ab64dc99-dcb0-44bf-96de-807c77ce3430"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("c527d5da-5ff5-4357-b41a-39020a5dee03"));
        }
    }
}
