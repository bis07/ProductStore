using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ActivityLibrary1.Entities;

namespace SportsStore.Infrastructure.Binders
{
    public class CartModelBinder :IModelBinder
    {
        private const string sessionKey = "Cart";
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //Получить объект Cart из сеанса
            Cart cart = null;
            if (controllerContext.HttpContext.Session !=null)
            {
                cart = (Cart) controllerContext.HttpContext.Session[sessionKey];
            }
            // Создать экземпляр Cart, если он не обнаружен в данных сеанса
            if (cart==null)
            {
                cart=new Cart();
                if (controllerContext.HttpContext.Session!=null)
                {
                    controllerContext.HttpContext.Session[sessionKey] = cart;
                }
            }
            return cart;
        }
    }
}