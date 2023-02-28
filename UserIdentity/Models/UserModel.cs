using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UserIdentity.Models
{
    //Burada ApplicationUser da geçerleri olan değerleri girdik fazladan ekleme yapmadık 
    //Eğer yaparsakta ApplicationUser içerisine tanımlamamız gerekir
    public class Register
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }     
        [Required]
        public string Password { get; set; }
    }

    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }


    }
    //-------------------------------------------------------------------------------
    public class RoleEditModel
    {
        //RoleAdminController'da RoleList actionunda edit butonuna tıklanıldığı zaman,tıklanılan RoleName'e sahip kullanıcıları Members olarak sayfa üzerine getirmek. Tıklanılan RoleName'e sahip olmayan kullanıcılarıda NonMembers listesiyle sayfaya getirmek.
        //RoleAdminController Get(Edit) methodu içerisine doldurup sayfaya gönderiyoruz. 
        public IdentityRole Role  { get; set; }                            //Role bilgileri buradan gelecek
         
        public IEnumerable<IdentityUser> Members { get; set; }             //Yukarıdaki Rolün içerisinde olan kullanıcılar

        public IEnumerable<IdentityUser> NonMembers { get; set; }          

    }

    public class RoleUpdateModel
    {
        //Memberlerden her bir checkboxa karşılık gelen userId posta geldiğinde ise IdToDelete olarak gelecek   
        //nonMemberlerden gelen kullanıcıların Id'leri ise checkboxlara atanacak ve IdsToAdd olacak
        //RoleAdminController Post(Edit) methodu içerisine gelecek bilgiler 
        [Required]
        public string RoleName { get; set; }
        public string RoleId { get; set; }

        public string[] IdsToAdd { get; set; }
        //(IdsToAdd )Role eklenmesini istediğimiz checkboxların atandığı user id'lerini barınıracak. Form üzerinden Action posta gelecek olan, kullannıcıların Id bilgilerini o rol içerisine atayacağız.
        public string[] IdsToDelete { get; set; } //buradan gele id bilgilerini'de o rol içerisinden sileceğiz
    }
    //-------------------------------------------------------------------------------

}