using Nancy;
using System;
using System.Collections.Generic;
using ToDoList;

namespace InventoryModule
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
          List<Category> newCategory = Category.GetAll();
          return View["index.cshtml", newCategory];
      };
      Post["/new-category"] = _ => {
          string categoryName = Request.Form["category-name"];
          Category newCategory = new Category(categoryName);
          newCategory.Save();
          List<Category> allCategory = Category.GetAll();
          return View["index.cshtml", allCategory];
      };
      Get["/{id}/create-task"] = parameters => {
          Category selected =  Category.Find(parameters.id);
          return View["create_task.cshtml", selected];
      };
      Post["/{id}/new-task"] = parameters => {
          string description = Request.Form["description"];
          string deadline = Request.Form["deadline"];
          Task newTask = new Task(description, parameters.id, deadline);
          newTask.Save();
          return View["/new-task.cshtml", newTask];
      };
      Get["/sorted-tasks"] = _ => {
          List<Task> sortedTasks = Task.SortDeadlines();
          Console.WriteLine(sortedTasks[0].GetDescription());
          return View["sorted-tasks.cshtml", sortedTasks];
      };
    }
  }
}
