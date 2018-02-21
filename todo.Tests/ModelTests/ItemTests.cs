using Microsoft.VisualStudio.TestTools.UnitTesting;
using TodoProject.Models;
using System;
using System.Collections.Generic;

namespace TodoProject.Tests
{

    [TestClass]
    public class ItemTests : IDisposable
    {
        public void Dispose()
        {
            Item.DeleteAll();
        }
        public ItemTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=todo_test;";
        }

        [TestMethod]
        public void GetAll_DatabaseEmptyAtFirst_0()
        {
          int result = Item.GetAll().Count;

          Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Save_SavesToDatabase_ItemList()
        {
          Item testItem = new Item("Mow the Lawn", "2018-02-01");

          testItem.Save();
          List<Item> result = Item.GetAll();
          List<Item> testList = new List<Item>{testItem};

          CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Save_AssignsIdToObject_Id()
        {
          Item testItem = new Item("Mow the Lawn", "2017-02-01");

          testItem.Save();
          Item savedItem = Item.GetAll()[0];

          int result = savedItem.GetId();
          int testId = testItem.GetId();

          Assert.AreEqual(testId, result);
        }
    }
}
