using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DSS.MoHra.Models
{
    public class AccountLoginModel
    {
        [Required(ErrorMessage = "Введите логин")]
        public string Login { set; get; }

        [Required(ErrorMessage = "Введите пароль")]
        public string Password { set; get; }

        public bool IsPersistent { set; get; }
    }
}