# dotnet issue

There appears to be a bug in System.Data.SqlClient.SqlCommand.DeriveParameters() that causes execution of stored procedures with a table valued parameter to fail when it is retrieved using 

https://github.com/dotnet/corefx/blob/875c9842c94fdf3885a7cd28497d492bdd584543/src/System.Data.SqlClient/src/System/Data/SqlClient/SqlCommand.cs#L2212-L2214