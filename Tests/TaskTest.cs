using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ToDoList
{
  public class ToDoTest : IDisposable
  {
    public ToDoTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=todo_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Task.DeleteAll();
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Task.GetAll().Count;
      Assert.Equal(0, result);
    }


    [Fact]
    public void  Test_Save_SavesToDatabase()
    {
     //Arrange
     Task testTask = new Task("Mow the lawn", 1, "01-10-1990");

      //Act
      testTask.Save();
      List<Task> result = Task.GetAll();
      List<Task> testList = new  List<Task>{testTask};

      //Assert
      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn", 1, "01-10-1990");
      //Act
      testTask.Save();
      Task savedTask = Task.GetAll()[0];

      int result = savedTask.GetId();
      int testId = testTask.GetId();

      //Assert
      Assert.Equal(testId,result);
    }

    [Fact]
    public void Test_Find_FindsTaskInDatabase()
    {
      Task testTask = new Task("Mow the lawn", 1, "01-10-1990");
      testTask.Save();

      Task foundTask = Task.Find(testTask.GetId());

      Assert.Equal(testTask, foundTask);
    }
    [Fact]
    public void Test_EqualOverrideTrueForSameDescription()
    {
        Task firstTask = new Task("Mow the lawn", 1, "01-10-1990");
        Task secondTask = new Task("Mow the lawn", 1, "01-10-1990");

        Assert.Equal(firstTask, secondTask);
    }



  }
}
