 using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserIdentity.Identity
{
    //IdentityDbContext generik olduğı için buna kullanacağımız IdentityUser'ı da vermemiz gerekiyor
    public class IdentityDataContext: IdentityDbContext<ApplicationUser>
    {
        //DbContext sınıfından miras aldığımız mantığa benziyor
        //connnectionStringi(identityConnnection) belirtmek için bir constructor oluşturuyoruz ve constructorun base'ine(IdentityDbContext)'e connnectionString gönderiyoruz
        //Sonra gidip connnectionString'i (identityConnnection) gidip Web.config'de oluşturuyoruz

        //identity veritabanı ve normal veri tabanını ayrı ayrı oluşturmak daha çok tercih edilir
        public IdentityDataContext():base("identityConnection")
        {

        }


    }
}