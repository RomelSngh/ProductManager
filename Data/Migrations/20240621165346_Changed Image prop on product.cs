using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductManagement.Data.Migrations
{
    public partial class ChangedImageproponproduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<byte[]>(
            //    name: "Image",
            //    table: "Products",
            //    type: "varbinary(max)",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)");
        }
    }
}
