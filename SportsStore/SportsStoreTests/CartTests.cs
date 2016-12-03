using System.Linq;
using System.Web.Mvc;
using ActivityLibrary1.Abstract;
using ActivityLibrary1.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;

namespace SportsStoreTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            Product p1 = new Product { ProductId = 1, Name = "P1" };
            Product p2 = new Product { ProductId = 2, Name = "P2" };

            Cart target = new Cart();

            target.AddItem(p1,1);
            target.AddItem(p2,2);
            target.AddItem(p1,10);
            CartLine[] result = target.Lines.OrderBy(x=>x.Product.ProductId).ToArray();

            Assert.IsTrue(result.Length==2);
            Assert.AreEqual(result[0].Product,p1);
            Assert.AreEqual(result[0].Quantity,11);
            Assert.AreEqual(result[1].Product,p2);
            Assert.AreEqual(result[1].Quantity,2);
        }
        [TestMethod]
        public void Can_Remove_Line()
        {
            Product p1 = new Product { ProductId = 1, Name = "P1" };
            Product p2 = new Product { ProductId = 2, Name = "P2" };
            Product p3 = new Product { ProductId = 3, Name = "P3" };

            Cart target = new Cart();

            target.AddItem(p1,1);
            target.AddItem(p2,3);
            target.AddItem(p3,10);
            target.RemoveLine(p3);

            Assert.AreEqual(target.Lines.Count(x => x.Product==p3),0);
            Assert.AreEqual(target.Lines.Count(),2);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            Product p1 = new Product { ProductId = 1, Name = "P1",Price = 100M};
            Product p2 = new Product { ProductId = 2, Name = "P2" ,Price = 50M};

            Cart target = new Cart();

            target.AddItem(p1,3);
            target.AddItem(p2,5);
            target.AddItem(p1,1);

            decimal result = target.ComputeToValue();

            Assert.AreEqual(result,650M);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            Product p1 = new Product { ProductId = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductId = 2, Name = "P2", Price = 50M };

            Cart target = new Cart();

            target.AddItem(p1,1);
            target.AddItem(p2,1);
            target.Clear();

            Assert.AreEqual(target.Lines.Count(),0);
        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns(new Product[]
            {
                new Product {ProductId = 1,Name ="P1", Category = "Apples"}, 
            }.AsQueryable());
            
            Cart cart = new Cart();
            CartController controller = new CartController(mock.Object,null);

            controller.AddToCart(cart,1,null);

            Assert.AreEqual(cart.Lines.Count(),1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductId,1);
        }

        [TestMethod]
        public void Adding_Product_To_Cart_Goes_To_Cart_Screen()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(x=>x.Products).Returns(new Product[]
            {
               new Product {ProductId = 1,Name ="P1", Category = "Apples"},
            }.AsQueryable());

            Cart cart = new Cart();

            CartController controller = new CartController(mock.Object,null);

            RedirectToRouteResult result = controller.AddToCart(cart, 2, "myUrl");

            Assert.AreEqual(result.RouteValues["action"],"Index");
            Assert.AreEqual(result.RouteValues["returnUrl"],"myUrl");
        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            Cart cart = new Cart();
            CartController controller = new CartController(null,null);

            CartIndexViewModel result = (CartIndexViewModel) controller.Index(cart, "myUrl").ViewData.Model;

            Assert.AreEqual(result.Cart,cart);
            Assert.AreEqual(result.ReturnUrl,"myUrl");
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            ShippingDetails shippingDetails = new ShippingDetails();
            CartController controller = new CartController(null,mock.Object);

            ViewResult result = controller.CheckOut(cart, shippingDetails);
            mock.Verify(x=>x.ProcessorOrder(It.IsAny<Cart>(),It.IsAny<ShippingDetails>()),Times.Never());

            Assert.AreEqual("",result.ViewName);
            Assert.AreEqual(false,result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
             Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            CartController controller = new CartController(null,mock.Object);

            controller.ModelState.AddModelError("error","error");

            ViewResult result = controller.CheckOut(cart, new ShippingDetails());

            mock.Verify(x=>x.ProcessorOrder(It.IsAny<Cart>(),It.IsAny<ShippingDetails>()),Times.Never);

            Assert.AreEqual("",result.ViewName);
            Assert.AreEqual(false,result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            CartController controller = new CartController(null,mock.Object);

            var result = controller.CheckOut(cart, new ShippingDetails());
            mock.Verify(x=>x.ProcessorOrder(It.IsAny<Cart>(),It.IsAny<ShippingDetails>()),Times.Once);

            Assert.AreEqual("Completed",result.ViewName);
            Assert.AreEqual(true,result.ViewData.ModelState.IsValid);
        }


    }
}
