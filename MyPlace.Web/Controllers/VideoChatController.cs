using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace MyPlace.Web.Controllers
{
    public class VideoChatController : Controller
    {
        public string _UserID
        {
            get
            {
                return User.Identity.GetUserId();
            }
        }


        [Authorize]
        public ActionResult Index()
        {
            ViewBag.MyId =_UserID;
            ViewBag.MyName = User.Identity.Name;
            return View();
        }

        public ActionResult MultiVideoChat()
        {
            return View();
        }

        public ActionResult SingleVideoChat()
        {
            string result = string.Empty;
            string callid = string.Empty;
            string code = string.Empty;
            if (!string.IsNullOrEmpty(HttpContext.Request.QueryString["code"]))
            {
                code = HttpContext.Request.QueryString["code"];
            }
          if(!string.IsNullOrEmpty(HttpContext.Request.QueryString["name"]))
          {
              result = HttpContext.Request.QueryString["name"];
              callid = HttpContext.Request.QueryString["callid"];
          }
          ViewBag.CallingUser = result;
          ViewBag.CallerId = callid;
          ViewBag.SuccessCode = code;
            return View();
        }
    }
}