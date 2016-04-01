using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DSS.MoHra.Models
{
    public class UserViewModel
    {
        public IQueryable<User> Users { set; get; }
    }

    public class UserMainModel
    {
        [Required(ErrorMessage = "Введите логин")]
        [Display(Name = "Логин")]
        public string Login { set; get; }

        [MinLength(6)]
        [Display(Name = "Пароль")]
        public string Password { set; get; }

        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Пароли должны совпадать")]
        [Display(Name = "Подтверждение пароля")]
        public string PasswordConfirm { set; get; }

        [Required(ErrorMessage = "Выберите роль")]
        [Display(Name = "Роль")]
        public int RoleId { set; get; }
    }

    public class UserEditViewModel
    {
        public UserEditViewModel(IList<UserRole> userRoles, bool isEdit)
        {
            RoleOptions = new SelectList(userRoles, "Id", "Name");
            IsEdit = isEdit;
        }

        public UserMainModel Main { set; get; }
        public UserInformation Additional { set; get; }

        public SelectList RoleOptions { set; get; }
        public bool IsEdit { set; get; }
    }
}