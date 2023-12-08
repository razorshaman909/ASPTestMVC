using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcMovie.Migrations
{
    /// <inheritdoc />
    public partial class Rental : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rental_Movie_MovieID",
                table: "Rental");

            migrationBuilder.DropForeignKey(
                name: "FK_Rental_User_UserID",
                table: "Rental");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rental",
                table: "Rental");

            migrationBuilder.RenameTable(
                name: "Rental",
                newName: "Rentals");

            migrationBuilder.RenameColumn(
                name: "MovieID",
                table: "Rentals",
                newName: "MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_Rental_UserID",
                table: "Rentals",
                newName: "IX_Rentals_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Rental_MovieID",
                table: "Rentals",
                newName: "IX_Rentals_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rentals",
                table: "Rentals",
                column: "RentalID");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Movie_MovieId",
                table: "Rentals",
                column: "MovieId",
                principalTable: "Movie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_User_UserID",
                table: "Rentals",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Movie_MovieId",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_User_UserID",
                table: "Rentals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rentals",
                table: "Rentals");

            migrationBuilder.RenameTable(
                name: "Rentals",
                newName: "Rental");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "Rental",
                newName: "MovieID");

            migrationBuilder.RenameIndex(
                name: "IX_Rentals_UserID",
                table: "Rental",
                newName: "IX_Rental_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Rentals_MovieId",
                table: "Rental",
                newName: "IX_Rental_MovieID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rental",
                table: "Rental",
                column: "RentalID");

            migrationBuilder.AddForeignKey(
                name: "FK_Rental_Movie_MovieID",
                table: "Rental",
                column: "MovieID",
                principalTable: "Movie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rental_User_UserID",
                table: "Rental",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
