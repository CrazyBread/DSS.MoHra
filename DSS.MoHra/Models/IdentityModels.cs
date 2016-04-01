using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Web;

namespace DSS.MoHra.Models
{
    public static class IdentityManager
    {
        public static bool IsCreditinalsCorrect(string login, string password, ref int userId)
        {
            bool result = false;
            using (var db = new DataContext())
            {
                // check admin existance
                _CheckAdmin(db);

                // get current user info
                var userInfo = db.Users.FirstOrDefault(i => i.Login == login);
                if (userInfo != null)
                {
                    var generatedHash = _GenerateHash(password, userInfo.PasswordSalt);
                    if (generatedHash == userInfo.PasswordHash)
                    {
                        result = true;
                        userId = userInfo.Id;
                    }
                }
            }
            return result;
        }

        public static ClaimsIdentity GenerateIdentity(int userId, string username)
        {
            ClaimsIdentity identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Name, username));
            return identity;
        }

        public static int Register(string login, string password, string role)
        {
            int result = -1;

            // normalize strings
            login = login.Trim();
            password = password.Trim();

            using (var db = new DataContext())
            {
                var salt = _GenerateSalt();
                var hash = _GenerateHash(password, salt);
                var roleId = db.UserRoles.First(i => i.Code == role).Id;

                var user = new User()
                {
                    Created = DateTime.Now,
                    Login = login,
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    RoleId = roleId
                };
                db.Users.Add(user);
                db.SaveChanges();
                result = user.Id;
            }
            return result;
        }
        
        public static void ChangePassword(string login, string password)
        {
            // normalize strings
            login = login.Trim();
            password = password.Trim();

            using (var db = new Models.DataContext())
            {
                // find
                var user = db.Users.FirstOrDefault(i => i.Login.ToLower() == login.ToLower());
                if (user == null)
                    throw new MetaException("Пользователь с таким логином не найден.");

                // generate
                var salt = _GenerateSalt();
                var hash = _GenerateHash(password, salt);

                // set
                user.PasswordHash = hash;
                user.PasswordSalt = salt;

                // save
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        private static string _GenerateSalt()
        {
            int startIndex = 48;
            int endIndex = 122;
            Random r = new Random();
            string result = string.Empty;

            for (int i = 1; i <= 16; i++)
                result += (char)r.Next(startIndex, endIndex);

            return result;
        }

        private static string _GetHashSha512(string input)
        {
            byte[] tmp = System.Text.Encoding.UTF8.GetBytes(input);

            SHA512 hasher = SHA512.Create();

            tmp = hasher.ComputeHash(tmp);
            return Convert.ToBase64String(tmp);
        }

        private static string _GenerateHash(string source, string salt)
        {
            if (source.Length > 3)
            {
                source = source.Substring(0, 2) + "=^__^==" + salt + source.Substring(3);
            }
            else
            {
                source += "+^" + salt;
            }
            return _GetHashSha512(source);
        }

        private static void _CheckAdmin(DataContext db)
        {
            // check role
            if (!RoleManager.HasRole("admin"))
                RoleManager.AddRole("admin", "Администратор системы");

            // check admin user
            if (!db.Users.Any(i => i.Role.Code == "admin"))
            {
                Register("admin", "admin", "admin");
            }
        }
    }

    public static class RoleManager
    {
        private static List<UserRole> _Roles = new List<UserRole>();

        public static List<string> GetRoles(string role)
        {
            var result = new List<string>();
            if (_Roles == null || !_Roles.Any())
                _Roles = new DataContext().UserRoles.Include("Parent").ToList();
            var _role = _Roles.FirstOrDefault(i => i.Code == role);
            while(_role != null)
            {
                result.Add(_role.Code);
                _role = (_role.Parent != null) ? _Roles.FirstOrDefault(i => i.Code == _role.Parent.Code) : null;
            }
            return result;
        }

        public static void AddRole(string code, string name)
        {
            using (var db = new DataContext())
            {
                var role = db.UserRoles.FirstOrDefault(i => i.Code == code);

                if (role != null)
                    throw new ArgumentException();

                var userRole = new UserRole()
                {
                    Created = DateTime.Now,
                    Code = code,
                    Name = name
                };
                db.UserRoles.Add(userRole);
                db.SaveChanges();
            }
        }

        public static bool HasRole(string code)
        {
            bool result = false;
            using (var db = new DataContext())
            {
                result = db.UserRoles.Any(i => i.Code == code);
            }
            return result;
        }
    }

    public class OwnPrincipal : ClaimsPrincipal
    {
        public User User { protected set; get; }

        public OwnPrincipal(User user, IIdentity identity) : base(identity)
        {
            User = user;
        }
    }
}