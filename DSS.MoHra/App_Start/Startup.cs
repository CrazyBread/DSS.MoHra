using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Linq;

[assembly: OwinStartup(typeof(DSS.MoHra.App_Start.Startup))]

namespace DSS.MoHra.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // authorization
            System.Web.Helpers.AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                CookieSecure = CookieSecureOption.SameAsRequest,
                SlidingExpiration = true,
                ExpireTimeSpan = new TimeSpan(31, 0, 0, 1), // 31 days and 1 sec
                Provider = new OwnCookieAuthenticationProvider()
            });
        }
    }

    public class OwnCookieAuthenticationProvider : CookieAuthenticationProvider
    {
        public override Task ValidateIdentity(CookieValidateIdentityContext context)
        {
            if (context.Identity.IsAuthenticated)
            {
                var userId = context.Identity.FindFirstValue(ClaimTypes.NameIdentifier);
                int id = -1;
                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out id))
                {
                    context.RejectIdentity();
                    return base.ValidateIdentity(context);
                }

                // get user info from database
                Models.User user = null;
                using (var db = new Models.DataContext())
                {
                    user = db.Users.Include("Role").Include("Information").FirstOrDefault(i => i.Id == id);
                    
                    // check user not exists
                    if (user == null)
                    {
                        context.RejectIdentity();
                        return base.ValidateIdentity(context);
                    }

                    if (user.Information == null)
                        user.Information = new Models.UserInformation() { UserId = user.Id };
                    
                    var roles = Models.RoleManager.GetRoles(user.Role.Code);
                    foreach(var role in roles)
                        context.Identity.AddClaim(new Claim(ClaimTypes.Role, role));
                }
            }
            return base.ValidateIdentity(context);
        }
    }
}
