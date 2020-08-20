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
    public class AboutModel : PageModel
    {
        public string Message { get; set; }

        public void OnGet()
        {
            string id = Request.Query["id"];
            if(id != null)
            {
                //Obtengo el directorio para el archivo json
                var service = HttpContext.RequestServices.GetService(typeof(Microsoft.AspNetCore.Hosting.IHostingEnvironment)) as Microsoft.AspNetCore.Hosting.IHostingEnvironment;
                string folderName = "json/";
                string webRootPath = service.WebRootPath;
                string Path = System.IO.Path.Combine(webRootPath, folderName);

                // Cargo el json en la entidad
                menuDta sf = JsonConvert.DeserializeObject<menuDta>(System.IO.File.ReadAllText(Path + @"/menu_" + id + ".json"));
                
            }
        }

        public void OnPost()
        {

        }

    }
}