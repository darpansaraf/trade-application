using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeEngine;
using TradeEngine.Controllers;
using TradeEngine.Entities;

namespace TradeEngine.Tests.Controllers
{
    [TestClass]
    public class TradesControllerTest
    {
        [TestMethod]
        public void TestToCheckWhetherAStockIsSuccessFullyAddedInTheTradingQueue()
        {
            var tradesController = new TradesController();

            var trade = new Trade()
            {
                Name = "GOOGL",
                Type = "BUY",
                Quantity = 50,
                Price = 100
            };
            var submitTradeResponse = tradesController.Post(trade).Result;

            Assert.IsNotNull(submitTradeResponse);
            Assert.IsNull(submitTradeResponse.Error);
            Assert.AreEqual(submitTradeResponse.Status, "Success");
            Assert.IsNotNull(submitTradeResponse.TradeId);
        }

        [TestMethod]
        public void TestToGetTradeExecutionsOnAStockWhenThereAreValidTradeRequests()
        {
            // Arrange
            var tradesController = new TradesController();

            var trade = new Trade()
            {
                Name = "GOOGL",
                Type = "BUY",
                Quantity = 50,
                Price = 100
            };
            var submitTradeResponse = tradesController.Post(trade).Result;
            Assert.IsNotNull(submitTradeResponse);
            Assert.IsNull(submitTradeResponse.Error);
            Assert.AreEqual(submitTradeResponse.Status, "Success");
            Assert.IsNotNull(submitTradeResponse.TradeId);

            trade = new Trade()
            {
                Name = "GOOGL",
                Type = "BUY",
                Quantity = 100,
                Price = 101
            };
            submitTradeResponse = tradesController.Post(trade).Result;
            Assert.IsNotNull(submitTradeResponse);
            Assert.IsNull(submitTradeResponse.Error);
            Assert.AreEqual(submitTradeResponse.Status, "Success");
            Assert.IsNotNull(submitTradeResponse.TradeId);

            trade = new Trade()
            {
                Name = "GOOGL",
                Type = "BUY",
                Quantity = 150,
                Price = 100
            };
            submitTradeResponse = tradesController.Post(trade).Result;
            Assert.IsNotNull(submitTradeResponse);
            Assert.IsNull(submitTradeResponse.Error);
            Assert.AreEqual(submitTradeResponse.Status, "Success");
            Assert.IsNotNull(submitTradeResponse.TradeId);

            trade = new Trade()
            {
                Name = "GOOGL",
                Type = "SELL",
                Quantity = 100,
                Price = 102
            };
            submitTradeResponse = tradesController.Post(trade).Result;
            Assert.IsNotNull(submitTradeResponse);
            Assert.IsNull(submitTradeResponse.Error);
            Assert.AreEqual(submitTradeResponse.Status, "Success");
            Assert.IsNotNull(submitTradeResponse.TradeId);

            trade = new Trade()
            {
                Name = "GOOGL",
                Type = "SELL",
                Quantity = 200,
                Price = 100
            };
            submitTradeResponse = tradesController.Post(trade).Result;
            Assert.IsNotNull(submitTradeResponse);
            Assert.IsNull(submitTradeResponse.Error);
            Assert.AreEqual(submitTradeResponse.Status, "Success");
            Assert.IsNotNull(submitTradeResponse.TradeId);

            var executionResponse = tradesController.Get("GOOGL").Result;

            Assert.IsNotNull(executionResponse);
            Assert.IsNull(executionResponse.Error);
            Assert.AreEqual(submitTradeResponse.Status, "Success");
            Assert.IsNotNull(executionResponse.TradeExecutions);
            Assert.IsTrue(executionResponse.TradeExecutions.Count > 0);
        }

        [TestMethod]
        public void TestToCheckErrorIsReturnedWhenNoStockIsNotFound()
        {
            var tradesController = new TradesController();

            var trade = new Trade()
            {
                Name = "GOOGL",
                Type = "BUY",
                Quantity = 50,
                Price = 100
            };
            var submitTradeResponse = tradesController.Post(trade).Result;
            Assert.IsNotNull(submitTradeResponse);
            Assert.IsNull(submitTradeResponse.Error);
            Assert.AreEqual(submitTradeResponse.Status, "Success");
            Assert.IsNotNull(submitTradeResponse.TradeId);

            trade = new Trade()
            {
                Name = "GOOGL",
                Type = "BUY",
                Quantity = 100,
                Price = 101
            };
            submitTradeResponse = tradesController.Post(trade).Result;
            Assert.IsNotNull(submitTradeResponse);
            Assert.IsNull(submitTradeResponse.Error);
            Assert.AreEqual(submitTradeResponse.Status, "Success");
            Assert.IsNotNull(submitTradeResponse.TradeId);

            trade = new Trade()
            {
                Name = "GOOGL",
                Type = "BUY",
                Quantity = 150,
                Price = 100
            };
            submitTradeResponse = tradesController.Post(trade).Result;
            Assert.IsNotNull(submitTradeResponse);
            Assert.IsNull(submitTradeResponse.Error);
            Assert.AreEqual(submitTradeResponse.Status, "Success");
            Assert.IsNotNull(submitTradeResponse.TradeId);

            var executionResponse = tradesController.Get("CSGN").Result;

            Assert.IsNotNull(executionResponse);
            Assert.IsNotNull(executionResponse.Error);
            Assert.AreEqual(executionResponse.Status, "Failure");
            Assert.AreEqual(executionResponse.TradeExecutions.Count, 0);
        }

