using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Description;
using Entities;
using Entities.Intermedio;
using ServiceInvgate;
namespace Inter_ServiceDesk_PM
{
    /// <summary>
    /// Summary description for MesaServicio
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class MemoryPostedFile : HttpPostedFileBase
    {
        private readonly byte[] fileBytes;

        public MemoryPostedFile(byte[] fileBytes, string fileName = null)
        {
            this.fileBytes = fileBytes;
            this.FileName = fileName;
            this.InputStream = new MemoryStream(fileBytes);
        }

        public override int ContentLength => fileBytes.Length;

        public override string FileName { get; }

        public override Stream InputStream { get; }
    }
    public class MesaServicio : System.Web.Services.WebService
    {
        private long ConvertToTimestamp(DateTime value)
        {
            long epoch = (value.Ticks - 621355968000000000) / 10000000;
            return epoch;
        }

        [WebMethod]
        public Entities.Intermedio.Result AddIncidente(CreaTicket request)
        {
            IncidentesPostRequest VarInter = new IncidentesPostRequest();
            DateTime dt =  Convert.ToDateTime(Convert.ToDateTime(request.FechaCreacion.ToUpper().Replace("P.M","").Replace("A.M", "")).ToShortDateString());
            VarInter.customer_id = 1;
            VarInter.attachments = null;
            VarInter.date = ConvertToTimestamp(dt).ToString();
            VarInter.related_to = null;
            VarInter.priority_id = 1;
            VarInter.creator_id = 1240;
            VarInter.type_id = 1;
            CategoriastInvgate ci = new CategoriastInvgate();
            String concat = "MESA DE SERVICIO IMSS" + "|" +
                    request.CategoriaOpe01 + "|" +
                    request.CategoriaOpe02 + "|" +
                    request.CategoriaOpe03 + "|" +
                    request.CategoriaPro01 + "|" +
                    request.CategoriaPro02 + "|" +
                    request.CategoriaPro03 + "|" +
                    request.NombreProducto ;
            ci.GetNombresCategoriasByProductoNombreArreglo(concat);

            ResultCategorias respuesta = ci.GetNombresCategoriasByProductoNombreArreglo(concat);
            for (int i = (respuesta.categorias.Count - 1); i >= 0; i--)
            {

                if (respuesta.categorias[i].idCategoria != null)
                {
                    VarInter.category_id = (int)respuesta.categorias[i].idCategoria;
                    break;
                }
            }

            VarInter.description = request.Descripcion;
            VarInter.title = request.Resumen;
            VarInter.source_id = 2;
            IncidentesInvgate incidentes = new IncidentesInvgate();
            Entities.Intermedio.Result response_ = incidentes.PostIncidente(VarInter);
            ///////////Attachments
            ///
            //incidentes.PostAttachments()
            List<HttpPostedFileBase> files_ = new List<HttpPostedFileBase>();
            byte[] img1 = request.Adjunto01 != String.Empty ? Convert.FromBase64String(request.Adjunto01) : null;
            byte[] img2 = request.Adjunto02 != String.Empty ? Convert.FromBase64String(request.Adjunto02) : null;
            byte[] img3 = request.Adjunto03 != String.Empty ? Convert.FromBase64String(request.Adjunto03) : null;



            if (img1 != null)
            {
                files_.Add((HttpPostedFileBase)new MemoryPostedFile(img1,request.AdjuntoName01));
            }
            if (img2 != null)
            {
                files_.Add((HttpPostedFileBase)new MemoryPostedFile(img2, request.AdjuntoName02));
            }
            if (img3 != null)
            {
                files_.Add((HttpPostedFileBase)new MemoryPostedFile(img3, request.AdjuntoName03));
            }

            incidentes.PostAttachments(files_.ToArray(), Convert.ToInt32( response_.Ticket));

            return response_;
        }
    }
}
