using Microsoft.EntityFrameworkCore.Migrations;

namespace FastCopy.Migrations
{
    public partial class CopyInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string createSql = string.Format("create table if not exists CopyInfo (Id INTEGER PRIMARY KEY,IsChecked INTEGER,SourceAddress TEXT,TargetAddress TEXT,CopyTime TEXT,Status TEXT,Sort INTEGER)");
            migrationBuilder.Sql(createSql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CopyInfo");

            migrationBuilder.DropTable(
                name: "DetailSetModel");

            migrationBuilder.DropTable(
                name: "SetModel");
        }
    }
}
