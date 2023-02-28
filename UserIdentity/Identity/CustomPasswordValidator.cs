using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace UserIdentity.Identity
{

    //AccountController içerisinde userManager kullanarak yaptığımız kontrollerden ayrı olarak belirtmek istediğimiz özel kontroller varsa diye bu sınıfı oluşturduk ve bunu PasswordValidator den türettik
    public class CustomPasswordValidator: PasswordValidator
    {
        public override async Task<IdentityResult> ValidateAsync(string password)
        {
            var result = await base.ValidateAsync(password);

            if (password.Contains("12345"))
            {
                var errors =result.Errors.ToList();
                errors.Add("Parola ardaşık sayılar içermemeli");
                result = new IdentityResult(errors);
            }
            return result;
        }
        //EN SONDA BU KONTROLLERİDE DAHİL ETMEK İÇİN ACCOUNTCONTROLLERD'A  userManager.PasswordValidator = new PasswordValidator() 'DA PasswordValidator()'I 
        //CustomPasswordValidator() İLE GEĞİŞTİRMEK GEREKİYOR 
    }
}