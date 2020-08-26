﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Extensions.Caching.Memory;
using QRcode_menu.Models;

namespace QRcode_menu.Pages
{
    public class IndexModel : PageModel
    {
        private IMemoryCache memoryCache;
        public List<menuType> types = new List<menuType>();
        public string pedidos { get; set; }

        public IndexModel(IMemoryCache memoryCache)    
        {    
            this.memoryCache = memoryCache;
        }  

        public void OnGet()
        {
            pedidos = "";
            string id = Request.Query["id"];
            string sector = "1"; // Request.Query["sec"];
            string pedidoExistente;

            if(id != null)
            {
                bool isExist = memoryCache.TryGetValue("mesa_" + id, out pedidoExistente);

                if(!isExist || pedidoExistente.Equals(""))
                {
                    //Obtengo el directorio para el archivo json
                    var service = HttpContext.RequestServices.GetService(typeof(Microsoft.AspNetCore.Hosting.IHostingEnvironment)) as Microsoft.AspNetCore.Hosting.IHostingEnvironment;
                    string folderName = "json/";
                    string webRootPath = service.WebRootPath;
                    string Path = System.IO.Path.Combine(webRootPath, folderName);

                    // Cargo el json en la entidad
                    menuDta md = JsonConvert.DeserializeObject<menuDta>(System.IO.File.ReadAllText(Path + @"/menu_" + sector + ".json"));
                    types = md.types;
                }
                else
                {
                    string pedido = @"<div class='alert alert-success' role='alert'> 
                    <h4>Ups!!!</h4>
                    <p class='text-success'>Ya existe un pedido para tu mesa de hace menos de 5 minutos (Nro: " + id + ") </p>";

                    pedido += "<b>";
                    
                    pedido += pedidoExistente;
                    
                    pedido += @"</b> 
                    <h6>Si necesitas hacer alguna modificacion solicítelo al personal de la sala</h6></span>
                    </div>";

                    pedidos = pedido;
                }
            }
            else
            {
                string mensaje = @"<div class='alert alert-danger' role='alert'> 
                <h4>Ups!!! numero de mesa invalido. </h4>
                </div>";
                pedidos = mensaje;
            }
        }

        public void OnPost()
        {
            string id = Request.Query["id"];
            string pedido = "";
            try{
                string message = Request.Form[nameof(pedidos)];;
                string[] messageArray = message.Split("-");
                pedido = @"<div class='alert alert-success' role='alert'> 
                    <h4>Todo salio muy bien!</h4>
                    <p class='text-success'>Ya estamos preparando el pedido para tu mesa (Nro: " + id + ") </p>";

                pedido += "<b>";
                foreach(string m in messageArray){
                    pedido += m + "<br>";
                }
                pedido += @"</b> 
                    <h6>Si necesitas hacer alguna modificacion solicítelo al personal de la sala</h6></span>
                    </div>";

                //Aqui se debe llamar a la funcion que enviá el email; en caso de fallo debemos avisar que no se pudo realizar.
                if (!guardarPedido(messageArray, id)){
                    pedido = 
                        @"<div class='alert alert-danger' role='alert'> 
                            <h2>Uy! algo paso cuando enviábamos tu pedido, por favor contacta al personal de la sala.</h2>
                        </div>";
                }               
            }
            catch(Exception ex){
                pedido = @"<div class='alert alert-danger' role='alert'> Error: " + ex.Message + "</div>";
            }

            //Informamos el resultado del pedido
            pedidos = pedido;
        }

        private bool guardarPedido(string[] pedidos, string id)
        {
            try{        
                string pedido = "";
                orderDta orders = new orderDta(); 

                // Abro el objeto json e inserto el nuevo pedido
                var service = HttpContext.RequestServices.GetService(typeof(Microsoft.AspNetCore.Hosting.IHostingEnvironment)) as Microsoft.AspNetCore.Hosting.IHostingEnvironment;
                string folderName = "json/";
                string webRootPath = service.WebRootPath;
                string Path = System.IO.Path.Combine(webRootPath, folderName);

                // Cargo el json en la entidad
                orderDta od = JsonConvert.DeserializeObject<orderDta>(System.IO.File.ReadAllText(Path + @"/orders.json"));
                orders = od;
                orders order = new orders();
                order.mesa = id;
                order.fecha_hora = DateTime.Now.ToString();
                order.entregado = false;

                foreach(string m in pedidos){
                    if(!m.Trim().Equals(""))
                        pedido += m.Trim() + ", ";
                }
                
                order.orden = pedido;
                od.order.Add(order);

                // vuelvo a guardar el objeto en el documento json.
                string objeto = JsonConvert.SerializeObject(od);
                System.IO.File.WriteAllText(Path + @"/orders.json", objeto);

                // Uso el inmemory para guardar el pedido de esa mesa y que no vuelvan a hacerlo hasta 3000 seg despues.
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(300));
                memoryCache.Set("mesa_" + id, pedido, cacheEntryOptions);

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        // Este método lo uso solo la primera vez para poder crear un objeto del tipo json que luego voy a desserializar
        private void crearJsonMenu()
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

        private void crearJsonOrder()
        {
            orderDta mnu = new orderDta();
            mnu.order = new List<orders>();
            orders order = new orders();
            order.mesa = "1";
            order.orden = "ejemplo";
            order.fecha_hora = DateTime.Now.ToString();
            mnu.order.Add(order);


            //Obtengo el directorio para el archivo
            var service = HttpContext.RequestServices.GetService(typeof(Microsoft.AspNetCore.Hosting.IHostingEnvironment)) as Microsoft.AspNetCore.Hosting.IHostingEnvironment;
            string folderName = "json/";
            string webRootPath = service.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);

            //Serializo el objeto a json
            string objeto = JsonConvert.SerializeObject(mnu);

            //Escribo en disco el archivo
            System.IO.File.WriteAllText(newPath + @"/orders.json", objeto);
        }

    }
}

