using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserIdentity.Identity;
using UserIdentity.Models;

namespace UserIdentity.Controllers
{
    [Authorize]
    //Controller bazında Autherizstion için kullandık.
    //Controller içerisinde dışarda tutumak istediğimiz alan varsa [AllowAnonymous] 
    public class AccountController : Controller
    {

        //Kullanıcı oluşturma işlemini burada yapacağız ama bunu yapmadna öce bir UserManager'e ihtiyaç var ve aşağıdaki şekilde bunu aldık
        //UserManager bir generik  ve bizden IdentityUser'ı(ApplicationUser) bekliyor
        private UserManager<ApplicationUser> userManager;

        public AccountController()
        {
            //Yukarıdaki userManager'i constructor içerisinden doldururken, bizden en sonda bir DbContext bekliyor bizde bizim oluşturduğumuz  IdentityDataContext'i gönderdik
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new IdentityDataContext()));


            //Parolamızın istediğimiz koşulda olup olmadığının kontrolünü sağlıyoruz 
            //Burakadi yapılardan ekstra olarak kontrol istiyorsak (Identity Klasörü -> CustomPasswordValidator)
            userManager.PasswordValidator = new PasswordValidator()
            {
                RequireDigit = true, 
                RequiredLength = 7,
                RequireLowercase = true,
                RequireUppercase = true,
                RequireNonLetterOrDigit = true,
            };
                

            userManager.UserValidator = new UserValidator<ApplicationUser>(userManager)
            {
                RequireUniqueEmail = true,             //MAİL ADRESLERİNİN UNİQUE OLMASINI BURADA SAĞLADIK
                AllowOnlyAlphanumericUserNames = false,
            };

        }


        //------------------------------------------------------------------------------------------
        [AllowAnonymous]     //Regiaster alanına Authenticate olmadan  erişilebilmesini sağlıyor (Login olmayan misafir kullanıcılar)
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Register(Register model)     //RegisterFormdan Post isteğinde bulunulduğunda bir Model gelecek ve modelimizin türü Register
        {

            if (ModelState.IsValid)   //(ModelState.IsValid) Formdaki veriler Register Modeldeki şekliyle doldurulduysa 
            {
                var user = new ApplicationUser();
                user.Email = model.Email;
                user.UserName = model.UserName;

                var result = userManager.Create(user, model.Password); //Buradaki Create() bir İdnetityResult döndürüyor

                if (result.Succeeded) 
                {
                    userManager.AddToRole(user.Id, "User"); //kaydolan kullanıcıların rolünü user olarak belirledik
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                        //result.Succeeded başaraılı olmazsa result hata fırlatıyor onu burada alarak  Register.cshmtl de validationSummary'de çıkacak
                    }

                }
            }
            return View(model);
            //kullanıcını girdiği bilgilerle beraber tekrardan registere dönüyor bilgiler hatalı da olsa kullanıcı form üzerinde tekrardan görecek
        }
        //------------------------------------------------------------------------------------------

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            //exp: Authantice olmadan bir alana ulaşmak isterse .../Account/Login?ReturnUrl=%2Home%2About
            //Get isteğiyle beraber ReturnURl gelirse bunu alıp Login View'ine gönderiyoruz

            if (HttpContext.User.Identity.IsAuthenticated)  //eğer kullanıcı kaydolduysa ve yetkisi olmayan bir alana erişmek isterse
            {
                return View("Error", new string[] { "Erişim hakkınız yok" });
            }

            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model, string returnUrl)
        //(string ReturnUrl) -Login View'inde Post işlemi sonucunda ReturnUrl gelecek boş yada dolu olabilir
        {

            if (ModelState.IsValid)
            {

                var user = userManager.Find(model.UserName, model.Password);

                if (user == null)
                {
                    ModelState.AddModelError("", "Yanalış Kullanıcı Adı Veya Parola");
                }
                else
                //Kullanıcın tarayıcısına kullanıcının tanımlamasıyla alakalı bir Cookie bırakamak gerekiyor
                {
                    //authManager -Uyulamadaki Login veya LogOut işlemini yerine getirecek nesne
                    var authManager = HttpContext.GetOwinContext().Authentication;

                    //idendtity diyerek bir cookie oluşturup authManager aracılığıyla kullanıcıya göndereceğiz
                    //CreateIdentity() bizden bir user ve ikinci olarakta Othentication'un typenı belirtmemiz gerekiyor
                    // Oluşturuduğumuz İdentity cookisini authManager aracılığıyla kullanıcıya gönderirken yanında bir kaç özellik gönderebiliyoruz exp:kullanıcının cookiesi kalıcı olark oluşturulsun mu.(birsonraki geldiğinde kullanıcı tanınsın mı)
                    var identity = userManager.CreateIdentity(user, "ApplicationCookie");
                    var authProperties = new AuthenticationProperties()
                    {
                        IsPersistent = true,
                    };

                    authManager.SignOut();
                    authManager.SignIn(authProperties, identity);

                    return Redirect(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl);
                    //returnUrl boş veya null ise kullanıcıyı direkt anadizine yönlendirdik
                }
            }

            //hata varsa tekrardan girilen değerlerle beraber login sayfasına gönderdik
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }
        //-----------------------------------------------------------------------------------------
        public ActionResult Logout()
        {
            //authManager -Uyulamadaki Login veya LogOut işlemini yerine getirecek nesne
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
            return RedirectToAction("Login");

        }



    }
}