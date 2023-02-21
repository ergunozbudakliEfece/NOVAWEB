using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NOVA.Models
{
    public class Login
    {
        
        [Display(Name = "User Name")]
        public string USER_NAME { get; set; }

        public string defurl { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string USER_PASSWORD { get; set; }
        [Display(Name = "Beni Hatırla")]
        public string RememberMe { get; set; }
        public PasswordChange PasswordChange { get; set; }
        public User User { get; set; }
    }
}