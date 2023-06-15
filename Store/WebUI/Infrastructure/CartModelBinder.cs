using Domain.Entities;
using System.Web.Mvc;

namespace WebUI.Infrastructure
{
    public class CartModelBinder : IModelBinder
    {
        private const string session = "Cart";
        public object BindModel(ControllerContext contrlContxt, ModelBindingContext bindCOntxt  )
        {
            Cart cart = null;
            if(contrlContxt.HttpContext.Session!=null)
            {
                cart = (Cart)contrlContxt.HttpContext.Session[session];
            }
            if (cart == null)
            {
                cart = new Cart();
                if(contrlContxt.HttpContext.Session[session]==null)
                    contrlContxt.HttpContext.Session[session] = cart;
            }
            return cart;
        }
    }
}