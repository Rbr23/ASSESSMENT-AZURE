using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Amigos.Migrations
{
    public partial class InitDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Amigos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UrlFoto = table.Column<string>(nullable: true),
                    Nome = table.Column<string>(nullable: true),
                    SobreNome = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Telefone = table.Column<string>(nullable: true),
                    DataAniversario = table.Column<DateTime>(nullable: false),
                    Id_PaisOrigiem = table.Column<Guid>(nullable: false),
                    Id_EstadoOrigem = table.Column<Guid>(nullable: false),
                    AmigosId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amigos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Amigos_Amigos_AmigosId",
                        column: x => x.AmigosId,
                        principalTable: "Amigos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Amigos_AmigosId",
                table: "Amigos",
                column: "AmigosId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Amigos");
        }
    }
}
