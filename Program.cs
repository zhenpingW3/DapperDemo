using DapperModelGenerator.TodoDemo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperModelGenerator
{
  internal class Program
  {
    static void Main(string[] args)
    {
      string connectionString = Properties.Settings.Default.DefaultDatabase;

      TodoDAL todoDAL = new TodoDAL(connectionString);

      // 批量插入新的 Todo
      var newTodos = new List<Todo>
        {
            new Todo { Title = "Learn Dapper", Completed = false, Description = "Study Dapper for database operations.", Creator = "Alice", CreateTime = DateTime.Now },
            new Todo { Title = "Implement DAL", Completed = false, Description = "Create a data access layer for the application.", Creator = "Bob", CreateTime = DateTime.Now }
        };

      todoDAL.InsertBatch(newTodos);

      // 获取所有 Todo
      var todos = todoDAL.GetAll();
      foreach (var todo in todos)
      {
        Console.WriteLine($"{todo.Id}: {todo.Title} - Completed: {todo.Completed}");
      }

      // 批量删除 Todo
      var idsToDelete = new List<int> { 1, 2 }; // 假设要删除 Id 为 1 和 2 的 Todo
      todoDAL.DeleteBatch(idsToDelete);
      Console.WriteLine("Todos deleted.");

      Console.ReadLine();
    }
  }
}
