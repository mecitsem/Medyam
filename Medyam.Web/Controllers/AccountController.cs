using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Medyam.Core.Helpers;
using Medyam.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Medyam.Web.Controllers
{
    public class AccountController : Controller
    {

        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string password, string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(password))
                return Login();

            var passPhrase = CloudConfigurationManager.GetSetting(Core.Common.Constants.AppSettings.PassPhrase);

            if (string.IsNullOrWhiteSpace(passPhrase))
                return Login();


            if (password.Equals(passPhrase))
            {
                FormsAuthentication.SetAuthCookie(Common.Constants.User.UserName, false);

                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }

            return View();
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
    }
}