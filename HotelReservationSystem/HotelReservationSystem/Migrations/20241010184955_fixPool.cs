using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelReservationSystem.Migrations
{
    public partial class fixPool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PoolId",
                table: "ReservePools",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ReservePools_PoolId",
                table: "ReservePools",
                column: "PoolId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservePools_Pools_PoolId",
                table: "ReservePools",
                column: "PoolId",
                principalTable: "Pools",
                principalColumn: "PoolId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservePools_Pools_PoolId",
                table: "ReservePools");

            migrationBuilder.DropIndex(
                name: "IX_ReservePools_PoolId",
                table: "ReservePools");

            migrationBuilder.DropColumn(
                name: "PoolId",
                table: "ReservePools");
        }
    }
}
