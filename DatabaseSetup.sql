USE master
CREATE DATABASE GetStoredProcCommand_TVP_Repro
GO
USE GetStoredProcCommand_TVP_Repro
GO
CREATE TABLE [dbo].[ReproTable] (
	[IntValue] int,
	[StrValue] nvarchar(50),
	[DateValue] datetimeoffset
)
GO
CREATE TYPE [dbo].[ReproTableType] AS TABLE (
	[IntValue] int,
	[StrValue] nvarchar(50),
	[DateValue] datetimeoffset
)
GO
CREATE PROCEDURE [dbo].[usp_ReproTable_PostTable]  
 @DataTable [dbo].[ReproTableType] READONLY  
AS  
BEGIN  
 SET NOCOUNT ON;  
 INSERT INTO [dbo].[ReproTable]  
   (  
    [IntValue],  
    [StrValue],  
    [DateValue]  
   )  
  SELECT  
    [IntValue],  
    [StrValue],  
    [DateValue]  
   FROM  
    @DataTable;  
END  
GO