        [TestMethod]
        public void TestToCheckErrorIsReturnedWhenNoTradeCanBeExecutedOnAStock()
        {
            var tradesController = new TradesController();

            var trade = new Trade()
            {
                Name = "GOOGL",
                Type = "BUY",
                Quantity = 50,
                Price = 100
            };
            var submitTradeResponse = tradesController.Post(trade).Result;
            Assert.IsNotNull(submitTradeResponse);
            Assert.IsNull(submitTradeResponse.Error);
            Assert.AreEqual(submitTradeResponse.Status, "Success");
            Assert.IsNotNull(submitTradeResponse.TradeId);

            trade = new Trade()
            {
                Name = "GOOGL",
                Type = "BUY",
                Quantity = 100,
                Price = 101
            };
            submitTradeResponse = tradesController.Post(trade).Result;
            Assert.IsNotNull(submitTradeResponse);
            Assert.IsNull(submitTradeResponse.Error);
            Assert.AreEqual(submitTradeResponse.Status, "Success");
            Assert.IsNotNull(submitTradeResponse.TradeId);

            trade = new Trade()
            {
                Name = "GOOGL",
                Type = "BUY",
                Quantity = 150,
                Price = 100
            };
            submitTradeResponse = tradesController.Post(trade).Result;
            Assert.IsNotNull(submitTradeResponse);
            Assert.IsNull(submitTradeResponse.Error);
            Assert.AreEqual(submitTradeResponse.Status, "Success");
            Assert.IsNotNull(submitTradeResponse.TradeId);

            var executionResponse = tradesController.Get("GOOGL").Result;

            Assert.IsNotNull(executionResponse);
            Assert.IsNotNull(executionResponse.Error);
            Assert.AreEqual(executionResponse.Status, "Failure");
            Assert.AreEqual(executionResponse.TradeExecutions.Count, 0);
        }

        [TestMethod]
        public void TestToCheckErrorIsReturnedWhenTradeNameIsInvalid()
        {
            var tradesController = new TradesController();

            var trade = new Trade()
            {
                Name = "",
                Type = "BUY",
                Quantity = 50,
                Price = 100
            };
            var submitTradeResponse = tradesController.Post(trade).Result;
            Assert.IsNotNull(submitTradeResponse);
            Assert.IsNotNull(submitTradeResponse.Error);
            Assert.AreEqual(submitTradeResponse.Status, "Failure");
            Assert.IsNull(submitTradeResponse.TradeId);
        }

        [TestMethod]
        public void TestToCheckErrorIsReturnedWhenTradeQuantityIsInvalid()
        {
            var tradesController = new TradesController();

            var trade = new Trade()
            {
                Name = "APPL",
                Type = "BUY",
                Quantity = -50,
                Price = 100
            };
            var submitTradeResponse = tradesController.Post(trade).Result;
            Assert.IsNotNull(submitTradeResponse);
            Assert.IsNotNull(submitTradeResponse.Error);
            Assert.AreEqual(submitTradeResponse.Status, "Failure");
            Assert.IsNull(submitTradeResponse.TradeId);
        }

        [TestMethod]
        public void TestToCheckErrorIsReturnedWhenTradePriceIsInvalid()
        {
            var tradesController = new TradesController();

            var trade = new Trade()
            {
                Name = "FCBL",
                Type = "BUY",
                Quantity = 50,
                Price = -65
            };
            var submitTradeResponse = tradesController.Post(trade).Result;
            Assert.IsNotNull(submitTradeResponse);
            Assert.IsNotNull(submitTradeResponse.Error);
            Assert.AreEqual(submitTradeResponse.Status, "Failure");
            Assert.IsNull(submitTradeResponse.TradeId);
        }

        [TestMethod]
        public void TestToCheckErrorIsReturnedWhenTradeTypeIsEmpty()
        {
            var tradesController = new TradesController();

            var trade = new Trade()
            {
                Name = "CSAG",
                Type = "",
                Quantity = 50,
                Price = 100
            };
            var submitTradeResponse = tradesController.Post(trade).Result;
            Assert.IsNotNull(submitTradeResponse);
            Assert.IsNotNull(submitTradeResponse.Error);
            Assert.AreEqual(submitTradeResponse.Status, "Failure");
            Assert.IsNull(submitTradeResponse.TradeId);
        }

        [TestMethod]
        public void TestToCheckErrorIsReturnedWhenTradeTypeIsInvalid()
        {
            var tradesController = new TradesController();

            var trade = new Trade()
            {
                Name = "APPL",
                Type = "HOLD",
                Quantity = 50,
                Price = 100
            };
            var submitTradeResponse = tradesController.Post(trade).Result;
            Assert.IsNotNull(submitTradeResponse);
            Assert.IsNotNull(submitTradeResponse.Error);
            Assert.AreEqual(submitTradeResponse.Status, "Failure");
            Assert.IsNull(submitTradeResponse.TradeId);
        }
    }
}
