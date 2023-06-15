using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    [AllowAnonymous]
    public class GoodsController : Controller
    {
        private IGoodsRepository repository;
        public int pageSize = 8;

        public GoodsController(IGoodsRepository repo)
        {
            repository = repo;
        }
        public ViewResult List(string genre, int page = 1)
        {
            GoodsListViewModel model = new GoodsListViewModel
            {
                Goods = repository.Goods.Where
                (b => genre == null || b.Genre == genre).OrderBy(good => good.GoodsId).
                Skip((page - 1) * pageSize).Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = genre == null ?
                    repository.Goods.Count() :
                    repository.Goods.Where(goods => goods.Genre == genre).Count()
                }
            };
            return View(model);
        }
        public ViewResult Contact()
        {
            return View();
        }
        public ViewResult About()
        {
            return View();
        }
        [HttpPost]
        public ViewResult List(string genre, string searchTerm, int page = 1)
        {
            GoodsListViewModel model;
            if (string.IsNullOrEmpty(searchTerm))
            {
                model = new GoodsListViewModel
                {
                    Goods = repository.Goods.Where
                  (b => genre == null || b.Genre == genre).OrderBy(good => good.GoodsId).
                  Skip((page - 1) * pageSize).Take(pageSize),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = pageSize,
                        TotalItems = genre == null ?
                      repository.Goods.Count() :
                      repository.Goods.Where(goods => goods.Genre == genre).Count()
                    }
                };
                return View(model);
            }
            else
            {
                model = new GoodsListViewModel
                {
                    Goods = repository.Goods.Where
                  (x => x.Name.StartsWith(searchTerm)).OrderBy(good => good.GoodsId).
                  Skip((page - 1) * pageSize).Take(pageSize),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = pageSize,
                        TotalItems = repository.Goods.Where
                             (x => x.Name.StartsWith(searchTerm)).Count()
                    }
                }; 
                return View(model);
            }
        }

    }
}