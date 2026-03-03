using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduLedger.Migrations
{
    /// <inheritdoc />
    public partial class FixCourseUserRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCourses_AspNetUsers_UsersId",
                table: "UserCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCourses_Courses_CoursesId",
                table: "UserCourses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCourses",
                table: "UserCourses");

            migrationBuilder.RenameTable(
                name: "UserCourses",
                newName: "StudentCourses");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "StudentCourses",
                newName: "StudentsId");

            migrationBuilder.RenameColumn(
                name: "CoursesId",
                table: "StudentCourses",
                newName: "EnrolledCoursesId");

            migrationBuilder.RenameIndex(
                name: "IX_UserCourses_UsersId",
                table: "StudentCourses",
                newName: "IX_StudentCourses_StudentsId");

            migrationBuilder.AddColumn<string>(
                name: "InstructorId",
                table: "Courses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentCourses",
                table: "StudentCourses",
                columns: new[] { "EnrolledCoursesId", "StudentsId" });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_InstructorId",
                table: "Courses",
                column: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_AspNetUsers_InstructorId",
                table: "Courses",
                column: "InstructorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourses_AspNetUsers_StudentsId",
                table: "StudentCourses",
                column: "StudentsId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourses_Courses_EnrolledCoursesId",
                table: "StudentCourses",
                column: "EnrolledCoursesId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_AspNetUsers_InstructorId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourses_AspNetUsers_StudentsId",
                table: "StudentCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourses_Courses_EnrolledCoursesId",
                table: "StudentCourses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_InstructorId",
                table: "Courses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentCourses",
                table: "StudentCourses");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "Courses");

            migrationBuilder.RenameTable(
                name: "StudentCourses",
                newName: "UserCourses");

            migrationBuilder.RenameColumn(
                name: "StudentsId",
                table: "UserCourses",
                newName: "UsersId");

            migrationBuilder.RenameColumn(
                name: "EnrolledCoursesId",
                table: "UserCourses",
                newName: "CoursesId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentCourses_StudentsId",
                table: "UserCourses",
                newName: "IX_UserCourses_UsersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCourses",
                table: "UserCourses",
                columns: new[] { "CoursesId", "UsersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserCourses_AspNetUsers_UsersId",
                table: "UserCourses",
                column: "UsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCourses_Courses_CoursesId",
                table: "UserCourses",
                column: "CoursesId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
