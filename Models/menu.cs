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

    // Un tipo de menu pueden ser Platos o Bebidas que se pueden servir en determinado tipo de horario... especificamente.
    public class menuType
    {
        public List<menuItem> items { get; set; }
        public string titulo_es { get; set; }
        public string titulo_en { get; set; }
        public string titulo_pt { get; set; }
        public string descripcion { get; set; }
        public string hora_desde { get; set; }
        public string hora_hasta { get; set; }
    }

    // Un menu data puede contener distintos menues que se pueden servir en distintos horarios, pero solo serán de un determinado lugar (El ID)
    public class menuDta
    {
        public List<menuType> types { get; set; }
        public string Nombre { get; set; }
        public string id { get; set; }
        
    }
}