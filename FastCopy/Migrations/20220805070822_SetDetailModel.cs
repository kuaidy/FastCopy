using Microsoft.EntityFrameworkCore.Migrations;

namespace FastCopy.Migrations
{
    public partial class SetDetailModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            string createSql = string.Format("create table if not exists DetailSetModel (Id INTEGER PRIMARY KEY,EName TEXT,CName TEXT,Value TEXT,Type TEXT)");
            migrationBuilder.Sql(createSql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CName",
                table: "DetailSetModel");

            migrationBuilder.RenameColumn(
                name: "EName",
                table: "DetailSetModel",
                newName: "Name");
        }
    }
}
