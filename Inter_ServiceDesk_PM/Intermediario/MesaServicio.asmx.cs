using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Services;
using Entities;
using Entities.Intermedio;
using ServiceInvgate;
using ServiceBitacora;
using Inter_ServiceDesk_PM.Helper;
using System.Web.Services.Protocols;
using Entities.Invgate;
using System.Configuration;


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
        public Autenticacion Autenticacion;
        IncidenteData bitacora = new IncidenteData();
        OrdenTrabajoData bitacoraWO = new OrdenTrabajoData();
        CatalogosData catalogos = new CatalogosData();
        IncidentesInvgate incidentes = new IncidentesInvgate();
        IncidentesCommentInvgate comments = new IncidentesCommentInvgate();
        Log log = new Log();
        int idCategorizacion = Convert.ToInt32(ConfigurationManager.AppSettings["MesaInvgate"]);
        private long ConvertToTimestamp(DateTime value)
        {
            long epoch = (value.Ticks - 621355968000000000) / 10000000;
            return epoch;
        }

        //private byte[] ObjectToByteArray(object obj)
        //{
        //    if (obj == null)
        //        return null;
        //    BinaryFormatter bf = new BinaryFormatter();
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        bf.Serialize(ms, obj);
        //        return ms.ToArray();
        //    }
        //}

        //public object Forbidden()
        //{
        //    Context.Response.Status = "403forbidden";
        //    Context.Response.StatusCode = 403;
        //    Context.ApplicationInstance.CompleteRequest();
        //    return null;
        //}

        [WebMethod]
        [SoapHeader("Autenticacion")]
        public Entities.Intermedio.Result Incidente_Add(CreaTicketIN request)
        {
            Entities.Intermedio.Result response_ = new Entities.Intermedio.Result();
            
            if (string.IsNullOrEmpty(request.VIP)) { request.VIP = "NO"; }
            log.LogCreaTicketIN(request);
            try
            {
                if (Autenticacion != null)
                {
                    if (Autenticacion.IsValid())
                    {
                        //Valida que no exista el ticket de IMSS
                        if (!bitacora.Existe(request.TicketIMSS))
                        {
                            //Otiene urgencia
                            int IdPrioridad = catalogos.GetUrgenciaInvgate(Convert.ToInt32(request.Urgencia));

                            string concat =  "\r\n" + "||Categoria:" +
                                    request.CategoriaOpe01 + "|" +
                                    request.CategoriaOpe02 + "|" +
                                    request.CategoriaOpe03 + "|" +
                                    request.CategoriaPro01 + "|" +
                                    request.CategoriaPro02 + "|" +
                                    request.CategoriaPro03 + "|" +
                                    request.NombreProducto;

                            IncidentesPostRequest VarInter = new IncidentesPostRequest();
                            VarInter.customer_id = 1;
                            VarInter.attachments = null;
                            //VarInter.date = ConvertToTimestamp(fecha).ToString();
                            VarInter.date = ConvertToTimestamp(request.FechaCreacion).ToString();
                            VarInter.related_to = null;
                            VarInter.priority_id = IdPrioridad;
                            VarInter.creator_id = 1240;
                            VarInter.type_id = 1;//Incidente
                            VarInter.category_id = idCategorizacion;
                            VarInter.description =  request.Descripcion + concat;
                            VarInter.title = request.TicketIMSS + " - " + request.Resumen;
                            VarInter.source_id = 2;

                            response_ = incidentes.PostIncidente(VarInter);

                            log.LogMsg("Ticket Invgate: " + response_.Ticket);
                            
                            if (response_.Estado == "Exito")
                            {
                                List<HttpPostedFileBase> files_ = new List<HttpPostedFileBase>();
                                #region BeforeFiles
                                //if (!string.IsNullOrEmpty(request.Adjunto01) && !string.IsNullOrEmpty(request.AdjuntoName01))
                                //{
                                //    byte[] img1 = request.Adjunto01 != String.Empty ? Convert.FromBase64String(request.Adjunto01) : null;

                                //    if (img1 != null)
                                //    {
                                //        files_.Add((HttpPostedFileBase)new MemoryPostedFile(img1, request.AdjuntoName01));
                                //    }
                                //}

                                //if (!string.IsNullOrEmpty(request.Adjunto02) && !string.IsNullOrEmpty(request.AdjuntoName02))
                                //{
                                //    byte[] img2 = request.Adjunto02 != String.Empty ? Convert.FromBase64String(request.Adjunto02) : null;
                                //    if (img2 != null)
                                //    {
                                //        files_.Add((HttpPostedFileBase)new MemoryPostedFile(img2, request.AdjuntoName02));
                                //    }
                                //}

                                //if (!string.IsNullOrEmpty(request.Adjunto02) && !string.IsNullOrEmpty(request.AdjuntoName02))
                                //{
                                //    byte[] img3 = request.Adjunto03 != String.Empty ? Convert.FromBase64String(request.Adjunto03) : null;

                                //    if (img3 != null)
                                //    {
                                //        files_.Add((HttpPostedFileBase)new MemoryPostedFile(img3, request.AdjuntoName03));
                                //    }
                                //}
                                #endregion

                                if (request.Adjunto01 != null && request.Adjunto01.Length > 0 && !string.IsNullOrEmpty(request.AdjuntoName01))
                                { 
                                    //Valida el nombre del archivo
                                    if(request.AdjuntoName01.Length > 50)
                                    {
                                        string[] nombre = request.AdjuntoName01.Split('.');
                                        request.AdjuntoName01 = nombre[0].Substring(0,45) + "." +nombre[1];
                                    }
                                    files_.Add(new MemoryPostedFile(request.Adjunto01, request.AdjuntoName01)); 
                                }

                                if (request.Adjunto02 != null && request.Adjunto02.Length > 0 && !string.IsNullOrEmpty(request.AdjuntoName02))
                                {
                                    //Valida el nombre del archivo
                                    if (request.AdjuntoName02.Length > 50)
                                    {
                                        string[] nombre = request.AdjuntoName02.Split('.');
                                        request.AdjuntoName02 = nombre[0].Substring(0, 45) + "." + nombre[1];
                                    }
                                    files_.Add(new MemoryPostedFile(request.Adjunto02, request.AdjuntoName02)); 
                                }

                                if (request.Adjunto03 != null && request.Adjunto03.Length > 0 && !string.IsNullOrEmpty(request.AdjuntoName03))
                                {
                                    //Valida el nombre del archivo
                                    if (request.AdjuntoName03.Length > 50)
                                    {
                                        string[] nombre = request.AdjuntoName03.Split('.');
                                        request.AdjuntoName03 = nombre[0].Substring(0, 45) + "." + nombre[1];
                                    }
                                    files_.Add(new MemoryPostedFile(request.Adjunto03, request.AdjuntoName03)); 
                                }

                                if (files_.Count > 0)
                                { incidentes.PostAttachments(files_.ToArray(), Convert.ToInt32(response_.Ticket)); }

                                //AgregaNotas
                                if (!string.IsNullOrEmpty(request.Notas))
                                {
                                    IncidentesCommentPostRequest VarComent = new IncidentesCommentPostRequest();

                                    VarComent.request_id = Convert.ToInt32(response_.Ticket);
                                    VarComent.comment = request.Notas;
                                    VarComent.author_id = 1240;
                                    VarComent.is_solution = false;

                                    response_ = comments.PostIncidenteComment(VarComent);
                                }

                                //Bitacora
                                bitacora.Crear(request, Convert.ToInt32(response_.Ticket), out string Result);
                            }
                        }
                        else
                        {
                            response_.Estado = "Error";
                            response_.Resultado = "El ticket " + request.TicketIMSS + " ya existe en la Mesa de Invgate, no es posible crearlo nuevamente.";
                        }
                    }
                    else
                    {
                        response_.Estado = "Error";
                        response_.Resultado = "Credenciales de acceso incorrectas.";
                    }
                }
                else
                {
                    response_.Estado = "Error";
                    response_.Resultado = "Es necesario ingresar credenciales de acceso.";
                }
            }
            catch (Exception ex)
            {
                response_.Estado = "Error";
                response_.Resultado = ex.Message;
                log.LogMsg("Error|Incident_Add: " + ex.Message);
            }

            return response_;
        }


        [WebMethod]
        [SoapHeader("Autenticacion")]
        public Entities.Intermedio.Result Incidente_Update(ActualizaTicketIN request)
        {
            Entities.Intermedio.Result response_ = new Entities.Intermedio.Result();

            try
            {
                if (Autenticacion != null)
                {
                    if (Autenticacion.IsValid())
                    {
                        int idTicketInvgate = 0;
                        ServiceBitacora.Incidente data = bitacora.GetIdInvgate(request.TicketIMSS);
                        if (data != null)
                        { idTicketInvgate = data.TicketInvgate == null ? 0 : Convert.ToInt32(data.TicketInvgate); }

                        if (idTicketInvgate > 0)
                        {
                            //Valida el estatus, si es Resuelto
                            if (request.EstadoNuevo == "4")//Resuelto
                            {
                                IncidentesCommentPostRequest VarComent = new IncidentesCommentPostRequest();

                                VarComent.request_id = idTicketInvgate;
                                VarComent.comment = request.Notas == string.Empty ? "Ticket Solucionado" : request.Notas;
                                VarComent.author_id = 2;
                                VarComent.is_solution = true;

                                response_ = comments.PostIncidenteComment(VarComent);
                            }
                            else
                            {
                                IncidentPutRequest VarInter = new IncidentPutRequest();
                                
                                //AgregaNotas
                                if (!string.IsNullOrEmpty(request.Notas))
                                {
                                    IncidentesCommentPostRequest VarComent = new IncidentesCommentPostRequest();

                                    VarComent.request_id = idTicketInvgate;
                                    VarComent.comment = request.Notas;
                                    VarComent.author_id = 1240;
                                    VarComent.is_solution = false;

                                    response_ = comments.PostIncidenteComment(VarComent);
                                }

                                //Obtiene urgencia
                                if (request.Urgencia != null)
                                {
                                    int IdPrioridad = catalogos.GetUrgenciaInvgate(Convert.ToInt32(request.Urgencia));
                                                                        
                                    VarInter.id = idTicketInvgate;
                                    VarInter.priorityId = IdPrioridad;

                                    response_ = incidentes.PutIncidentePriority(VarInter);
                                }

                                //Obtiene estatus
                                if (!string.IsNullOrEmpty(request.EstadoNuevo))
                                {
                                    int IdEstatus = catalogos.GetEstatusIncidenteInvgate(Convert.ToInt32(request.EstadoNuevo));

                                    VarInter.id = idTicketInvgate;
                                    VarInter.statusId = IdEstatus;

                                    response_ = incidentes.PutIncidenteStatus(VarInter);
                                }
                            }

                            //Bitacora
                            bitacora.ActualizaIncidente(request, idTicketInvgate, out string Result);

                            //Si no actualizamos nada en Invgate
                            if (string.IsNullOrEmpty(response_.Estado))
                            {
                                response_.Ticket = idTicketInvgate.ToString();
                                response_.Resultado = "Transacción exitosa, registro actualizado en Invgate People Media";
                                response_.Estado = "Exito";
                            }
                        }
                        else
                        {
                            response_.Estado = "Error";
                            response_.Resultado = "No se encontro Ticket solicitado, favor de validar";
                        }
                    }
                    else
                    {
                        response_.Estado = "Error";
                        response_.Resultado = "Credenciales de acceso incorrectas.";
                    }
                }
                else
                {
                    response_.Estado = "Error";
                    response_.Resultado = "Es necesario ingresar credenciales de acceso.";
                }
            }
            catch (Exception ex)
            {
                response_.Estado = "Error";
                response_.Resultado = ex.Message;
                log.LogMsg("Error|Incidente_Update: " + ex.Message);
            }

            return response_;
        }


        [WebMethod]
        [SoapHeader("Autenticacion")]
        public Entities.Intermedio.Result Incidente_Update_Prioridad(ActualizaPriorizacionIN request)
        {
            Entities.Intermedio.Result response_ = new Entities.Intermedio.Result();

            try
            {
                if (Autenticacion != null)
                {
                    if (Autenticacion.IsValid())
                    {
                        //Obtiene id Invgate
                        int idTicketInvgate = 0;
                        ServiceBitacora.Incidente data = bitacora.GetIdInvgate(request.TicketIMSS);
                        if (data != null)
                        { idTicketInvgate = data.TicketInvgate == null ? 0 : Convert.ToInt32(data.TicketInvgate); }

                        if (idTicketInvgate > 0)
                        {
                            IncidentPutRequest VarInter = new IncidentPutRequest();
                            //DateTime dt = Convert.ToDateTime(Convert.ToDateTime(request.FechaCambio.ToUpper().Replace("P.M", "").Replace("A.M", "")).ToShortDateString());

                            //Otiene urgencia
                            int IdPrioridad = catalogos.GetUrgenciaInvgate(Convert.ToInt32(request.Urgencia));

                            VarInter.id = idTicketInvgate;
                            VarInter.priorityId = IdPrioridad;

                            //VarInter.date = ConvertToTimestamp(dt).ToString();

                            response_ = incidentes.PutIncidentePriority(VarInter);

                            //Bitacora
                            bitacora.ActualizaPriorizacion(request, idTicketInvgate, out string Result);

                            //Si no actualizamos nada en Invgate
                            if (string.IsNullOrEmpty(response_.Estado))
                            {
                                response_.Ticket = idTicketInvgate.ToString();
                                response_.Resultado = "Transacción exitosa, registro actualizado en Invgate People Media";
                                response_.Estado = "Exito";
                            }
                        }
                        else
                        {
                            response_.Estado = "Error";
                            response_.Resultado = "No se encontro Ticket solicitado, favor de validar";
                        }
                    }
                    else
                    {
                        response_.Estado = "Error";
                        response_.Resultado = "Credenciales de acceso incorrectas.";
                    }
                }
                else
                {
                    response_.Estado = "Error";
                    response_.Resultado = "Es necesario ingresar credenciales de acceso.";
                }
            }
            catch (Exception ex)
            {
                response_.Estado = "Error";
                response_.Resultado = ex.Message;
                log.LogMsg("Error|Incidente_Update_Prioridad: " + ex.Message);
            }

            return response_;
        }


        [WebMethod]
        [SoapHeader("Autenticacion")]
        public Entities.Intermedio.Result Incidente_Update_Categorizacion(ActualizaCategorizacion request)
        {
            Entities.Intermedio.Result response_ = new Entities.Intermedio.Result();

            try
            {
                if (Autenticacion != null)
                {
                    if (Autenticacion.IsValid())
                    {
                        //Obtiene id Invgate
                        int idTicketInvgate = 0;
                        ServiceBitacora.Incidente data = bitacora.GetIdInvgate(request.TicketIMSS);
                        if (data != null)
                        { idTicketInvgate = data.TicketInvgate == null ? 0 : Convert.ToInt32(data.TicketInvgate); }
                        //idTicketInvgate = 7630;
                        if (idTicketInvgate > 0)
                        {
                            //Consulta incidente para actualizar descripcion
                            IncidentesGetRequest getRequest = new IncidentesGetRequest();
                            getRequest.id = idTicketInvgate;
                            dynamic incident = incidentes.GetIncidente(getRequest);

                            string descripcion = incident.description;

                            string[] principal = descripcion.Split(new[] { "||" }, StringSplitOptions.None); 

                            IncidentesPutRequest VarInter = new IncidentesPutRequest();
                            
                            //Obtiene Categoria
                            string concat = "\r\n" + "||Categoria:" +
                                    request.CategoriaOpe01 + "|" +
                                    request.CategoriaOpe02 + "|" +
                                    request.CategoriaOpe03 + "|" +
                                    request.CategoriaPro01 + "|" +
                                    request.CategoriaPro02 + "|" +
                                    request.CategoriaPro03 + "|" +
                                    data.NombreProducto;
               
                            VarInter.id = idTicketInvgate;
                            VarInter.description = principal[0] + concat;
                            
                            response_ = incidentes.PutIncidente(VarInter);

                            //Bitacora
                            bitacora.ActualizaCategoria(request, idTicketInvgate, out string Result);
                        }
                        else
                        {
                            response_.Estado = "Error";
                            response_.Resultado = "No se encontro Ticket solicitado, favor de validar";
                        }
                    }
                    else
                    {
                        response_.Estado = "Error";
                        response_.Resultado = "Credenciales de acceso incorrectas.";
                    }
                }
                else
                {
                    response_.Estado = "Error";
                    response_.Resultado = "Es necesario ingresar credenciales de acceso.";
                }
            }
            catch (Exception ex)
            {
                response_.Estado = "Error";
                response_.Resultado = ex.Message;
                log.LogMsg("Error|Incidente_Update_Categorizacion: " + ex.Message);
            }

            return response_;
        }

        [WebMethod]
        [SoapHeader("Autenticacion")]
        public Entities.Intermedio.Result Incidente_Add_Notas(AgregaNota request)
        {
            Entities.Intermedio.Result response_ = new Entities.Intermedio.Result();

            try
            {
                if (Autenticacion != null)
                {
                    if (Autenticacion.IsValid())
                    {
                        //Obtiene id Invgate
                        int idTicketInvgate = 0;
                        ServiceBitacora.Incidente data = bitacora.GetIdInvgate(request.TicketIMSS);
                        if (data != null)
                        { idTicketInvgate = data.TicketInvgate == null ? 0 : Convert.ToInt32(data.TicketInvgate); }

                        if (idTicketInvgate > 0)
                        {
                            //Valida si es una nota automatica
                            if (request.Notas.Contains("Status Marked"))
                            {
                                //Valida si es una reapertura de incidente mediante la nota
                                IncidentesGetRequest getRequest = new IncidentesGetRequest();
                                getRequest.id = idTicketInvgate;
                                dynamic valida = incidentes.GetIncidente(getRequest);

                                if(valida.status_id != null)
                                {
                                    int statusInvgate = Convert.ToInt32(valida.status_id);

                                    if(statusInvgate == 5 && request.Notas.Contains("Status Marked: In Progress"))
                                    {
                                        //Si el ticket esta Solucionado y se detecta nota de reapertura, actualiza el estatus
                                        IncidentPutRequest VarInter = new IncidentPutRequest(); 
                                        VarInter.id = idTicketInvgate;
                                        VarInter.statusId = 2;//Ticket Abierto

                                        response_ = incidentes.PutIncidenteStatus(VarInter);
                                    }
                                }

                                response_.Estado = "Exito";
                                response_.Resultado = "Comentario Automatico.";
                            }
                            else
                            {
                                //Agrega los adjuntos
                                List<HttpPostedFileBase> files_ = new List<HttpPostedFileBase>();
                                if (request.Adjunto01 != null && request.Adjunto01.Length > 0 && !string.IsNullOrEmpty(request.AdjuntoName01))
                                {
                                    //Valida el nombre del archivo
                                    if (request.AdjuntoName01.Length > 50)
                                    {
                                        string[] nombre = request.AdjuntoName01.Split('.');
                                        request.AdjuntoName01 = nombre[0].Substring(0, 45) + "." + nombre[1];
                                    }          
                                    files_.Add(new MemoryPostedFile(request.Adjunto01, request.AdjuntoName01)); 
                                }

                                if (request.Adjunto02 != null && request.Adjunto02.Length > 0 && !string.IsNullOrEmpty(request.AdjuntoName02))
                                {
                                    //Valida el nombre del archivo
                                    if (request.AdjuntoName02.Length > 50)
                                    {
                                        string[] nombre = request.AdjuntoName02.Split('.');
                                        request.AdjuntoName02 = nombre[0].Substring(0, 45) + "." + nombre[1];
                                    }
                                    files_.Add(new MemoryPostedFile(request.Adjunto02, request.AdjuntoName02)); 
                                }

                                if (request.Adjunto03 != null && request.Adjunto03.Length > 0 && !string.IsNullOrEmpty(request.AdjuntoName03))
                                {
                                    //Valida el nombre del archivo
                                    if (request.AdjuntoName03.Length > 50)
                                    {
                                        string[] nombre = request.AdjuntoName03.Split('.');
                                        request.AdjuntoName03 = nombre[0].Substring(0, 45) + "." + nombre[1];
                                    }
                                    files_.Add(new MemoryPostedFile(request.Adjunto03, request.AdjuntoName03)); 
                                }

                                if (files_.Count > 0)
                                { incidentes.PostAttachments(files_.ToArray(), idTicketInvgate); }

                                //Envia la nota
                                IncidentesCommentPostRequest VarInter = new IncidentesCommentPostRequest();

                                VarInter.request_id = idTicketInvgate;
                                VarInter.comment = request.Notas;
                                VarInter.author_id = 1240;
                                VarInter.is_solution = false;

                                response_ = comments.PostIncidenteComment(VarInter);

                                //Bitacora
                                bitacora.AgregaNota(request, idTicketInvgate, out string Result);
                            }
                        }
                        else
                        {
                            response_.Estado = "Error";
                            response_.Resultado = "No se encontro Ticket solicitado, favor de validar";
                        }
                    }
                    else
                    {
                        response_.Estado = "Error";
                        response_.Resultado = "Credenciales de acceso incorrectas.";
                    }
                }
                else
                {
                    response_.Estado = "Error";
                    response_.Resultado = "Es necesario ingresar credenciales de acceso.";
                }
            }
            catch (Exception ex)
            {
                response_.Estado = "Error";
                response_.Resultado = ex.Message;
                log.LogMsg("Error|Incidente_Add_Notas: " + ex.Message);
            }

            return response_;
        }


        [WebMethod]
        [SoapHeader("Autenticacion")]
        public Entities.Intermedio.Result Orden_Add(CreaTicketWO request)
        {
            Entities.Intermedio.Result response_ = new Entities.Intermedio.Result();

            if (string.IsNullOrEmpty(request.VIP)) { request.VIP = "NO"; }

            log.LogCreaTicketWO(request);
            try
            {
                if (Autenticacion != null)
                {
                    if (Autenticacion.IsValid())
                    {
                        //Valida que no exista el ticket de IMSS
                        if (!bitacoraWO.Existe(request.TicketIMSS))
                        {
                            //Otiene prioridad
                            //int IdPrioridad = catalogos.GetPrioridadInvgate(Convert.ToInt32(request.Prioridad));
                            int IdPrioridad = catalogos.GetPrioridadInvgate(request.Prioridad);

                            string concat = "\r\n" + "||Categoria:" +
                                    request.CategoriaOpe01 + "|" +
                                    request.CategoriaOpe02 + "|" +
                                    request.CategoriaOpe03 + "|" +
                                    request.CategoriaPro01 + "|" +
                                    request.CategoriaPro02 + "|" +
                                    request.CategoriaPro03 + "|" +
                                    request.NombreProducto;

                            IncidentesPostRequest VarInter = new IncidentesPostRequest();
                            VarInter.customer_id = 1;
                            VarInter.attachments = null;
                            //VarInter.date = ConvertToTimestamp(fecha).ToString();
                            VarInter.date = ConvertToTimestamp(request.FechaCreacion).ToString();
                            VarInter.related_to = null;
                            VarInter.priority_id = IdPrioridad;
                            VarInter.creator_id = 1240;
                            VarInter.type_id = 2; //Orden Trabajo
                            VarInter.category_id = idCategorizacion;
                            VarInter.description = request.Descripcion + concat;
                            VarInter.title = request.TicketIMSS + " - " + request.Resumen;
                            VarInter.source_id = 2;

                            response_ = incidentes.PostIncidente(VarInter);

                            if(response_.Estado == "Exito")
                            {
                                List<HttpPostedFileBase> files_ = new List<HttpPostedFileBase>();

                                if (request.Adjunto01 != null && request.Adjunto01.Length > 0 && !string.IsNullOrEmpty(request.AdjuntoName01))
                                {
                                    //Valida el nombre del archivo
                                    if (request.AdjuntoName01.Length > 50)
                                    {
                                        string[] nombre = request.AdjuntoName01.Split('.');
                                        request.AdjuntoName01 = nombre[0].Substring(0, 45) + "." + nombre[1];
                                    }
                                    files_.Add(new MemoryPostedFile(request.Adjunto01, request.AdjuntoName01)); 
                                }

                                if (request.Adjunto02 != null && request.Adjunto02.Length > 0 && !string.IsNullOrEmpty(request.AdjuntoName02))
                                {
                                    //Valida el nombre del archivo
                                    if (request.AdjuntoName02.Length > 50)
                                    {
                                        string[] nombre = request.AdjuntoName02.Split('.');
                                        request.AdjuntoName02 = nombre[0].Substring(0, 45) + "." + nombre[1];
                                    }
                                    files_.Add(new MemoryPostedFile(request.Adjunto02, request.AdjuntoName02)); 
                                }

                                if (request.Adjunto03 != null && request.Adjunto03.Length > 0 && !string.IsNullOrEmpty(request.AdjuntoName03))
                                {
                                    //Valida el nombre del archivo
                                    if (request.AdjuntoName03.Length > 50)
                                    {
                                        string[] nombre = request.AdjuntoName03.Split('.');
                                        request.AdjuntoName03 = nombre[0].Substring(0, 45) + "." + nombre[1];
                                    }
                                    files_.Add(new MemoryPostedFile(request.Adjunto03, request.AdjuntoName03)); 
                                }

                                if (files_.Count > 0)
                                { incidentes.PostAttachments(files_.ToArray(), Convert.ToInt32(response_.Ticket)); }

                                //AgregaNotas
                                if (!string.IsNullOrEmpty(request.Notas))
                                {
                                    IncidentesCommentPostRequest VarComent = new IncidentesCommentPostRequest();

                                    VarComent.request_id = Convert.ToInt32(response_.Ticket);
                                    VarComent.comment = request.Notas;
                                    VarComent.author_id = 1240;
                                    VarComent.is_solution = false;

                                    response_ = comments.PostIncidenteComment(VarComent);
                                }

                                //Bitacora
                                //IncidenteData bitacora = new IncidenteData();
                                bitacoraWO.Crear(request, Convert.ToInt32(response_.Ticket), out string Result);
                            }
                        }
                        else
                        {
                            response_.Estado = "Error";
                            response_.Resultado = "La Orden de Trabajo " + request.TicketIMSS + " ya existe en la Mesa de Invgate, no es posible crearla nuevamente.";
                        }
                    }
                    else
                    {
                        response_.Estado = "Error";
                        response_.Resultado = "Credenciales de acceso incorrectas.";
                    }
                }
                else
                {
                    response_.Estado = "Error";
                    response_.Resultado = "Es necesario ingresar credenciales de acceso.";
                }
            }
            catch (Exception ex)
            {
                response_.Estado = "Error";
                response_.Resultado = ex.Message;
                log.LogMsg("Error|Orden_Add: " + ex.Message);
            }

            return response_;
        }


        [WebMethod]
        [SoapHeader("Autenticacion")]
        public Entities.Intermedio.Result Orden_Update(ActualizaTicketWO request)
        {
            Entities.Intermedio.Result response_ = new Entities.Intermedio.Result();

            try
            {
                if (Autenticacion != null)
                {
                    if (Autenticacion.IsValid())
                    {
                        //Obtiene id Invgate
                        int idTicketInvgate = 0;
                        ServiceBitacora.OrdenTrabajo data = bitacoraWO.GetIdInvgate(request.TicketIMSS);
                        if (data != null)
                        { idTicketInvgate = data.TicketInvgate == null ? 0 : Convert.ToInt32(data.TicketInvgate); }

                        if (idTicketInvgate > 0)
                        {
                            //Valida el estatus, si es Terminado
                            if (request.EstadoNuevo == "5")//Terminado
                            {
                                IncidentesCommentPostRequest VarComent = new IncidentesCommentPostRequest();

                                VarComent.request_id = idTicketInvgate;
                                VarComent.comment = request.Notas == string.Empty ? "Solicitud Terminada" : request.Notas; ;
                                VarComent.author_id = 2;
                                VarComent.is_solution = true;

                                response_ = comments.PostIncidenteComment(VarComent);
                            }
                            else
                            {
                                IncidentPutRequest VarInter = new IncidentPutRequest();
                                
                                //AgregaNotas
                                if (!string.IsNullOrEmpty(request.Notas))
                                {
                                    IncidentesCommentPostRequest VarComent = new IncidentesCommentPostRequest();

                                    VarComent.request_id = idTicketInvgate;
                                    VarComent.comment = request.Notas;
                                    VarComent.author_id = 1240;
                                    VarComent.is_solution = false;

                                    response_ = comments.PostIncidenteComment(VarComent);
                                }

                                if (request.Prioridad != null)
                                {
                                    //Otiene prioridad
                                    //int IdPrioridad = catalogos.GetPrioridadInvgate(Convert.ToInt32(request.Prioridad));
                                    int IdPrioridad = catalogos.GetPrioridadInvgate(request.Prioridad);

                                    VarInter.id = idTicketInvgate; 
                                    VarInter.priorityId = IdPrioridad;
                                    //VarInter.date = ConvertToTimestamp(dt).ToString();

                                    response_ = incidentes.PutIncidentePriority(VarInter);
                                }

                                //Obtiene estatus
                                if (!string.IsNullOrEmpty(request.EstadoNuevo))
                                {
                                    int IdEstatus = catalogos.GetEstatusWOInvgate(Convert.ToInt32(request.EstadoNuevo));

                                    VarInter.id = idTicketInvgate;
                                    VarInter.statusId = IdEstatus;

                                    response_ = incidentes.PutIncidenteStatus(VarInter);
                                }
                            }

                            //Bitacora
                            bitacoraWO.ActualizaOrdenTrabajo(request, idTicketInvgate, out string Result);

                            //Si no actualizamos nada en Invgate
                            if (string.IsNullOrEmpty(response_.Estado))
                            {
                                response_.Ticket = idTicketInvgate.ToString();
                                response_.Resultado = "Transacción exitosa, registro actualizado en Invgate People Media";
                                response_.Estado = "Exito";
                            }
                        }
                        else
                        {
                            response_.Estado = "Error";
                            response_.Resultado = "No se encontro Ticket solicitado, favor de validar";
                        }
                    }
                    else
                    {
                        response_.Estado = "Error";
                        response_.Resultado = "Credenciales de acceso incorrectas.";
                    }
                }
                else
                {
                    response_.Estado = "Error";
                    response_.Resultado = "Es necesario ingresar credenciales de acceso.";
                }
            }
            catch (Exception ex)
            {
                response_.Estado = "Error";
                response_.Resultado = ex.Message;
                log.LogMsg("Error|Orden_Update: " + ex.Message);
            }

            return response_;
        }


        [WebMethod]
        [SoapHeader("Autenticacion")]
        public Entities.Intermedio.Result Orden_Update_Prioridad(ActualizaPriorizacionWO request)
        {
            Entities.Intermedio.Result response_ = new Entities.Intermedio.Result();

            try
            {
                if (Autenticacion != null)
                {
                    if (Autenticacion.IsValid())
                    {
                        //Obtiene id Invgate
                        int idTicketInvgate = 0;
                        ServiceBitacora.OrdenTrabajo data = bitacoraWO.GetIdInvgate(request.TicketIMSS);
                        if (data != null)
                        { idTicketInvgate = data.TicketInvgate == null ? 0 : Convert.ToInt32(data.TicketInvgate); }

                        if (idTicketInvgate > 0)
                        {
                            IncidentPutRequest VarInter = new IncidentPutRequest();

                            //Otiene prioridad
                            //int IdPrioridad = catalogos.GetPrioridadInvgate(Convert.ToInt32(request.Prioridad));
                            int IdPrioridad = catalogos.GetPrioridadInvgate(request.Prioridad);

                            VarInter.id = idTicketInvgate;
                            VarInter.priorityId = IdPrioridad;

                            response_ = incidentes.PutIncidentePriority(VarInter);

                            //Bitacora
                            bitacoraWO.ActualizaPriorizacion(request, idTicketInvgate, out string Result);

                            //Si no actualizamos nada en Invgate
                            if (string.IsNullOrEmpty(response_.Estado))
                            {
                                response_.Ticket = idTicketInvgate.ToString();
                                response_.Resultado = "Transacción exitosa, registro actualizado en Invgate People Media";
                                response_.Estado = "Exito";
                            }
                        }
                        else
                        {
                            response_.Estado = "Error";
                            response_.Resultado = "No se encontro Ticket solicitado, favor de validar";
                        }
                    }
                    else
                    {
                        response_.Estado = "Error";
                        response_.Resultado = "Credenciales de acceso incorrectas.";
                    }
                }
                else
                {
                    response_.Estado = "Error";
                    response_.Resultado = "Es necesario ingresar credenciales de acceso.";
                }
            }
            catch (Exception ex)
            {
                response_.Estado = "Error";
                response_.Resultado = ex.Message;
                log.LogMsg("Error|Orden_Update_Prioridad: " + ex.Message);
            }

            return response_;
        }


        [WebMethod]
        [SoapHeader("Autenticacion")]
        public Entities.Intermedio.Result Orden_Update_Categorizacion(ActualizaCategorizacion request)
        {
            Entities.Intermedio.Result response_ = new Entities.Intermedio.Result();

            try
            {
                if (Autenticacion != null)
                {
                    if (Autenticacion.IsValid())
                    {
                        //Obtiene id Invgate
                        int idTicketInvgate = 0;
                        ServiceBitacora.OrdenTrabajo data = bitacoraWO.GetIdInvgate(request.TicketIMSS);
                        if (data != null)
                        { idTicketInvgate = data.TicketInvgate == null ? 0 : Convert.ToInt32(data.TicketInvgate); }

                        if (idTicketInvgate > 0)
                        {
                            //Consulta incidente para actualizar descripcion
                            IncidentesGetRequest getRequest = new IncidentesGetRequest();
                            getRequest.id = idTicketInvgate;
                            dynamic incident = incidentes.GetIncidente(getRequest);

                            string descripcion = incident.description;

                            string[] principal = descripcion.Split(new[] { "||" }, StringSplitOptions.None);

                            IncidentesPutRequest VarInter = new IncidentesPutRequest();

                            //Obtiene Categoria
                            string concat = "\r\n" + "||Categoria:" +
                                    request.CategoriaOpe01 + "|" +
                                    request.CategoriaOpe02 + "|" +
                                    request.CategoriaOpe03 + "|" +
                                    request.CategoriaPro01 + "|" +
                                    request.CategoriaPro02 + "|" +
                                    request.CategoriaPro03 + "|" +
                                    data.NombreProducto;

                            VarInter.id = idTicketInvgate;
                            VarInter.description = principal[0] + concat;

                            response_ = incidentes.PutIncidente(VarInter);

                            //Bitacora
                            bitacoraWO.ActualizaCategoria(request, idTicketInvgate, out string Result);
                        }
                        else
                        {
                            response_.Estado = "Error";
                            response_.Resultado = "No se encontro Ticket solicitado, favor de validar";
                        }
                    }
                    else
                    {
                        response_.Estado = "Error";
                        response_.Resultado = "Credenciales de acceso incorrectas.";
                    }
                }
                else
                {
                    response_.Estado = "Error";
                    response_.Resultado = "Es necesario ingresar credenciales de acceso.";
                }
            }
            catch (Exception ex)
            {
                response_.Estado = "Error";
                response_.Resultado = ex.Message;
                log.LogMsg("Error|Orden_Update_Categorizacion: " + ex.Message);
            }

            return response_;

        }

        [WebMethod]
        [SoapHeader("Autenticacion")]
        public Entities.Intermedio.Result Orden_Add_Nota(AgregaNota request)
        {
            Entities.Intermedio.Result response_ = new Entities.Intermedio.Result();

            try
            {
                if (Autenticacion != null)
                {
                    if (Autenticacion.IsValid())
                    {
                        //Obtiene id Invgate
                        int idTicketInvgate = 0;
                        ServiceBitacora.OrdenTrabajo data = bitacoraWO.GetIdInvgate(request.TicketIMSS);
                        if (data != null)
                        { idTicketInvgate = data.TicketInvgate == null ? 0 : Convert.ToInt32(data.TicketInvgate); }

                        if (idTicketInvgate > 0)
                        {
                            //Valida si es una nota automatica
                            if (request.Notas.Contains("Status Marked"))
                            {
                                //Valida si es una reapertura de orden mediante la nota
                                IncidentesGetRequest getRequest = new IncidentesGetRequest();
                                getRequest.id = idTicketInvgate;
                                dynamic valida = incidentes.GetIncidente(getRequest);

                                if (valida.status_id != null)
                                {
                                    int statusInvgate = Convert.ToInt32(valida.status_id);

                                    if (statusInvgate == 5 && request.Notas.Contains("Status Marked: In Progress"))
                                    {
                                        //Si el ticket esta Solucionado y se detecta nota de reapertura, actualiza el estatus
                                        IncidentPutRequest VarInter = new IncidentPutRequest();
                                        VarInter.id = idTicketInvgate;
                                        VarInter.statusId = 2;//Ticket Abierto

                                        response_ = incidentes.PutIncidenteStatus(VarInter);
                                    }
                                }

                                response_.Estado = "Exito";
                                response_.Resultado = "Comentario Automatico.";
                            }
                            else
                            {
                                //Agrega adjuntos
                                List<HttpPostedFileBase> files_ = new List<HttpPostedFileBase>();

                                if (request.Adjunto01 != null && request.Adjunto01.Length > 0 && !string.IsNullOrEmpty(request.AdjuntoName01))
                                {
                                    //Valida el nombre del archivo
                                    if (request.AdjuntoName01.Length > 50)
                                    {
                                        string[] nombre = request.AdjuntoName01.Split('.');
                                        request.AdjuntoName01 = nombre[0].Substring(0, 45) + "." + nombre[1];
                                    }
                                    files_.Add(new MemoryPostedFile(request.Adjunto01, request.AdjuntoName01)); 
                                }

                                if (request.Adjunto02 != null && request.Adjunto02.Length > 0 && !string.IsNullOrEmpty(request.AdjuntoName02))
                                {
                                    //Valida el nombre del archivo
                                    if (request.AdjuntoName02.Length > 50)
                                    {
                                        string[] nombre = request.AdjuntoName02.Split('.');
                                        request.AdjuntoName02 = nombre[0].Substring(0, 45) + "." + nombre[1];
                                    }
                                    files_.Add(new MemoryPostedFile(request.Adjunto02, request.AdjuntoName02)); 
                                }

                                if (request.Adjunto03 != null && request.Adjunto03.Length > 0 && !string.IsNullOrEmpty(request.AdjuntoName03))
                                {
                                    //Valida el nombre del archivo
                                    if (request.AdjuntoName03.Length > 50)
                                    {
                                        string[] nombre = request.AdjuntoName03.Split('.');
                                        request.AdjuntoName03 = nombre[0].Substring(0, 45) + "." + nombre[1];
                                    }
                                    files_.Add(new MemoryPostedFile(request.Adjunto03, request.AdjuntoName03)); 
                                }

                                if (files_.Count > 0)
                                { incidentes.PostAttachments(files_.ToArray(), idTicketInvgate); }

                                //Agrega nota
                                IncidentesCommentPostRequest VarInter = new IncidentesCommentPostRequest();

                                VarInter.request_id = idTicketInvgate;
                                VarInter.comment = request.Notas;
                                VarInter.author_id = 1240;
                                VarInter.is_solution = false;

                                response_ = comments.PostIncidenteComment(VarInter);

                                //Bitacora
                                bitacoraWO.AgregaNota(request, idTicketInvgate, out string Result);

                            }
                        }
                        else
                        {
                            response_.Estado = "Error";
                            response_.Resultado = "No se encontro Ticket solicitado, favor de validar";
                        }                        
                    }
                    else
                    {
                        response_.Estado = "Error";
                        response_.Resultado = "Credenciales de acceso incorrectas.";
                    }
                }
                else
                {
                    response_.Estado = "Error";
                    response_.Resultado = "Es necesario ingresar credenciales de acceso.";
                }
            }
            catch (Exception ex)
            {
                response_.Estado = "Error";
                response_.Resultado = ex.Message;
                log.LogMsg("Error|Orden_Add_Nota: " + ex.Message);
            }

            return response_;
        }


    }
}
