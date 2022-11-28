using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NOVA.Models
{
    public class PasswordChange
    {
        public Guid CODE { get; set; }
        public string OLD_PASSWORD { get; set; }
        
        [StringLength(maximumLength:8,MinimumLength =6)]
        public string NEW_PASSWORD { get; set; }
        
        [StringLength(maximumLength: 8, MinimumLength = 6)]
        public string NEW_PASSWORD_REPEAT { get; set; }
    }
}