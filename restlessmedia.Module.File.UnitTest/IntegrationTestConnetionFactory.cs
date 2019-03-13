using SqlBuilder.DataServices;
using System.Data;
using System.Data.SqlClient;

namespace restlessmedia.Module.File.UnitTest
{
  public class IntegrationTestConnetionFactory : IConnectionFactory
  {
    public IDbConnection CreateConnection(bool open = false)
    {
      IDbConnection connection = new SqlConnection(_connectionString);

      if (open)
      {
        connection.Open();
      }

      return connection;
    }

    public IDbTransaction CreateTransaction(IDbConnection connection)
    {
      return connection.BeginTransaction();
    }

    public IDbTransaction CreateTransaction(bool open = false)
    {
      return CreateTransaction(CreateConnection(open));
    }

    private const string _connectionString = @"Server=.\SQLEXPRESS;database=restlessmedia.UnitTest;Trusted_Connection=Yes;APP=PC";
  }
}
