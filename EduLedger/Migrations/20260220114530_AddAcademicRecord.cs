using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduLedger.Migrations
{
    /// <inheritdoc />
    public partial class AddAcademicRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcademicRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Session = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Term = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CA1 = table.Column<double>(type: "float", nullable: false),
                    CA2 = table.Column<double>(type: "float", nullable: false),
                    Exam = table.Column<double>(type: "float", nullable: false),
                    TotalScore = table.Column<double>(type: "float", nullable: false),
                    Grade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicRecords_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AcademicRecords_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicRecords_CourseId",
                table: "AcademicRecords",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicRecords_StudentId_CourseId_Session_Term",
                table: "AcademicRecords",
                columns: new[] { "StudentId", "CourseId", "Session", "Term" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcademicRecords");
        }
    }
}
