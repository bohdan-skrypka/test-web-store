using Domain.Abstract;
using Domain.Entities;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    [AllowAnonymous]
    public class CartController : Controller
    {
        private IGoodsRepository repository;
        private IOrderProcessor orderProcessor;
        public CartController(IGoodsRepository repo,IOrderProcessor ordProc)
        {
            repository=repo;
            orderProcessor = ordProc;
        }
    
        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }
        public RedirectToRouteResult AddToCart(Cart cart,int ? GoodId,string returnUrl)
        {
            Goods good = repository.Goods.FirstOrDefault(b => b.GoodsId == GoodId);
            if (good != null)
                cart.AddItem(good, 1);
            return RedirectToAction("Index", new { returnUrl });
        }
        public ViewResult Index(Cart cart,string ReturnUrl)
        {
            return View(new CartIndexViewModel {
                Cart = cart,
                ReturnUrl = ReturnUrl
            });
        }
        public RedirectToRouteResult RemoveFromCart(Cart cart)
        {
            if (cart != null)
               cart.Clear();

            return RedirectToAction("List","Goods");
        }
        [HttpGet]
        public ViewResult RsvpForm()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RsvpForm(ShippingDetails guest,Cart cart)
        {
            if (cart.Lines.Count()==0)
            { TempData["message"] ="Ваша корзина пуста!!!";
                return View();
            }

            string browser = HttpContext.Request.Browser.Browser;
            var IP = HttpContext.Request.UserHostAddress;

            TempData["data"] = "Данные о пользователе:"+"<br/>"+"Browser:"+browser+"<br/>"+"IP:"+IP+"<br/>";
            TempData["ShippingDetails"] += "Имя: "+guest.Name+ "<br/>";
            TempData["ShippingDetails"] += "Адрес: " + guest.Line1+";";
            TempData["ShippingDetails"] += guest.Line2 ?? "";
            TempData["ShippingDetails"] += guest.Line3 ?? "" + "<br/>";
            TempData["ShippingDetails"] += "Город: "+guest.City + "<br/>";
            TempData["ShippingDetails"] += "Страна: "+guest.Country + "<br/>";
            TempData["ShippingDetails"] += "Дополнительные услуги: ";
            TempData["ShippingDetails"] += guest.GiftWrap ? "Подарочная упаковка " : "Без подарочной упаковки ";
            if (ModelState.IsValid)
            {
                return View("Thanks",cart);
            }
            else
            {
                return View();
            }
        }
    }
}