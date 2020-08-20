using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace QRcode_menu.Models
{
    public class menuItem
    {
        public string titulo_es { get; set; }
        public string titulo_en { get; set; }
        public string titulo_pt { get; set; }        
    }

    public class menuDta
    {
        public List<menuItem> items { get; set; }
        public string Nombre { get; set; }
        public string id { get; set; }
        
    }
}