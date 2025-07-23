using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmpManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddDateJoinedField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DateJoined",
                table: "Employees",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateJoined",
                table: "Employees");
        }
    }
}
