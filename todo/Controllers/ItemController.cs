using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using TodoProject.Models;

namespace TodoListProject.Controllers
{
  public class ItemController : Controller
  {
    [HttpGet("Item/Form")]
    public ActionResult Form()
    {
      return View();
    }

    [HttpPost("Item/Create")]
    public ActionResult Create()
    {
      string myDescription = Request.Form["itemDescription"];
      string myDueDate = Request.Form["itemDueDate"];
      Item myItem = new Item(myDescription, myDueDate);
      myItem.Save();
      return View("Info", myItem);
    }

    [HttpGet("Item/Info/{id}")]
    public ActionResult Info(int id)
    {
      Item myItem = Item.Find(id);
      return View(myItem);
    }

    [HttpGet("Item/Delete/{id}")]
    public ActionResult Delete(int id)
    {
      Item myItem = Item.Find(id);
      myItem.Delete();
      return RedirectToAction("Index", "Home");
    }
  }
}
