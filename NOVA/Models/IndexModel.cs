using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NOVA.Models
{
    public class IndexModel:PageModel
    { private readonly UretimTakipService _service;
        public List<UretimTakip> UretimTakips;
        public IndexModel(UretimTakipService uretimTakipservice)
        {
            _service = uretimTakipservice;
            UretimTakips = _service.TakipList();
            
        }
        
        
        
    }
}