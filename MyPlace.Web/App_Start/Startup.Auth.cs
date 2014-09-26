using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Google;
using Owin;
using System;
using MyPlace.Web.Models;

namespace MyPlace.Web
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "000000004C120661",
            //    clientSecret: "XbakcpN-UjJEGfeuobI9efYjHlIolOFV");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            app.UseFacebookAuthentication(
               appId: "614094388711282",
               appSecret: "a92e7b77e8e5f1c5ce337e270f5dacd7");

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
               ClientId = "941359399121-0gv63kqru474cnv0nm8oidau0lh2ghh3.apps.googleusercontent.com",
               ClientSecret = "zhj_NnOITBPzhh7CLNZNzLyP"
              
            }
            );
        }
    }
}


// CallbackPath = new PathString("/Account/ExternalLoginCallback")

//{
//                ClientId = "941359399121-0gv63kqru474cnv0nm8oidau0lh2ghh3.apps.googleusercontent.com",
//                ClientSecret = "zhj_NnOITBPzhh7CLNZNzLyP"
//            }