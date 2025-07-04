﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoListMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddIsHiddenToTodoItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "TodoItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "TodoItems");
        }
    }
}
