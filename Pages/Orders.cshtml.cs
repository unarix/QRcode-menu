﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Extensions.Caching.Memory;
using QRcode_menu.Models;

namespace QRcode_menu.Pages
{
    public class OrdersModel : PageModel
    {
        public orderDta ordersD { get; set; }
        public string idMesa { get; set; }
        public string idSide { get; set; }

        public void OnGet()
        {
            // Cargo el json en la entidad de ordenes
            var service = HttpContext.RequestServices.GetService(typeof(Microsoft.AspNetCore.Hosting.IHostingEnvironment)) as Microsoft.AspNetCore.Hosting.IHostingEnvironment;
            string folderName = "json/";
            string webRootPath = service.WebRootPath;
            string Path = System.IO.Path.Combine(webRootPath, folderName);
            // Cargo el json en la entidad
            orderDta od = JsonConvert.DeserializeObject<orderDta>(System.IO.File.ReadAllText(Path + @"/orders.json"));
            // cargo el objeto json en el objeto del modelo
            ordersD = od;
        }
        public void OnPost()
        {
            string mesa = Request.Form[nameof(idMesa)];
            string side = Request.Form[nameof(idSide)];
            // Cargo el json en la entidad de ordenes
            var service = HttpContext.RequestServices.GetService(typeof(Microsoft.AspNetCore.Hosting.IHostingEnvironment)) as Microsoft.AspNetCore.Hosting.IHostingEnvironment;
            string folderName = "json/";
            string webRootPath = service.WebRootPath;
            string Path = System.IO.Path.Combine(webRootPath, folderName);
            // Cargo el json en la entidad
            orderDta od = JsonConvert.DeserializeObject<orderDta>(System.IO.File.ReadAllText(Path + @"/orders.json"));
            // Elimino el pedido de la mesa enviada
            od.order.RemoveAll(x => (x.mesa == mesa && x.side == side));
            // Vuelvo a guardar el objeto serializado
            string objeto = JsonConvert.SerializeObject(od);
            System.IO.File.WriteAllText(Path + @"/orders.json", objeto);
            
            ordersD = od;
        }
    }
}
