using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using ServiceIMSS;
using System.Configuration;
using System.Net;
using System.IO;
using ServiceInvgate;
using Entities.Invgate;
using ServiceBitacora;
using Entities.Intermedio;

namespace Prueba
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Pruebas bitacora
            //CreaTicket ticket = new CreaTicket();
            //ticket.TipoOperacion = "C_INC";
            //ticket.NombreProveedor = "PEOPLE-MEDIA";
            //ticket.TicketIMSS = "INC007878";
            //ticket.Descripcion = "PRUEBA DE INTEGRACIOn 7";
            //ticket.Resumen = "PRUEBA DE INTEGRACIOn 7";
            //ticket.Impacto = "1000";
            //ticket.Urgencia = "2000";
            //ticket.Prioridad = null;
            //ticket.TipoIncidencia = "";
            //ticket.FuenteReportada = "2000";
            //ticket.NombreProducto = "";
            //ticket.GrupoSoporte = "SOPORTE";
            //ticket.CategoriaOpe01 = "INFRAESTRUCTURA DE REDES Y TELECOMUNICACIONES";
            //ticket.CategoriaOpe02 = "REDES Y ENLACES";
            //ticket.CategoriaOpe03 = "SOPORTAR";
            //ticket.CategoriaPro01 = "HARDWARE";
            //ticket.CategoriaPro02 = "COMPUTO PERSONAL";
            //ticket.CategoriaPro03 = "";
            //ticket.EstadoNuevo = 1;
            //ticket.Notas = "Prueba de notas";
            //ticket.FechaCreacion = "02/02/2023 07:30:00 p.m.";
            //ticket.Cliente = "IMSS";
            //ticket.VIP = "Si";
            //ticket.Sensibilidad = "Sensibilidad";

            //String concat = "CAT PROD" + "|" +//"MESA DE SERVICIO IMSS" + "|" +
            //                "APLICACIONES|" +
            //                "ACCESO A SERVICIOS DE TI|" +
            //                "GESTIONAR NIVELES DE ACCESO|" +
            //                "SW MEDICO|" +
            //                "PROVISION DE SERVICIOS MEDICOS|" +
            //                "CALCULADORA COVID|" +
            //                "";

            //CategoriastInvgate invgate = new CategoriastInvgate();

            //invgate.GetCategoria(concat);

            //CatalogosData catalogos = new CatalogosData();
            //catalogos.GetEstatusIncidenteIMSS(4);

            //IncidentesGetRequest get = new IncidentesGetRequest();
            //get.id = 7211;

            //IncidentesInvgate invgate = new IncidentesInvgate();
            //invgate.GetIncidente(get);


            CategoriastInvgate invgate = new CategoriastInvgate();

            Categorizacion categorizacion = invgate.GetCategorizacion(1413);


            //IncidenteData incidenteData= new IncidenteData();
            ////incidenteData.Crear(ticket, 1275, out string result);
            //Ticket result = incidenteData.Get(ticket.TicketIMSS);
            //Console.WriteLine(result);  



            //Categorizacion categorizacion = new Categorizacion();
            //Incidentes incidentes = new Incidentes();

            //categorizacion.IDTicketRemedy = "INC840214";
            //categorizacion.IDTicketInvgate = "0001";
            //categorizacion.CatOperacion01 = "APLICACIONES";
            //categorizacion.CatOperacion02 = "ATENCION DE APLICACIONES";
            //categorizacion.CatOperacion03 = "SOPORTAR";
            //categorizacion.CatProducto01 = "SW MEDICO";
            //categorizacion.CatProducto02 = "PROVISION DE SERVICIOS MEDICOS";
            //categorizacion.CatProducto03 = "DEVENGO";


            //Result result = new Result();

            //result = incidentes.ActualizaCategorizacion(categorizacion);


            ////GET INCIDENTE
            //IncidentesGetRequest getRequest = new IncidentesGetRequest();
            //getRequest.id = 7086;

            //IncidentesInvgate instance = new IncidentesInvgate();
            //var respuesta = instance.GetIncidente(getRequest);


            ////POST INCIDENTE
            //IncidentesPostRequest postRequest = new IncidentesPostRequest();
            //postRequest.category_id = 1833;
            //postRequest.title = "INCIDENTE DE PRUEBA - LIM";
            //postRequest.description = "INCIDENTE DE PRUEBA - LIM";
            //postRequest.creator_id = 101;
            //postRequest.priority_id = 1; //"id": 1,            "name": "Baja"
            //postRequest.date = "1675126927";
            //postRequest.customer_id = 1;
            //postRequest.source_id = 2; //"id": 2,        "name": "Web"
            ////postRequest.related_to = [];
            ////postRequest.attachments = [];
            //postRequest.type_id = 1; // "id": 1, "name": "Incidente"



            //IncidentesInvgate incidentes = new IncidentesInvgate();
            //var respuesta = incidentes.PostIncidente(postRequest);


            //PUT INCIDENTE
            //IncidentesPutRequest putRequest = new IncidentesPutRequest(){
            //    category_id= 1413, //NO ES OBLIGATORIO
            //    //title = "INCIDENTE DE PRUEBA 4 MODIFICACION postman- LIM", ////OBLIGATORIO
            //    //                priority_id = 2, //NO ES OBLIGATORIO
            //    //date_format = null, //NO ES OBLIGATORIO
            //    //customer_id = 1, //NO ES OBLIGATORIO
            //    //source_id = 1, //NONO ES OBLIGATORIO
            //    //id = 7086,  //OBLIGATORIO
            //    id = 7086,  //OBLIGATORIO
            //    //type_id= 3, //NO ES OBLIGATORIO
            //    //description = null, //NO ES OBLIGATORIO
            //    //location_id= null, //NO ES OBLIGATORIO
            //    //date = "1675183785", //NO ES OBLIGATORIO
            //    //reassignment = false
            //}
            //putRequest.category_id = 1833;
            //putRequest.title = "INCIDENTE DE PRUEBA MODIFICACION - LIM";
            //putRequest.priority_id = 5; // "id": 5,            "name": "Crítica"
            //putRequest.date_format = null;
            //putRequest.customer_id = 1;
            //putRequest.source_id = 1; //"id": 1,             "name": "Correo"
            //putRequest.id = 7081;
            //putRequest.type_id = 6; // "id": 6, "name": "Incidente mayor"
            //putRequest.description = "INCIDENTE DE PRUEBA MODIFICACION";
            //putRequest.location_id = null;
            //putRequest.date = "1675215295";
            //putRequest.reassignment = true;


            //IncidentesInvgate incidentes = new IncidentesInvgate();
            //Entities.Intermedio.Result respuesta = incidentes.PutIncidente(putRequest);




            //GET INCIDENTE COMMENT
            //IncidentesCommentGetRequest getRequest = new IncidentesCommentGetRequest();
            //getRequest.request_id = 7081;

            //IncidentesCommentInvgate instance = new IncidentesCommentInvgate();
            //var respuesta = instance.GetIncidenteComment(getRequest);


            ////POST INCIDENTE COMMENT
            //IncidentesCommentPostRequest postRequest = new IncidentesCommentPostRequest();
            //postRequest.is_solution = false;
            //postRequest.author_id = 1;
            //postRequest.request_id = 7091;
            //postRequest.customer_visible = true;
            //postRequest.comment = "COMENTARIO DOS - LIM";

            //IncidentesCommentInvgate comment = new IncidentesCommentInvgate();
            //Entities.Intermedio.Result respuesta = comment.PostIncidenteComment(postRequest);

            //GET INCIDENTE
            //CategoriastInvgate getRequest = new CategoriastInvgate();
            //getRequest.id = 7086;

            //CategoriastInvgate instance = new CategoriastInvgate();
            //INCOMPLETO POR ID object respuesta = instance.GetCategoriasProducto(1140);
            //COMPLETO POR ID
            //object respuesta = instance.GetNombresCategoriasByProductoId(1413);
            //object respuesta = instance.GetNombresCategoriasByProductoNombre("NA");

            //object respuesta = instance.GetIdsCategoriasByProductoId(1347);
            //object respuesta = instance.GetIdsCategoriasByProductoNombre("PLUS 5500");

            //Console.WriteLine(respuesta);




            //CategoriasProductoModel model = new CategoriasProductoModel();

            //model.Mesa = "MESA DE SERVICIO IMSS";
            //model.CatOperacion01 = "INFRAESTRUCTURA DE REDES Y TELECOMUNICACIONES";
            //model.CatOperacion02 = "REDES Y ENLACES";
            //model.CatOperacion03 = "SOPORTAR";
            //model.CatProducto01 = "HARDWARE";
            //model.CatProducto02 = "COMPUTO PERSONAL";
            //model.CatProducto03 = "ESCANER";
            //model.Producto = "TARJETA LOGICA";


            //// PRUEBAS
            ////string cadena = "MESA DE SERVICIO IMSS|INFRAESTRUCTURA DE REDES Y TELECOMUNICACIONES|REDES Y ENLACES|SOPORTAR|HARDWARE|COMPUTO PERSONAL|ESCANER|TARJETA LOGICA";
            ////string cadena = "MESA DE SERVICIO IMSS|INFRAESTRUCTURA DE REDES Y TELECOMUNICACIONES|TELEFONIA|SOPORTAR||||";
            //string cadena = "MESA DE SERVICIO IMSS|APLICACIONES|CORREO Y MENSAJERIA|DAR BAJA||||";

            //object respuesta = instance.GetNombresCategoriasByProductoNombreArreglo(cadena);

            Console.ReadKey();

        }




    }
}
