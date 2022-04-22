using System;
using System.ComponentModel.DataAnnotations;

namespace SqlApi.Models
{
    public class Log
    {
        
        public int? USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public int? MODULE_ID { get; set; }
        public int? PROGRAM_ID { get; set; }
        public string LOG_DATE { get; set; }
        public string ACTIVITY_TYPE { get; set; }
    }
}


