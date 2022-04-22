using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NOVA.Models
{
    public class UsersViewModel
    {
        public User user { get; set; }
        public IList<User> users { get; set; }
    }
}