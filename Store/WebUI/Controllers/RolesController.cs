using Domain.App_Data;
using Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebUI.Filters;
using WebUI.Models;

namespace WebUI.Controllers
{
    [Auth] // пользовательский фильтр аутентификации
    [Authorize(Users = "skripkabodya@gmail.com, skripkabodya97@gmail.com")]
    public class RolesController : Controller
    {
        private  EFDataDb UserList = new EFDataDb();

        private ApplicationRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }
        
        public ActionResult ListUsers()
        {
            
            return View(UserList.Users.ToList());
        }
        public PartialViewResult GetUserData(string searchTerm)
        {
            IEnumerable<Users> users;
            if (searchTerm != null)
            {
                users = UserList.Users.Where(p => p.UserName.StartsWith(searchTerm)).ToList();
            }
            else
                users = UserList.Users.ToList();
            return PartialView("_AllUsersTable", users);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListRole()
        {
            return View(RoleManager.Roles);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateRoleModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await RoleManager.CreateAsync(new ApplicationRole
                {
                    Name = model.Name,
                    Description = model.Description
                });
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRole");
                }
                else
                {
                    ModelState.AddModelError("", "Что-то пошло не так");
                }
            }
            return View(model);
        }

        public async Task<ActionResult> Edit(string id)
        {
            ApplicationRole role = await RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                return View(new EditRoleModel { Id = role.Id, Name = role.Name, Description = role.Description });
            }
            return RedirectToAction("ListRole");
        }
        [HttpPost]
        public async Task<ActionResult> Edit(EditRoleModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationRole role = await RoleManager.FindByIdAsync(model.Id);
                if (role != null)
                {
                    role.Description = model.Description;
                    role.Name = model.Name;
                    IdentityResult result = await RoleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRole");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Что-то пошло не так");
                    }
                }
            }
            return View(model);
        }

        public async Task<ActionResult> Delete(string id)
        {
            ApplicationRole role = await RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await RoleManager.DeleteAsync(role);
            }
            return RedirectToAction("ListRole");
        }
    }
}