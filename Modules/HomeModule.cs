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
      Get["/"] = _ => View["index.cshtml"];
      Post["/new_task"] = _ => {
          string description = Request.Form["description"];
          string deadline = Request.Form["deadline"];
          return View["new-task.cshtml"];
      };
    }
  }
}
