﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PocketMall.Domain.Abstract;
using PocketMall.Domain.Entities;
using PocketMall.WebUI.Controllers;

namespace PocketMall.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "p1"},
                new Product {ProductID = 2, Name = "p2"},
                new Product {ProductID = 3, Name = "p3"},
                new Product {ProductID = 4, Name = "p4"},
                new Product {ProductID = 5, Name = "p5"},
            });

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //Act
            IEnumerable<Product> result =
                (IEnumerable<Product>)controller.List(2).Model;

            //Assert
            Product[] prodArray = result.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "p4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }
    }
}