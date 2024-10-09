using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;


namespace DapperModelGenerator.ModelGenerator
{
  public class DapperModelGenerator
  {
    private readonly string _connectionString;

    public DapperModelGenerator(string connectionString)
    {
      _connectionString = connectionString;
    }

    public void GenerateDapperModel(string tableName, string outputFilePath)
    {
      using (var connection = new SqlConnection(_connectionString))
      {
        connection.Open();

        // 查询列信息以及主键信息
        string query = $@"
                SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, 
                COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsPrimaryKey') AS IsPrimaryKey,
                COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') AS IsIdentity
                FROM INFORMATION_SCHEMA.COLUMNS 
                WHERE TABLE_NAME = '{tableName}'";

        using (var command = new SqlCommand(query, connection))
        using (var reader = command.ExecuteReader())
        {
          var classCode = new StringBuilder();
          classCode.AppendLine("using Dapper.Contrib.Extensions;");
          classCode.AppendLine();
          classCode.AppendLine($"[Table(\"{tableName}\")]");
          classCode.AppendLine($"public class {tableName}");
          classCode.AppendLine("{");

          while (reader.Read())
          {
            string columnName = reader["COLUMN_NAME"].ToString();
            string dataType = reader["DATA_TYPE"].ToString();
            bool isNullable = reader["IS_NULLABLE"].ToString() == "YES";
            bool isPrimaryKey = reader["IsPrimaryKey"] != DBNull.Value ? Convert.ToBoolean(reader["IsPrimaryKey"]) : false;
            bool isIdentity = Convert.ToBoolean(reader["IsIdentity"]);

            // 如果是主键属性且不是自增长，添加 [Key] 特性
            if (isPrimaryKey && !isIdentity)
            {
              classCode.AppendLine($"    [Key]");
            }

            string csharpType = SqlTypeToCSharpType(dataType, isNullable);
            classCode.AppendLine($"    public {csharpType} {columnName} {{ get; set; }}");
          }

          classCode.AppendLine("}");

          // Write the generated class to file
          File.WriteAllText(outputFilePath, classCode.ToString());
        }
      }
    }

    // Map SQL Server types to C# types
    private string SqlTypeToCSharpType(string sqlType, bool isNullable)
    {
      string csharpType;
      switch (sqlType)
      {
        case "int":
          csharpType = "int";
          break;
        case "bigint":
          csharpType = "long";
          break;
        case "smallint":
          csharpType = "short";
          break;
        case "tinyint":
          csharpType = "byte";
          break;
        case "bit":
          csharpType = "bool";
          break;
        case "decimal":
        case "numeric":
          csharpType = "decimal";
          break;
        case "float":
          csharpType = "double";
          break;
        case "real":
          csharpType = "float";
          break;
        case "money":
        case "smallmoney":
          csharpType = "decimal";
          break;
        case "char":
        case "varchar":
        case "text":
        case "nchar":
        case "nvarchar":
        case "ntext":
          csharpType = "string";
          break;
        case "date":
        case "datetime":
        case "datetime2":
        case "smalldatetime":
          csharpType = "DateTime";
          break;
        case "time":
          csharpType = "TimeSpan";
          break;
        case "uniqueidentifier":
          csharpType = "Guid";
          break;
        default:
          csharpType = "object"; // Handle unrecognized types
          break;
      }

      // If the field is nullable and not a string, make the type nullable
      if (isNullable && csharpType != "string" && csharpType != "object")
      {
        csharpType += "?";
      }

      return csharpType;
    }
  }



}
