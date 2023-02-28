using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserIdentity.Identity;

namespace UserIdentity.Controllers
{
    public class UserAdminController : Controller
    {
        //AdminController Index actionu tüm kullanıcı bilgilerini göstersin, yani sisteme kayıt olan kullanıcıları admin panelinden görmek istediğinde burada genen bir UserManagere ihtiyaç oluyor 

        private UserManager<ApplicationUser> userManager;

        public UserAdminController()
        {
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new IdentityDataContext()));
            //Yukarıdaki userManager'i constructor içerisinden doldururken, bizden en sonda bir DbContext bekliyor bizde bizim oluşturduğumuz  IdentityDataContext i gönderdik

        }

        // GET: Admin
        public ActionResult UserList()
        {
            return View(userManager.Users); 
            //bütün kullanıcı listesini viewe taşıyor 
        }
    }
}