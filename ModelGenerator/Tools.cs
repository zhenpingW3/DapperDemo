using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperModelGenerator.ModelGenerator
{
  public class Tools
  {
    public static void BuildDapperModelClassFile(string tableName = "CECLPrepayHistRegressResultList", string classFileName = "Cls{0}.cs")
    {
      string connectionString = Properties.Settings.Default.DefaultDatabase;
      var generator = new DapperModelGenerator(connectionString);

      // 创建output文件夹（如果不存在
      string outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "output");
      Directory.CreateDirectory(outputDirectory);

      // 生成表名为 "YourTableName" 的 Dapper 实体类，并保存到指定文件
      generator.GenerateDapperModel(tableName, System.IO.Path.Combine(outputDirectory, string.Format(classFileName, tableName)));

      Console.WriteLine("Model generated successfully!");
    }
  }
}
