using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperModelGenerator.TodoDemo
{
  using Dapper.Contrib.Extensions;

  [Table("Todo")]
  public class Todo
  {
    [ExplicitKey] // 非自增长主键
    public int Id { get; set; }

    public string Title { get; set; }

    public bool Completed { get; set; }

    public string Description { get; set; }

    public string Creator { get; set; }

    public DateTime CreateTime { get; set; }
  }

}
