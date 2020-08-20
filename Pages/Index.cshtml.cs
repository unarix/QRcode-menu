using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
//using survey.Models;
using Newtonsoft.Json;
using System.IO;

namespace QRcode_menu.Pages
{
    public class IndexModel : PageModel
    {

        public void OnGet()
        {
            qrsourcefile = "";
        }

        public void OnPost()
        {
            nombre = Request.Form[nameof(nombre)];
            dni = Request.Form[nameof(dni)].ToString().Replace(".","");
            fiebre = Request.Form[nameof(fiebre)].ToString().Contains("true")? "SI" : "NO";
            tos = Request.Form[nameof(tos)].ToString().Contains("true")? "SI" : "NO";;
            garganta = Request.Form[nameof(garganta)].ToString().Contains("true")? "SI" : "NO";;
            olfato = Request.Form[nameof(olfato)].ToString().Contains("true")? "SI" : "NO";;
            respirar = Request.Form[nameof(respirar)].ToString().Contains("true")? "SI" : "NO";;
            diarrea = Request.Form[nameof(diarrea)].ToString().Contains("true")? "SI" : "NO";;
            vomito = Request.Form[nameof(vomito)].ToString().Contains("true")? "SI" : "NO";;
            erupciones = Request.Form[nameof(erupciones)].ToString().Contains("true")? "SI" : "NO";;
            cabeza = Request.Form[nameof(cabeza)].ToString().Contains("true")? "SI" : "NO";;
            muscular = Request.Form[nameof(muscular)].ToString().Contains("true")? "SI" : "NO";;
            sangrado = Request.Form[nameof(sangrado)].ToString().Contains("true")? "SI" : "NO";;
            piel = Request.Form[nameof(piel)].ToString().Contains("true")? "SI" : "NO";;
            contacto = Request.Form[nameof(contacto)].ToString().Contains("true")? "SI" : "NO";;
            contacto_desc = Request.Form[nameof(contacto_desc)];
            test = Request.Form[nameof(test)].ToString().Contains("true")? "SI" : "NO";;
            test_desc = Request.Form[nameof(test_desc)];
            fecha_test = Request.Form[nameof(fecha_test)];

            // surveyform sr = new surveyform();
            // sr.nombre = nombre.ToUpper();
            // sr.dni = dni.ToUpper();
            // sr.fiebre = fiebre; 
            // sr.tos = tos;
            // sr.garganta = garganta;
            // sr.olfato=olfato;
            // sr.respirar = respirar;
            // sr.diarrea = diarrea;
            // sr.vomito = vomito;
            // sr.erupciones = erupciones;
            // sr.cabeza = cabeza;
            // sr.muscular = muscular;
            // sr.sangrado = sangrado;
            // sr.piel = piel;
            // sr.contacto = contacto;
            // sr.contacto_desc = contacto_desc;
            // sr.test = test;
            // sr.test_desc = test_desc;
            // sr.fecha_test = fecha_test;

            //Obtengo el directorio para el archivo
            var service = HttpContext.RequestServices.GetService(typeof(Microsoft.AspNetCore.Hosting.IHostingEnvironment)) as Microsoft.AspNetCore.Hosting.IHostingEnvironment;
            string folderName = "json/";
            string webRootPath = service.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);

            //Serializo el objeto a json
            //string objeto = JsonConvert.SerializeObject(sr);

            //Escribo en disco el archivo
            //System.IO.File.WriteAllText(newPath + @"/person_" + dni + ".json", objeto);

            // Genero el QR
            qrsourcefile = "https://api.qrserver.com/v1/create-qr-code/?data=https://usurvey.azurewebsites.net/About?id="+ dni +"&amp;size=100x100" ;
        }

        public string nombre { get; set; }
        public string dni { get; set; }
        public string fiebre { get; set; }
        public string tos { get; set; }
        public string garganta { get; set; }
        public string olfato { get; set; }
        public string respirar { get; set; }
        public string diarrea { get; set; }
        public string vomito { get; set; }
        public string erupciones { get; set; }
        public string cabeza { get; set; }
        public string muscular { get; set; }
        public string sangrado { get; set; }
        public string piel { get; set; }
        public string contacto { get; set; }
        public string contacto_desc { get; set; }
        public string test { get; set; }
        public string fecha_test { get; set; }
        public string test_desc { get; set; }
        public string qrsourcefile
        {
            get;
            set;
        } 
    }
}

