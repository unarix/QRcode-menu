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
        public List<menuItem> items = new List<menuItem>();

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
                items = md.items;
            }
        }

        public void OnPost()
        {
            
        }

        // Este método lo uso solo la primera vez para poder crear un objeto del tipo json que luego voy a desserializar
        private void crearJson()
        {
            menuDta dta = new menuDta();
            dta.Nombre = "Menu de mañana";
            dta.id = "1";
            dta.items = new List<menuItem>();
            menuItem mnu = new menuItem();
            mnu.titulo_es = "Papas Fritas";
            mnu.titulo_en = "Chips";
            mnu.titulo_pt = "Patatas Fritas";
            dta.items.Add(mnu);

            //Obtengo el directorio para el archivo
            var service = HttpContext.RequestServices.GetService(typeof(Microsoft.AspNetCore.Hosting.IHostingEnvironment)) as Microsoft.AspNetCore.Hosting.IHostingEnvironment;
            string folderName = "json/";
            string webRootPath = service.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);

            //Serializo el objeto a json
            string objeto = JsonConvert.SerializeObject(dta);

            //Escribo en disco el archivo
            System.IO.File.WriteAllText(newPath + @"/menu_" + dta.id + ".json", objeto);
        }


    }
}

