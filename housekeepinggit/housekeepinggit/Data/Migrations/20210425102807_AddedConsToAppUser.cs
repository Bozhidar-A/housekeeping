using Microsoft.EntityFrameworkCore.Migrations;

namespace housekeepinggit.Data.Migrations
{
    public partial class AddedConsToAppUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "creatorId",
                table: "Task",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "houseKeeperId",
                table: "Task",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "creatorId",
                table: "Location",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Task_creatorId",
                table: "Task",
                column: "creatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Task_houseKeeperId",
                table: "Task",
                column: "houseKeeperId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_creatorId",
                table: "Location",
                column: "creatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_AspNetUsers_creatorId",
                table: "Location",
                column: "creatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Task_AspNetUsers_creatorId",
                table: "Task",
                column: "creatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Task_AspNetUsers_houseKeeperId",
                table: "Task",
                column: "houseKeeperId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_AspNetUsers_creatorId",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_AspNetUsers_creatorId",
                table: "Task");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_AspNetUsers_houseKeeperId",
                table: "Task");

            migrationBuilder.DropIndex(
                name: "IX_Task_creatorId",
                table: "Task");

            migrationBuilder.DropIndex(
                name: "IX_Task_houseKeeperId",
                table: "Task");

            migrationBuilder.DropIndex(
                name: "IX_Location_creatorId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "creatorId",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "houseKeeperId",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "creatorId",
                table: "Location");
        }
    }
}
