using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Controllers;
using SportsStore.Infrastructure.Abstract;
using SportsStore.Models;

namespace SportsStoreTests
{
    [TestClass]
    public  class AdminSecurityTests
    {
        [TestMethod]
        public void Can_Login_With_Valid_Credentials()
        {
            Mock<IAuthProvider>mock = new Mock<IAuthProvider>();
            mock.Setup(x => x.Authenticate("admin", "secret")).Returns(true);

            LoginViewModel model = new LoginViewModel
            {
                UserName = "admin",
                Password = "secret"
            };

            AccountController controller= new AccountController(mock.Object);
            ActionResult result = controller.Login(model, "/MyURL");

            Assert.AreEqual("/MyURL",((RedirectResult)result).Url);
            Assert.IsInstanceOfType(result,typeof(RedirectResult));
        }

        [TestMethod]
        public void Cannot_Login_With_Invalid_Credentials()
        {
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(x => x.Authenticate("user", "hello")).Returns(false);

            LoginViewModel model = new LoginViewModel
            {
                UserName = "user",
                Password = "hello"
            };

            AccountController controller =new AccountController(mock.Object);

            ActionResult resut = controller.Login(model, "/MyURL");

            Assert.IsFalse(((ViewResult)resut).ViewData.ModelState.IsValid);
            Assert.IsInstanceOfType(resut,typeof(ViewResult));
        }
    }
}
