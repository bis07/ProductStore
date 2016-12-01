using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ActivityLibrary1.Abstract;
using ActivityLibrary1.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Controllers;
using SportsStore.HTMLHelpers;
using SportsStore.Models;

namespace SportsStoreTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns(new Product[]
            {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"},
                new Product {ProductId = 3, Name = "P3"},
                new Product {ProductId = 4, Name = "P4"},
                new Product {ProductId = 5, Name = "P5"},
            });
            ProductController conteller = new ProductController(mock.Object);
            conteller.pageSize = 3;
            ProductsListViewModel result = (ProductsListViewModel)conteller.List(null, 2).Model;
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper myHelper = null;
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };
            Func<int, string> pageUrlDelagate = i => "Page" + i;
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelagate);

            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                            + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                            + @"<a class=""btn btn-default"" href=""Page3"">3</a>", result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns(new Product[] {
                new Product { ProductId = 1, Name = "P1" },
                new Product { ProductId = 2, Name = "P2" },
                new Product { ProductId = 3, Name = "P3" },
                new Product { ProductId = 4, Name = "P4" },
                new Product { ProductId = 5, Name = "P5" },
            });
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns(new Product[] {
                new Product { ProductId = 1, Name = "P1",Category= "Cat1" },
                new Product { ProductId = 2, Name = "P2",Category= "Cat2" },
                new Product { ProductId = 3, Name = "P3",Category= "Cat1" },
                new Product { ProductId = 4, Name = "P4",Category= "Cat2" },
                new Product { ProductId = 5, Name = "P5",Category= "Cat3" }
            });
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            Product[] result = ((ProductsListViewModel)controller.List("Cat2", 1).Model).Products.ToArray();

            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "P4" && result[0].Category == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(x=>x.Products).Returns(new Product[] {
                new Product { ProductId = 1, Name = "P1",Category= "Apples" },
                new Product { ProductId = 2, Name = "P2",Category= "Apples" },
                new Product { ProductId = 3, Name = "P3",Category= "Plums" },
                new Product { ProductId = 4, Name = "P4",Category= "Oranges" }
            });

            NavController controller = new NavController(mock.Object);

            string[] result = ((IEnumerable<string>)controller.Menu().Model).ToArray();

            string category = "Apples";

            string checkViewBAg = controller.Menu(category).ViewBag.SelectedCategory;

            Assert.AreEqual(result.Length,3);
            Assert.AreEqual(result[0],"Apples");
            Assert.AreEqual(result[1], "Oranges");
            Assert.AreEqual(result[2],"Plums");
            Assert.AreEqual(checkViewBAg,category);
        }

        [TestMethod]
        public void Generate_Category_Specific_Product_Count()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns(new Product[] {
                new Product { ProductId = 1, Name = "P1",Category= "Cat1" },
                new Product { ProductId = 2, Name = "P2",Category= "Cat2" },
                new Product { ProductId = 3, Name = "P3",Category= "Cat1" },
                new Product { ProductId = 4, Name = "P4",Category= "Cat2" },
                new Product { ProductId = 5, Name = "P5",Category= "Cat3" }
            });

            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            int result1 = ((ProductsListViewModel) controller.List("Cat1").Model).PagingInfo.TotalItems;
            int result2 = ((ProductsListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems;
            int result3 = ((ProductsListViewModel)controller.List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((ProductsListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            Assert.AreEqual(result1,2);
            Assert.AreEqual(result2,2);
            Assert.AreEqual(result3,1);
            Assert.AreEqual(resAll,5);
        }
    }
}
