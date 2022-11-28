using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NOVA.Models
{
    public class SignInTBL
    {
        
        public int INCKEY { get; set; }
        
        public int USER_ID { get; set; }
        public string USER_NAME { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime LOG_DATETIME { get; set; }

        public string ACTIVITY_TYPE { get; set; }

        public string PLATFORM { get; set; }
    }
}