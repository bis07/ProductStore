using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ActivityLibrary1.Abstract;
using ActivityLibrary1.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Controllers;



namespace SportsStoreTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Products()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns(new Product[]
            {
                new Product {ProductId = 1, Name ="P1"},
                new Product {ProductId = 2, Name ="P2"},
                new Product {ProductId = 3, Name ="P3"}
            });

            AdminController controller = new AdminController(mock.Object);

            Product[] result = ((IEnumerable<Product>) controller.Index().ViewData.Model).ToArray();

            Assert.AreEqual(result.Length,3);
            Assert.AreEqual(result[0].Name,"P1");
            Assert.AreEqual("P2",result[1].Name);
            Assert.AreEqual(result[2].Name, "P3");
        }
        [TestMethod]
        public void Can_Edit_Products()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns(new Product[]
            {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"},
                new Product {ProductId = 3, Name = "P3"}
            });
            AdminController controller = new AdminController(mock.Object);

            Product p1 = controller.Edit(1).Model as Product;
            Product p2 = controller.Edit(2).Model as Product;
            Product p3 = controller.Edit(3).Model as Product;

            Assert.AreEqual(p1.Name,"P1");
            Assert.AreEqual(p2.ProductId,2);
            Assert.AreEqual(p3.ProductId,3);
        }
        [TestMethod]

        public void Cannot_Edit_NotExistet_Products()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns(new Product[]
            {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"},
                new Product {ProductId = 3, Name = "P3"}
            });
            AdminController controller = new AdminController(mock.Object);
            
            Product result = controller.Edit(4).Model as Product;
            
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Can_Save_Valid_Products()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            AdminController controller = new AdminController(mock.Object);
            Product p1 = new Product {Name = "P1"};
            ActionResult result = controller.Edit(p1);
            
            mock.Verify(x=>x.SaveProduct(p1));
            Assert.IsNotInstanceOfType(result,typeof(ViewResult));
        }
        [TestMethod]
        public void Cannot_Save_Invalid_Products()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            AdminController controller = new AdminController(mock.Object);

            Product p1 = new Product { Name = "P1" };
            controller.ModelState.AddModelError("error","error");

            ActionResult result = controller.Edit(p1);

            mock.Verify(x => x.SaveProduct(It.IsAny<Product>()),Times.Never);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_Products()
        {
            Product prod = new Product {ProductId = 3, Name = "P3"};
            Mock<IProductRepository>mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns(new Product[]
            {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"},
                new Product {ProductId = 4,Name = "P4"},
                prod
            });

            AdminController controller = new AdminController(mock.Object);

            controller.Delete(prod.ProductId);
            
            mock.Verify(x=>x.DeleteProduct(prod.ProductId));
            
        }
    }
}
