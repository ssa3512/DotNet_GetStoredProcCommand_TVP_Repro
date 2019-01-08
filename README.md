# .NET SQL Client issue
## Summary
There appears to be a bug in System.Data.SqlClient.SqlCommand that causes execution of stored procedures with a table valued parameter to fail when the type associated with the TVP is retrieved using DeriveParameters.

This was originally discovered using EnterpriseLibrary's GetStoredProcCommand, however the bug seems to still exist in the current .NET code.

## Repro Steps

 - Clone this repository
 - Using sql localdb or an existing sql server run the contents of [DatabaseSetup.sql](DatabaseSetup.sql) - this creates the database, table, SQL type and stored procedure for inserts used by these tests
 - Load up the solution file and run unit tests

 ## Results

 The two unit tests included are identical in all aspects except for one - in the unit test that completes successfully, the type name in the derived parameter is replaced by the type name minus the database name:
 ```
 GetStoredProcCommand_TVP_Repro.dbo.ReproTableType
 ```
 becomes
 ```
 dbo.ReproTableType
 ```
 The method that does not alter the parameter type name fails with the exception
 ```
 System.Data.SqlClient.SqlException: The incoming tabular data stream (TDS) remote procedure call (RPC) protocol stream is incorrect. Table-valued parameter 1 ("@DataTable"), row 0, column 0: Data type 0xF3 (user-defined table type) has a non-zero length database name specified.  Database name is not allowed with a table-valued parameter, only schema name and type name are valid.
 ```

This behavior is also noted in [this stack overflow link](https://stackoverflow.com/questions/9921121/unable-to-access-table-variable-in-stored-procedure) from 2012. The top solution poster notes there is a bug in DeriveParameters referencing a comment on the MSDN article, but that comment does not appear to exist anymore.

## Resolution

I propose the code that derives the type name

https://github.com/dotnet/corefx/blob/875c9842c94fdf3885a7cd28497d492bdd584543/src/System.Data.SqlClient/src/System/Data/SqlClient/SqlCommand.cs#L2212-L2214

is changed to remove the database name from the parameter type name, allowing a user to use DeriveParameters to get the correct parameters needed to execute the stored procedure without error.