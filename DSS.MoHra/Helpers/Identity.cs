using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace DSS.MoHra.Helpers
{
    public static class Identity
    {
        public static Models.User User
        {
            get
            {
                var userInfo = System.Threading.Thread.CurrentPrincipal as ClaimsPrincipal;
                if (userInfo == null || !(userInfo.Identity is ClaimsIdentity) || !userInfo.Identity.IsAuthenticated)
                    return null;
                if (userInfo is Models.OwnPrincipal)
                    return (userInfo as Models.OwnPrincipal).User;

                // try to get user information from claim
                int userId = -1;
                var claim = (userInfo.Identity as ClaimsIdentity).FindFirst(ClaimTypes.NameIdentifier);
                if (claim == null || string.IsNullOrEmpty(claim.Value) || !int.TryParse(claim.Value, out userId))
                    return null;

                // get user info from database
                Models.User user = null;
                using (var db = new Models.DataContext())
                {
                    user = db.Users.Include("Role").Include("Information").FirstOrDefault(i => i.Id == userId);
                    if (user.Information == null)
                        user.Information = new Models.UserInformation() { UserId = user.Id };
                }

                // update principal and return user info
                System.Threading.Thread.CurrentPrincipal = new Models.OwnPrincipal(user, userInfo.Identity);
                return user;
            }
        }

        public static bool IsAuthenticated
        {
            get
            {
                return System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated;
            }
        }
    }
}