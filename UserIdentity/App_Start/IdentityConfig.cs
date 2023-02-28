using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Threading.Tasks;

//!!!!!!!!!!!ÖNEMLİ!!!!!!!!!
//OwinStartUp sınıfı burası(IdentityConfig) 
//Owin, uygulama ile server arasındaki bir ara katman, bu ara katman içerisinde istediğimiz ayarlamaları uygulamaya tanıtabiliyoruz
//örneğin bu uygulama içerisinde kullanıcın tarayıcısına bırakacağımız cookie yardımıyla mı kullanıcıyı tanıyalım yada biz bu tanıma işlemini servisler içerisindeki token'ler ile mi yapalım  
//Genel ollarak Identitiy içerisinde kullanacağımız ayarları özelleştirmek için kullanacağımız bir yapı
//Uygulamaya facebook ile mi google ile mi login olacağız gibi standartları uygulamaya tanıtacağımız bir alan
//Bunun için App_Start altına Owin Startup Class(özel class) oluşturduk -class isimini de IdentityConfig koyduk
//Sonra Web.Config içerisinde Uygulamaya tanıttık

//OwinStartup'ın yerini belirtiyoruz (UserIdentity.IdentityConfig)  -proje ismi.OwninClass
[assembly: OwinStartup(typeof(UserIdentity.IdentityConfig))]

namespace UserIdentity
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
               AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie, //Kullanıcın Authentication işlemleri cookielere göre 
                LoginPath = new PathString("/Account/Login")          //kullanıcı izni olmayan bir alana girmeye çalıştığı zaman 
            });
        }
    }
}
