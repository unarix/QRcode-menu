using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace QRcode_menu.Models
{
    public class orders
    {
        public string fecha_hora { get; set; }
        public string orden { get; set; }
        public string mesa { get; set; }     
        public bool entregado { get; set; }
    }

    public class orderDta
    {
        public List<orders> order { get; set; }        
    }
}