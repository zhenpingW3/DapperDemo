using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperModelGenerator.TodoDemo
{
  using System.Collections.Generic;
  using System.Data;
  using System.Data.SqlClient; // 或者其他数据库的命名空间
  using Dapper;
  using Dapper.Contrib.Extensions;

  public class TodoDAL
  {
    private readonly string _connectionString;

    public TodoDAL(string connectionString)
    {
      _connectionString = connectionString;
    }

    // 获取所有 Todo
    public IEnumerable<Todo> GetAll()
    {
      using (IDbConnection dbConnection = new SqlConnection(_connectionString))
      {
        dbConnection.Open();
        return dbConnection.GetAll<Todo>();
      }
    }

    // 根据 Id 获取 Todo
    public Todo GetById(int id)
    {
      using (IDbConnection dbConnection = new SqlConnection(_connectionString))
      {
        dbConnection.Open();
        return dbConnection.Get<Todo>(id);
      }
    }

    // 插入新的 Todo
    public void Insert(Todo todo)
    {
      using (IDbConnection dbConnection = new SqlConnection(_connectionString))
      {
        dbConnection.Open();
        dbConnection.Insert(todo);
      }
    }

    // 批量插入 Todo
    public void InsertBatch(IEnumerable<Todo> todos)
    {
      using (IDbConnection dbConnection = new SqlConnection(_connectionString))
      {
        dbConnection.Open();
        foreach (var todo in todos)
        {
          todo.Id = GenerateNewId(dbConnection); // 为每个 todo 生成新 Id
          dbConnection.Insert(todo);
        }
      }
    }

    // 更新 Todo
    public void Update(Todo todo)
    {
      using (IDbConnection dbConnection = new SqlConnection(_connectionString))
      {
        dbConnection.Open();
        dbConnection.Update(todo);
      }
    }

    // 删除 Todo
    public void Delete(int id)
    {
      using (IDbConnection dbConnection = new SqlConnection(_connectionString))
      {
        dbConnection.Open();
        var todo = GetById(id); // 根据 Id 获取 Todo
        if (todo != null)
        {
          dbConnection.Delete(todo);
        }
      }
    }

    // 批量删除 Todo
    public void DeleteBatch(IEnumerable<int> ids)
    {
      using (IDbConnection dbConnection = new SqlConnection(_connectionString))
      {
        dbConnection.Open();
        foreach (var id in ids)
        {
          Delete(id);
        }
      }
    }

    // 生成新的 Id
    private int GenerateNewId(IDbConnection dbConnection)
    {
      // 这里假设 Id 是整数类型，查询当前最大 Id 并加一
      var maxId = dbConnection.QuerySingleOrDefault<int?>("SELECT MAX(Id) FROM Todo");
      return (maxId ?? 0) + 1; // 如果没有记录则从 1 开始
    }
  }
}
