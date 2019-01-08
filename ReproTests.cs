using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GetStoredProcCommand_TVP_Repro
{
    [TestClass]
    public class ReproTests
    {
        [TestMethod]
        public void TableInsertFails_TypeName()
        {
            var sqlConnection = new SqlConnection("Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=GetStoredProcCommand_TVP_Repro;Integrated Security=true");
            try
            {
                sqlConnection.Open();
                
                var dataTable = CreateNewDataTable();
                var commandBuilder = new SqlCommandBuilder();

                SqlCommand command = new SqlCommand("usp_ReproTable_PostTable", sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                SqlCommandBuilder.DeriveParameters(command);

                foreach (SqlParameter parameter in command.Parameters)
                {
                    if (parameter.SqlDbType == SqlDbType.Structured && parameter.TypeName != null)
                    {
                        parameter.Value = dataTable;
                        break;
                    }
                }
                command.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        [TestMethod]
        public void TableInsert_Succeeeds_WithWorkaround()
        {
            var sqlConnection = new SqlConnection("Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=GetStoredProcCommand_TVP_Repro;Integrated Security=true");
            try
            {
                sqlConnection.Open();
                var dataTable = CreateNewDataTable();
                var commandBuilder = new SqlCommandBuilder();

                SqlCommand command = new SqlCommand("usp_ReproTable_PostTable", sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                SqlCommandBuilder.DeriveParameters(command);

                foreach(SqlParameter parameter in command.Parameters)
                {
                    if (parameter.SqlDbType == SqlDbType.Structured && parameter.TypeName != null)
                    {
                        parameter.Value = dataTable;
                        parameter.TypeName = parameter.TypeName.Substring(parameter.TypeName.IndexOf('.'));
                        break;
                    }
                }
                command.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private DataTable CreateNewDataTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add();
            dataTable.Columns.Add();
            dataTable.Columns.Add();

            dataTable.Rows.Add(1, "string1", new DateTimeOffset(2019, 1, 1, 0, 0, 0, new TimeSpan(0, 0, 0)));
            dataTable.Rows.Add(2, "string2", new DateTimeOffset(2019, 1, 2, 0, 0, 0, new TimeSpan(0, 0, 0)));
            dataTable.Rows.Add(3, "string3", new DateTimeOffset(2019, 1, 3, 0, 0, 0, new TimeSpan(0, 0, 0)));
            return dataTable;
        }
    }
}
