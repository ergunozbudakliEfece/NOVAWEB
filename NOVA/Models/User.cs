
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NOVA.Models
{
    

    public partial class User
    {
      
        public string USER_ID { get; set; }

     
        public string USER_NAME { get; set; }

        public string USER_PASSWORD { get; set; }

        public string USER_FIRSTNAME { get; set; }

        
        public string USER_LASTNAME { get; set; }

    
        public string USER_ROLE { get; set; }
        public Roles roles { get; set; }
        public bool ACTIVE { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Lütfen Mail Adresinizi Girin")]
        [DataType(DataType.EmailAddress)]
        public string USER_MAIL { get; set; }
        public bool? USER_AUTH { get; set; }


        public bool? SELECT_AUTH { get; set; }

        public bool? INSERT_AUTH { get; set; }

        public bool? UPDATE_AUTH { get; set; }

        public bool? DELETE_AUTH { get; set; }

        public int? MODULE_INCKEY { get; set; }

        public Modules modules { get; set; }
        public Mail mail { get; set; }
        
    }
}