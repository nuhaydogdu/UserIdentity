using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserIdentity.Identity;
using UserIdentity.Models;

namespace UserIdentity.Controllers
{
    [Authorize(Roles="Admin")]  
    public class RoleAdminController : Controller
    {
        //AplicationRole'ü IdentityRole'den türettik üzerinde değişiklik yapmadığımız için direkt olarak burada IdentityRole'ü kullanabiliyoruz.
        private RoleManager<IdentityRole> roleManager;
        private UserManager<ApplicationUser> userManager;


        public RoleAdminController()
        {
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDataContext()));
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new IdentityDataContext()));
        }

        //-----------------------------------------------------------
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string name)
        {
            if (ModelState.IsValid)
            {
                var result = roleManager.Create(new IdentityRole(name));


                if (result.Succeeded)
                {
                    return RedirectToAction("RoleList");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item);
                    }
                }
            }
            return View(name);
        }

        //--------------------------------------------------------------------------
        public ActionResult RoleList()
        {
            return View(roleManager.Roles);
        }

        //--------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var role=roleManager.FindById(id);  
            if(role!= null)
            {
                //roleManager.Delete(role); -bir result nesnesi dönüyor
               var result= roleManager.Delete(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("RoleList");
                }
                else
                {
                    return View("Error", result.Errors);
                    //result.Errors'lar ile beraber yeni bir actiona gönderdik
                }
            }
            else
            {
                return View("Error", new string[] {"Role Bulunamadı.."});
            }
        }

        //--------------------------------------------------------------------------
        //Buradaki görevimiz bize gelen id(Query string) ile rol bilgilerine ulaşıp, rol bilgilerinin yanında o role ait olan kullanıcıları memberleri yada o rol içerisinde olmayan kullanıcıları nonMemberları bir model(RoleEditModel) içerisine paketleyip sayfaya göndermek.
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var role= roleManager.FindById(id);

            var members = new List<ApplicationUser>();
            var nonMembers= new List<ApplicationUser>();

            foreach (var user in userManager.Users.ToList()) 
            {
                //List referans tipte ve atama işlemi referans olarak yapılıyor!!!!! Eğer IsInRole(user.Id, role.Name) true ise bu durumda members listesini ,list'in List Listesine aktaracağız.
                //IsInRole(user.Id, role.Name) -Gelen userId'lerin role.Name içerisinde olup olmadığının kontrolünü sağlıyoruz bu bize true false döndürüyor
                var list = userManager.IsInRole(user.Id, role.Name)? members : nonMembers;

                list.Add(user); 
            }
            return View(new RoleEditModel()
            {
                Role=role,  
                Members=members,    
                NonMembers=nonMembers
            });
                
        }

        [HttpPost]
        public ActionResult Edit(RoleUpdateModel model)
        {
            IdentityResult result; //Succeeded'in sonucuna bakmak için
            

            if (ModelState.IsValid)  
            {
                //birinci foreach ile istediğimiz Id'leri bu role atamış olduk
                //IdsToAdd'e bir parametre gelmezsse(null) ise hata almamak için bir dizi olarak algılanması gerekiyor (??)
                foreach (var userId in model.IdsToAdd ?? new string[] { }) 
                {
                    result = userManager.AddToRole(userId, model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }

                foreach (var userId in model.IdsToDelete ?? new string[] { })
                {
                    result = userManager.RemoveFromRole(userId, model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
                return RedirectToAction("RoleList");
            }


            return View("Error", new string[] {"Aranılan Rol Yok.."} );

        }
        //--------------------------------------------------------------------------

    }
}