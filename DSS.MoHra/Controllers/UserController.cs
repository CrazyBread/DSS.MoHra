using DSS.MoHra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DSS.MoHra.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public ActionResult List()
        {
            var users = db.Users.Include("Information").Include("Role").OrderBy(i => i.Login);
            return View(new Models.UserViewModel() { Users = users });
        }

        public ActionResult Edit(int? id)
        {
            var userRoles = db.UserRoles.OrderBy(i => i.Name).ToList();
            var model = new UserEditViewModel(userRoles, id.HasValue);

            if (!User.IsInRole("admin") && Helpers.Identity.User.Id != id)
                return HttpNotFound("access denided");

            if (id.HasValue)
            {
                var user = db.Users.Include("Information").FirstOrDefault(i => i.Id == id);
                if (user == null)
                    return HttpNotFound("user not found");
                if (user.Information != null)
                    model.Additional = user.Information;
                model.Main = new UserMainModel()
                {
                    Login = user.Login,
                    RoleId = user.RoleId
                };
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(
            int? id,
            [Bind(Prefix = "Main")] UserMainModel model,
            [Bind(Prefix = "Additional", Include = "Name,DepartmentPosition,DepartmentName,CompanyName,Phone,Email")] UserInformation info)
        {
            try
            {
                if (!User.IsInRole("admin") && Helpers.Identity.User.Id != id)
                    return JsonResponse("Нет доступа.");

                if (model == null)
                    throw new MetaException("Отсутствуют необходимые сведения для регистрации пользователя.");
                if (ModelState.IsValid)
                {
                    // get other data
                    var role = db.UserRoles.FirstOrDefault(i => i.Id == model.RoleId);

                    // checks
                    if (db.Users.Any(i => i.Login.ToLower() == model.Login.ToLower() && i.Id != id))
                        throw new MetaException("Пользователь с таким логином уже зарегистрирован в системе.", "Main.Login");
                    if (role == null)
                        throw new MetaException("Выберите необходимую роль пользователя.", "Main.RoleId");
                    if (!id.HasValue && string.IsNullOrEmpty(model.Password))
                        throw new MetaException("При создании нового пользователя необходимо задать ему пароль.", "Main.Password");

                    // save
                    if (id.HasValue)
                    {
                        if (!string.IsNullOrEmpty(model.Password))
                            IdentityManager.ChangePassword(model.Login, model.Password);
                        var user = db.Users.First(i => i.Id == id);
                        user.Login = model.Login;
                        user.RoleId = model.RoleId;
                        user.Updated = DateTime.Now;
                        db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        id = IdentityManager.Register(model.Login, model.Password, role.Code);
                    }

                    // save additional info
                    if (info != null)
                    {
                        var realInfo = db.UserInformations.FirstOrDefault(i => i.UserId == id);
                        if (realInfo == null)
                        {
                            info.Created = DateTime.Now;
                            info.UserId = id.Value;
                            db.UserInformations.Add(info);
                        }
                        else
                        {
                            realInfo.Import(info, "Name,DepartmentPosition,DepartmentName,CompanyName,Phone,Email");
                            realInfo.Updated = DateTime.Now;
                            db.Entry(realInfo).State = System.Data.Entity.EntityState.Modified;
                        }
                        db.SaveChanges();
                    }
                    return JsonResponse(true, Url.Action("List"));
                }
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }
            return JsonResponse();
        }

        [HttpPost, Authorize(Roles = "admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                // get
                var item = db.Users.Include("Role").Include("Information").FirstOrDefault(i => i.Id == id);
                if (item == null)
                    return HttpNotFound("user not found");

                // check
                if (item.Role.Code == "admin" && db.Users.Count(i => i.Role.Code == "admin") == 1)
                    throw new MetaException("Удаление последнего администратора системы невозможно.");

                // act
                if (item.Information != null)
                    db.UserInformations.Remove(item.Information);
                db.Users.Remove(item);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
                this.StoreModelStateErrors();
            }
            return RedirectToAction("List");
        }
    }
}