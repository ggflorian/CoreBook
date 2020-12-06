﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreBook.Data.Migrations
{
    public partial class AddStoredProcForCoverType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROC usp_GetCoverTypes 
                                    AS 
                                    BEGIN 
                                     SELECT * FROM  dbo.CoverTypes 
                                    END");

            migrationBuilder.Sql(@"CREATE PROC usp_GetCoverType 
                                    @Id int 
                                    AS 
                                    BEGIN 
                                     SELECT * FROM  dbo.CoverTypes  WHERE  (Id = @Id) 
                                    END ");

            migrationBuilder.Sql(@"CREATE PROC usp_UpdateCoverType
	                                @Id int,
	                                @Name varchar(32)
                                    AS 
                                    BEGIN 
                                     UPDATE dbo.CoverTypes
                                     SET  Name = @Name
                                     WHERE ID = @Id
                                    END");

            migrationBuilder.Sql(@"CREATE PROC usp_DeleteCoverType
	                                @Id int
                                    AS 
                                    BEGIN 
                                     DELETE FROM dbo.CoverTypes
                                     WHERE  ID = @Id
                                    END");

            migrationBuilder.Sql(@"CREATE PROC usp_CreateCoverType
                                   @Name varchar(32)
                                   AS 
                                   BEGIN 
                                    INSERT INTO dbo.CoverTypes(Name)
                                    VALUES (@Name)
                                   END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE usp_GetCoverTypes");
            migrationBuilder.Sql(@"DROP PROCEDURE usp_GetCoverType");
            migrationBuilder.Sql(@"DROP PROCEDURE usp_UpdateCoverType");
            migrationBuilder.Sql(@"DROP PROCEDURE usp_DeleteCoverType");
            migrationBuilder.Sql(@"DROP PROCEDURE usp_CreateCoverType");
        }
    }
}
