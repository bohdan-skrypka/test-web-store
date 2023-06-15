using Domain.App_Data;
using Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebUI.Filters;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class AccountController : Controller
    {
        private EFDataDb db = new EFDataDb();

        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser { UserName = model.Email, Email = model.Email, Year = model.Year,PhoneNumber=model.PhoneNumber };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    Users userDb = new Users();
                    userDb.Id = user.Id;
                    userDb.Year = user.Year;
                    userDb.UserName = user.UserName;
                    userDb.PhoneNumber = user.PhoneNumber;
                    userDb.Email = user.Email;

                    try { 
                        db.Users.Add(userDb);
                        db.SaveChanges();
                    }
                    catch(DbEntityValidationException e)
                    {
                        db.SaveChanges();
                    }
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return View(model);
        }

        // аутентификация
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindAsync(model.Email, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                }
                else 
                {
                    if (user.UserName == "skripkabodya@gmail.com" || user.UserName == "skripkabodya97@gmail.com")
                        TempData["messageUser"] = " Администратор сайта!! ";
                    else 
                        TempData["messageUser"] = " Пользователь сайта!! ";

                    ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    if (String.IsNullOrEmpty(returnUrl))
                        return RedirectToAction("List", "Goods");
                    return Redirect(returnUrl);
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }

        // редакция и удаление пользователей
        [HttpGet]
        [Auth]
        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        [Auth]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed()
        {
            ApplicationUser user = await UserManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                if (user.UserName == "skripkabodya@gmail.com" || user.UserName == "skripkabodya97@gmail.com")
                {
                    TempData["message"] = "Попытка удалить администрацию сайта!!!Доступ заперщен. Удаление возможно только из базы данных через скрипт";
                    return RedirectToActionPermanent("List", "Goods");
                }
                else
                {
                    IdentityResult result = await UserManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Logout", "Account");
                    }
                }
             }
          return RedirectToAction("List", "Goods");
        }

        [Auth]
        public async Task<ActionResult> Edit()
        {
            ApplicationUser user = await UserManager.FindByNameAsync(User.Identity.Name);
            if (user!=null)
            {
                if (user.UserName == "skripkabodya@gmail.com" || user.UserName == "skripkabodya97@gmail.com")
                {
                    return new HttpStatusCodeResult(404, "Attempt to change administrator data!!!");
                }
                EditModel model = new EditModel { Year = user.Year,PhoneNumber=user.PhoneNumber };
                return View(model);
            }
            else 
                return RedirectToAction("Login", "Account");
        }
        
        [Auth]
        [HttpPost]
        public async Task<ActionResult> Edit(EditModel model)
        {
            ApplicationUser user = await UserManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                Users userDb = new Users();

                user.Year = model.Year;
                user.PhoneNumber = model.PhoneNumber;

                userDb.Id = user.Id;
                userDb.Year = user.Year;
                userDb.UserName = user.UserName;
                userDb.PhoneNumber = user.PhoneNumber;
                userDb.Email = user.Email;


                IdentityResult result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    db.Entry(userDb).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("List", "Goods");
                }
                else
                {
                    ModelState.AddModelError("", "Что-то пошло не так");
                }
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не найден");
            }

            return View(model);
        }

        // методы для удаления пользователей из базы данных АДМИНИСТРАТОРОМ
        [Auth]
        [Authorize(Users = "skripkabodya@gmail.com, skripkabodya97@gmail.com")]
        public ActionResult FastDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "This USER not exist!!!");
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound("NulReferences, please, check your input data!!!");
            }
            return View(users);
        }

        [Auth]
        [HttpPost, ActionName("FastDelete")]
        [Authorize(Users = "skripkabodya@gmail.com, skripkabodya97@gmail.com")]
        public async Task<ActionResult> FastDeleteConfirmed(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);

            if (user != null )
            {
                if (user.UserName == "skripkabodya@gmail.com" || user.UserName == "skripkabodya97@gmail.com")
                   {
                    TempData["message"] = "Попытка удалить администрацию сайта!!!Доступ заперщен. Удаление возможно только из базы данных через скрипт";
                    return RedirectToActionPermanent("ListUsers","Roles");
                   }
            }

            Users users = db.Users.Find(id);
            db.Users.Remove(users);
            db.SaveChanges();

            IdentityResult result = await UserManager.DeleteAsync(user);
            if (User.Identity.IsAuthenticated && User.Identity.Name == user.UserName)
            {
                AuthenticationManager.SignOut();
                return RedirectToAction("Login","Account");
            }
            return RedirectToAction("ListUsers", "Roles");
        }
    }
}