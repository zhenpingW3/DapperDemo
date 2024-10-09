using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DapperModelGenerator.TodoDemo
{
  public enum DatabaseType
  {
    SqlServer,  //SQLServer数据库
    MySql,      //Mysql数据库
    PostgreSQL,     //PostgreSQL数据库
    Oracle,     //Oracle数据库
    Sqlite,     //SQLite数据库
    DB2         //IBM DB2数据库
  }

  public class ConnectionFactory
  {
    private static DatabaseType GetDataBaseType(string databaseType)
    {
      DatabaseType returnValue = DatabaseType.SqlServer;
      foreach (DatabaseType dbType in Enum.GetValues(typeof(DatabaseType)))
      {
        if (dbType.ToString().Equals(databaseType, StringComparison.OrdinalIgnoreCase))
        {
          returnValue = dbType;
          break;
        }
      }
      return returnValue;
    }


    public static IDbConnection CreateConnection(DatabaseType dbType, string database, string strConn)
    {
      IDbConnection connection = null;

      switch (dbType)
      {
        case DatabaseType.SqlServer:
          connection = new System.Data.SqlClient.SqlConnection(strConn);
          break;
        case DatabaseType.MySql:
          connection = new MySql.Data.MySqlClient.MySqlConnection(strConn);
          break;
        case DatabaseType.PostgreSQL:
          connection = new Npgsql.NpgsqlConnection(strConn);
          break;
        case DatabaseType.Sqlite:
          connection = new System.Data.SQLite.SQLiteConnection(strConn);
          break;
        case DatabaseType.Oracle:
          connection = new Oracle.ManagedDataAccess.Client.OracleConnection(strConn);

          //connection = new System.Data.OracleClient.OracleConnection(strConn);
          break;
        case DatabaseType.DB2:
          connection = new System.Data.OleDb.OleDbConnection(strConn);
          break;
      }

      return connection;
    }
  }
}
