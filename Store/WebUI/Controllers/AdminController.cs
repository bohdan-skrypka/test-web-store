using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Domain.App_Data;
using Domain.Entities;
using System.Collections.Generic;
using System.Web;
using WebUI.Filters;

namespace WebUI.Controllers
{
    [Auth] // пользовательский фильтр аутентификации
    [Authorize(Users = "skripkabodya@gmail.com, skripkabodya97@gmail.com")]
    public class AdminController : Controller
    {
        private EFDataDb db = new EFDataDb();

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult GetGoodsData(string searchTerm)
        {
            IEnumerable<Goods> goods;
            if (searchTerm != null)
            {
                goods = db.Goods.Where(p => p.Name.StartsWith(searchTerm)).ToList();
            }
            else
                goods = db.Goods.ToList();
            return PartialView("_AdminTable",goods);
        }
        public ActionResult GetList(string term)
        {
            List<string> list;
            list = (db.Goods.Where(x => x.Name.StartsWith(term)).Select(x => x.Name).ToList());
            return View("_AdminTable", list);
        }

        public ActionResult ManageGoods()
        {
            return View(db.Goods.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Goods goods = db.Goods.Find(id);
            if (goods == null)
            {
                return HttpNotFound();
            }
            return View(goods);
        }

        //Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create( Goods goods)
        {
            if (ModelState.IsValid)
            {
                db.Goods.Add(goods);
                db.SaveChanges();
                TempData["messageAdd"] = string.Format("Запись <<{0}>> успешно создана", goods.Name);
                return RedirectToAction("ManageGoods");
            }

            return View(goods);
        }

        //*///Edit
        //gif jpeg png jpg
        public bool IsValidImage(string contentType)
        {
            if ((contentType.Equals("image/png") || contentType.Equals("image/gif")
                || contentType.Equals("image/jpg") || contentType.Equals("image/jpeg")))
            return true;
        else
            return false;
  
        }
        //pdf , docx ,doc
        public bool IsValidFile(string contentType)
        {
            if ((contentType.Equals("application/pdf") ||
                    contentType.Equals("application/vnd.openxmlformats-officedocument.wordprocessingml.document")))
                return true;
            else
                return false;

        }
        //2MB
        public bool IsValidLength(int contentLength)
        {
            double r = ((contentLength / 1024 ) / 3072);
            bool temp = ((contentLength / 1024) / 3072) <= 3;
            return temp;
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Goods goods = db.Goods.Find(id);
            if (goods == null)
            {
                return HttpNotFound();
            }
            return View(goods);
        }

        [HttpPost]
        public ActionResult Edit(Goods goods, HttpPostedFileBase image, HttpPostedFileBase file)
        {
            bool t = true;
            //// загрузка картинки
            try
            {
                t = IsValidImage(image.ContentType);
            }
            catch
            { }
            if (!t)
            {
                TempData["ErrorTypeImage"] = "Выберите картинку типа .gif, .jpeg, .png, .jpg";
                return View("Edit", goods);
            }
            else
            {
                try
                {
                    if (!IsValidLength(image.ContentLength))
                    {
                        TempData["ErrorLengthImage"] = "Размер картинки превысил 3МБ!!!";
                        return View("Edit", goods);
                    }
                }
                catch { }
                #region загрузка файла
                ///
                //а
                /*   //// загрузка файла 
                   t = true;
                   try
                   {
                       t = IsValidFile(file.ContentType);
                   }
                   catch
                   {
                   }
                   if (!t)
                   {
                       TempData["ErrorTypeImage"] = "Выберите файл типа .pdf, .docx";
                       return View("Edit", goods);
                   }
                   else
                   {
                       try
                       {
                           if (!IsValidLength(file.ContentLength))
                           {
                               TempData["ErrorLengthImage"] = "Размер файла превысил 3МБ!!!";
                               return View("Edit", goods);
                           }
                       }
                       catch { }
                   }
                   */
                #endregion
                ////// проверка формы на валидность
                if (ModelState.IsValid)
                {
                    if (image != null)
                    {
                        goods.ImageMimeType = image.ContentType;
                        goods.ImageData = new byte[image.ContentLength];
                        image.InputStream.Read(goods.ImageData, 0, image.ContentLength);
                    }
                    else
                    {
                        goods.ImageData = null;
                        goods.ImageMimeType = null;

                    }
                    db.Entry(goods).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["message"] = string.Format("Запись <<{0}>> была изменина", goods.Name);
                    return RedirectToAction("ManageGoods");
                }

                else
                    return View(goods);
            }
        }
        ////*//*
        [NonAction]
        public void RemoveImg(int ImageId)
        {
            if (db.Goods.Find(ImageId).ImageData != null)
            {
                db.Goods.Find(ImageId).ImageData = null;
                db.Goods.Find(ImageId).ImageMimeType = null;
                db.SaveChanges();
            }
        }

        [NonAction]
        public void UpdateImg(int ImageId)
        {
            if (db.Goods.Find(ImageId).ImageData != null)
            {
                db.Goods.Find(ImageId).ImageData = null;
                db.Goods.Find(ImageId).ImageMimeType = null;
                db.SaveChanges();
            }
        }

        [NonAction]
        public void RemoveFile(int FileId)
        {
            if (db.Goods.Find(FileId).ImageData != null)
            {
                db.Goods.Find(FileId).ImageData = null;
                db.Goods.Find(FileId).ImageMimeType = null;
                db.SaveChanges();
            }
        }

        [NonAction]
        public void UpdateFile(int FileId)
        {
            if (db.Goods.Find(FileId).ImageData != null)
            {
                db.Goods.Find(FileId).ImageData = null;
                db.Goods.Find(FileId).ImageMimeType = null;
                db.SaveChanges();
            }
        }

        [AllowAnonymous]
        public FileContentResult GetImage(int GoodsId)
        {
            Goods goods = db.Goods.Find(GoodsId);
            if (goods != null)
            {
                return File(goods.ImageData, goods.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
        [AllowAnonymous]
        public FileContentResult GetFile(int FileId)
        {
            Goods goods = db.Goods.FirstOrDefault(p => p.GoodsId == FileId);
            if (goods != null)
            {
                return File(goods.ImageData, goods.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
        //Delete

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Goods goods = db.Goods.Find(id);
            if (goods == null)
            {
                return HttpNotFound();
            }
            return View(goods);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
      {
            Goods goods = db.Goods.Find(id);
            db.Goods.Remove(goods);
            db.SaveChanges();
            TempData["message"] = string.Format("Выбранная запись удалена");
            return RedirectToAction("ManageGoods");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}