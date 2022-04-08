
using System;
namespace NOVA.Models
{
    

    public partial class User
    {
        public User()
        {

        }
        public long USER_ID { get; set; }

     
        public string USER_NAME { get; set; }

        public string USER_PASSWORD { get; set; }

        public string USER_FIRSTNAME { get; set; }

        
        public string USER_LASTNAME { get; set; }

    
        public string USER_ROLE { get; set; }

        public bool ACTIVE { get; set; }

        public string USER_MAIL { get; set; }
    }
}