using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightManager.Migrations
{
    /// <inheritdoc />
    public partial class ticket3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationTicket");

            migrationBuilder.AddColumn<int>(
                name: "ReservationId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TicketId",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ReservationId",
                table: "Tickets",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_TicketId",
                table: "Reservations",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Tickets_TicketId",
                table: "Reservations",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Reservations_ReservationId",
                table: "Tickets",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Tickets_TicketId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Reservations_ReservationId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ReservationId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_TicketId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "Reservations");

            migrationBuilder.CreateTable(
                name: "ReservationTicket",
                columns: table => new
                {
                    ReservationsId = table.Column<int>(type: "int", nullable: false),
                    TicketsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationTicket", x => new { x.ReservationsId, x.TicketsId });
                    table.ForeignKey(
                        name: "FK_ReservationTicket_Reservations_ReservationsId",
                        column: x => x.ReservationsId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationTicket_Tickets_TicketsId",
                        column: x => x.TicketsId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationTicket_TicketsId",
                table: "ReservationTicket",
                column: "TicketsId");
        }
    }
}
