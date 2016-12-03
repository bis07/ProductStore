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
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrive_Image_Data()
        {
            Product prod = new Product
            {
                ProductId = 2,
                Name = "Test",
                ImageData = new byte[] {},
                ImageMimeType = "image/png"
            };
            Mock<IProductRepository>mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns(new Product[]
            {
                new Product {ProductId = 1, Name = "P1"},
                prod,
                new Product {ProductId = 3, Name = "P3"}
            }.AsQueryable());

            ProductController controller= new ProductController(mock.Object);
            ActionResult result = controller.GetImage(2);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result,typeof(FileResult));
            Assert.AreEqual(prod.ImageMimeType,((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retrive_Image_Data_For_Invalid_Id()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns(new Product[]
            {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P3"}
            }.AsQueryable());

            ProductController controller = new ProductController(mock.Object);

            ActionResult result = controller.GetImage(100);

            Assert.IsNull(result);
        }
    }
}
