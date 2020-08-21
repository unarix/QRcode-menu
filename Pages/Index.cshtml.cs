using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.IO;
using QRcode_menu.Models;

namespace QRcode_menu.Pages
{
    public class IndexModel : PageModel
    {
        //public List<menuItem> items = new List<menuItem>();
        
        public List<menuType> types = new List<menuType>();

        public void OnGet()
        {
            // Solo usar este método para el caso que se quiera crear un objeto json de tipo menu por primera vez
            //crearJson();

            string id = "1" ; // Request.Query["id"];
            if(id != null)
            {
                //Obtengo el directorio para el archivo json
                var service = HttpContext.RequestServices.GetService(typeof(Microsoft.AspNetCore.Hosting.IHostingEnvironment)) as Microsoft.AspNetCore.Hosting.IHostingEnvironment;
                string folderName = "json/";
                string webRootPath = service.WebRootPath;
                string Path = System.IO.Path.Combine(webRootPath, folderName);

                // Cargo el json en la entidad
                menuDta md = JsonConvert.DeserializeObject<menuDta>(System.IO.File.ReadAllText(Path + @"/menu_" + id + ".json"));
                types = md.types;
            }
        }

        public void OnPost()
        {
            ViewData["types"] = types;
        }

        // Este método lo uso solo la primera vez para poder crear un objeto del tipo json que luego voy a desserializar
        private void crearJson()
        {
            menuDta mnu = new menuDta();
            mnu.Nombre = "Cena";
            mnu.id = "1";
            mnu.types = new List<menuType>();
            
            menuType type = new menuType();
            type.titulo = "Platos";
            type.hora_desde = "10:00";
            type.hora_hasta = "18:00";
            type.items = new List<menuItem>();
            
            menuItem item = new menuItem();
            item.titulo_es = "Papas Fritas";
            item.titulo_en = "Chips";
            item.titulo_pt = "Patatas Fritas";
            type.items.Add(item);
            menuItem item2 = new menuItem();
            item2.titulo_es = "Lomo";
            item2.titulo_en = "Meat Beef";
            item2.titulo_pt = "Rostizado";
            
            type.items.Add(item2);
            mnu.types.Add(type);


            //Obtengo el directorio para el archivo
            var service = HttpContext.RequestServices.GetService(typeof(Microsoft.AspNetCore.Hosting.IHostingEnvironment)) as Microsoft.AspNetCore.Hosting.IHostingEnvironment;
            string folderName = "json/";
            string webRootPath = service.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);

            //Serializo el objeto a json
            string objeto = JsonConvert.SerializeObject(mnu);

            //Escribo en disco el archivo
            System.IO.File.WriteAllText(newPath + @"/menu_" + mnu.id + ".json", objeto);
        }


    }
}

