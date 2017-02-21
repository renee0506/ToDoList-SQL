using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace ToDoList
{
  public class Task
  {
    private int _id;
    private string _description;
    private int _categoryId;
    private string _deadline;

    public Task (string Description, int CategoryId, string Deadline, int Id = 0)
    {
      _id= Id;
      _description = Description;
      _categoryId = CategoryId;
      _deadline = Deadline;
    }

    public override bool Equals(System.Object otherTask)
    {
      if (!(otherTask is Task))
      {
        return false;
      }
      else
      {
        Task newTask = (Task) otherTask;
        bool idEquality = (this.GetId()== newTask.GetId());
        bool descriptionEquality = (this.GetDescription() == newTask.GetDescription());
        bool categoryEquality = this.GetCategoryId() == newTask.GetCategoryId();
        bool deadlineEquality = this.GetDeadline() == newTask.GetDeadline();
        return (idEquality && descriptionEquality && categoryEquality && deadlineEquality);

      }
    }
    public int GetId()
    {
      return _id;
    }
    public string GetDescription()
    {
      return _description;
    }
    public string GetDeadline()
    {
      return _deadline;
    }
    public void SetDeadline(string newDeadline)
    {
      _deadline = newDeadline;
    }

    public void SetDescription(string newDescription)
    {
      _description = newDescription;
    }

    public int GetCategoryId()
    {
        return _categoryId;
    }
    public void SetCategoryId(int newCategoryId)
    {
        _categoryId = newCategoryId;
    }
    public static List<Task> SortDeadlines()
    {
        List<Task> sortedTasks = new List<Task>{};
        SqlConnection conn = DB.Connection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("SELECT * FROM tasks ORDER BY deadline;", conn);
        SqlDataReader rdr = cmd.ExecuteReader();

        while (rdr.Read())
        {
            int taskId = rdr.GetInt32(0);
            string taskDescription = rdr.GetString(1);
            int taskCategoryId = rdr.GetInt32(2);
            string taskDeadline = rdr.GetDateTime(3).ToString("MM-dd-yyyy");
            Task newTask = new Task(taskDescription, taskCategoryId, taskDeadline, taskId);
            sortedTasks.Add(newTask);
        }

        if (rdr != null)
        {
          rdr.Close();
        }
        if (conn != null)
        {
          conn.Close();
        }


        return sortedTasks;
    }

    public static List<Task> GetAll()
    {
      List<Task> allTasks = new  List<Task>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM tasks;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        int taskId = rdr.GetInt32(0);
        string taskDescription = rdr.GetString(1);
        int taskCategoryId = rdr.GetInt32(2);
        string taskDeadline = rdr.GetDateTime(3).ToString("MM-dd-yyyy");
        Task newTask = new Task(taskDescription, taskCategoryId, taskDeadline, taskId);
        allTasks.Add(newTask);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allTasks;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO tasks (description, category_id, deadline) OUTPUT INSERTED.id VALUES (@TaskDescription, @TaskCategoryId, @TaskDeadline);", conn);

      SqlParameter descriptionParameter = new SqlParameter();
      descriptionParameter.ParameterName = "@TaskDescription";
      descriptionParameter.Value = this.GetDescription();

      SqlParameter categoryIdParameter = new SqlParameter();
      categoryIdParameter.ParameterName = "@TaskCategoryId";
      categoryIdParameter.Value = this.GetCategoryId();

      SqlParameter deadlineParameter = new SqlParameter();
      deadlineParameter.ParameterName = "@TaskDeadline";
      deadlineParameter.Value = this.GetDeadline();

      cmd.Parameters.Add(descriptionParameter);
      cmd.Parameters.Add(categoryIdParameter);
      cmd.Parameters.Add(deadlineParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM tasks;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static Task Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM tasks WHERE id = @TaskId;", conn);
      SqlParameter taskIdParameter = new SqlParameter();
      taskIdParameter.ParameterName = "@TaskId";
      taskIdParameter.Value = id.ToString();
      cmd.Parameters.Add(taskIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundTaskId = 0;
      string foundTaskDescription = null;
      int foundTaskCategoryId = 0;
      string foundTaskDeadline = null;
      while(rdr.Read())
      {
        foundTaskId = rdr.GetInt32(0);
        foundTaskDescription = rdr.GetString(1);
        foundTaskCategoryId = rdr.GetInt32(2);
        foundTaskDeadline = rdr.GetDateTime(3).ToString("MM-dd-yyyy");
      }
      Task foundTask = new Task(foundTaskDescription, foundTaskCategoryId, foundTaskDeadline, foundTaskId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return foundTask;
    }
  }
}
