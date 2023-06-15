using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    [AllowAnonymous]
    public class NavController : Controller
    {
        private IGoodsRepository repository;
        public NavController(IGoodsRepository repo)
        {
            repository = repo;
        }

        public PartialViewResult Menu(string genre = null, bool horizNav = false)
        {
            ViewBag.SelectedGenre = genre;

            IEnumerable<string> genres = repository.Goods.Select(good => good.Genre).Distinct().OrderBy(x => x);
            string viewName = horizNav ? "MenuHorizontal":"Menu";
            return PartialView("FlexMenu",genres);
        }
       
    }
}