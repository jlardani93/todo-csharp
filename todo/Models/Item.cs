using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace TodoProject.Models
{
  public class Item
  {
    private string _description;
    private string _dueDate;
    private int _id;

    public Item(string description, string dueDate)
    {
      _description = description;
      _dueDate = dueDate;
    }

    public void SetId(int id)
    {
      _id = id;
    }

    public string GetDescription()
    {
      return _description;
    }

    public string GetDueDate()
    {
      return _dueDate;
    }

    public int GetId()
    {
      return _id;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO items (description, due_date) VALUES (@thisDescription, @thisDueDate);";

      MySqlParameter description = new MySqlParameter();
      description.ParameterName = "@thisDescription";
      description.Value = this._description;
      cmd.Parameters.Add(description);

      MySqlParameter dueDate = new MySqlParameter();
      dueDate.ParameterName = "@thisDueDate";
      dueDate.Value = this._dueDate;
      cmd.Parameters.Add(dueDate);

      cmd.ExecuteNonQuery();
      this._id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Item Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"Select * FROM items WHERE id=@thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = id;
      cmd.Parameters.Add(thisId);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      int itemId = 0;
      string itemDueDate = " ";
      string itemDescription = " ";

      while (rdr.Read())
      {
        itemId = rdr.GetInt32(0);
        itemDueDate = rdr.GetString(2);
        itemDescription = rdr.GetString(1);
      }

      Item myItem = new Item(itemDescription, itemDueDate);
      myItem.SetId(itemId);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return myItem;
    }

    public static List<Item> GetAll()
    {
      List<Item> allItems = new List<Item>();
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM items;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while (rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string description = rdr.GetString(1);
        string dueDate = rdr.GetString(2);
        allItems.Add(new Item(description, dueDate));
        allItems[allItems.Count-1].SetId(id);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allItems;
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText= "DELETE FROM items WHERE id=@itemId;";

      MySqlParameter itemId = new MySqlParameter();
      itemId.ParameterName = "@itemId";
      itemId.Value = _id;
      cmd.Parameters.Add(itemId);

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM items;";

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public override bool Equals(System.Object otherItem)
    {
      if (!(otherItem is Item))
      {
        return false;
      }
      else
      {
        Item newItem = (Item) otherItem;
        bool descriptionEquality = (this.GetDescription() == newItem.GetDescription());
        return descriptionEquality;
      }
    }
  }
}
