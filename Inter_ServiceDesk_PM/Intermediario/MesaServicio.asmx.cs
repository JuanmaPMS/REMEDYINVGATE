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
using ServiceBitacora;
using Inter_ServiceDesk_PM.Helper;
using System.Web.Services.Protocols;

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
        private long ConvertToTimestamp(DateTime value)
        {
            long epoch = (value.Ticks - 621355968000000000) / 10000000;
            return epoch;
        }

        //public object Forbidden()
        //{
        //    Context.Response.Status = "403forbidden";
        //    Context.Response.StatusCode = 403;
        //    Context.ApplicationInstance.CompleteRequest();
        //    return null;
        //}

        [WebMethod]
        [SoapHeader("Autenticacion")]
        public Entities.Intermedio.Result Incidente_Add(CreaTicket request)
        {
            Entities.Intermedio.Result response_ = new Entities.Intermedio.Result();

            try
            {
                if (Autenticacion != null)
                {
                    if (Autenticacion.IsValid())
                    {
                        //Valida que no exista el ticket de IMSS
                        if (!bitacora.Existe(request.TicketIMSS))
                        {
                            IncidentesPostRequest VarInter = new IncidentesPostRequest();
                            DateTime dt = Convert.ToDateTime(Convert.ToDateTime(request.FechaCreacion.ToUpper().Replace("P.M", "").Replace("A.M", "")).ToShortDateString());
                            VarInter.customer_id = 1;
                            VarInter.attachments = null;
                            VarInter.date = ConvertToTimestamp(dt).ToString();
                            VarInter.related_to = null;
                            VarInter.priority_id = 1;
                            VarInter.creator_id = 1240;
                            VarInter.type_id = 1;//Incidente
                            CategoriastInvgate ci = new CategoriastInvgate();
                            string concat = "CAT PROD" + "|" +//"MESA DE SERVICIO IMSS" + "|" +
                                    request.CategoriaOpe01 + "|" +
                                    request.CategoriaOpe02 + "|" +
                                    request.CategoriaOpe03 + "|" +
                                    request.CategoriaPro01 + "|" +
                                    request.CategoriaPro02 + "|" +
                                    request.CategoriaPro03 + "|" +
                                    request.NombreProducto;
                            
                            VarInter.category_id = ci.GetCategoria(concat);
                            VarInter.description = request.Descripcion;
                            VarInter.title = request.Resumen;
                            VarInter.source_id = 2;

                            response_ = incidentes.PostIncidente(VarInter);
                            ///////////Attachments
                            ///
                            //incidentes.PostAttachments()
                            List<HttpPostedFileBase> files_ = new List<HttpPostedFileBase>();
                            byte[] img1 = request.Adjunto01 != String.Empty ? Convert.FromBase64String(request.Adjunto01) : null;
                            byte[] img2 = request.Adjunto02 != String.Empty ? Convert.FromBase64String(request.Adjunto02) : null;
                            byte[] img3 = request.Adjunto03 != String.Empty ? Convert.FromBase64String(request.Adjunto03) : null;



                            if (img1 != null)
                            {
                                files_.Add((HttpPostedFileBase)new MemoryPostedFile(img1, request.AdjuntoName01));
                            }
                            if (img2 != null)
                            {
                                files_.Add((HttpPostedFileBase)new MemoryPostedFile(img2, request.AdjuntoName02));
                            }
                            if (img3 != null)
                            {
                                files_.Add((HttpPostedFileBase)new MemoryPostedFile(img3, request.AdjuntoName03));
                            }

                            incidentes.PostAttachments(files_.ToArray(), Convert.ToInt32(response_.Ticket));

                            //AgregaNotas
                            if (!string.IsNullOrEmpty(request.Notas))
                            {
                                IncidentesCommentPostRequest VarComent = new IncidentesCommentPostRequest();

                                VarComent.request_id = Convert.ToInt32(response_.Ticket);
                                VarComent.comment = request.Notas;
                                VarComent.author_id = 1;
                                VarComent.is_solution = false;

                                response_ = comments.PostIncidenteComment(VarComent);
                            }

                            //Bitacora
                            //IncidenteData bitacora = new IncidenteData();
                            bitacora.Crear(request, Convert.ToInt32(response_.Ticket), out string Result);

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
            }
            
            return response_;
        }


        [WebMethod]
        [SoapHeader("Autenticacion")]
        public Entities.Intermedio.Result Incidente_Update(ActualizaTicket request)
        {
            Entities.Intermedio.Result response_ = new Entities.Intermedio.Result();

            try
            {
                if (Autenticacion != null)
                {
                    if (Autenticacion.IsValid())
                    {
                        //Obtiene id Invgate
                        Ticket data = bitacora.Get(request.TicketIMSS);

                        if (data.TicketInvgate > 0)
                        {
                            //Valida el estatus, si es Resuelto
                            if (request.EstadoNuevo == "4")//Resuelto
                            {
                                IncidentesCommentPostRequest VarComent = new IncidentesCommentPostRequest();

                                VarComent.request_id = data.TicketInvgate;
                                VarComent.comment = request.Notas;
                                VarComent.author_id = 1;
                                VarComent.is_solution = true;

                                response_ = comments.PostIncidenteComment(VarComent);
                            }
                            else
                            {
                                IncidentesPutRequest VarInter = new IncidentesPutRequest();
                                DateTime dt = Convert.ToDateTime(Convert.ToDateTime(request.FechaCambio.ToUpper().Replace("P.M", "").Replace("A.M", "")).ToShortDateString());

                                //Otiene urgencia
                                if (!string.IsNullOrEmpty(request.Urgencia))
                                {
                                    int IdPrioridad = catalogos.GetUrgencia(Convert.ToInt32(request.Urgencia));

                                    VarInter.priority_id = IdPrioridad;
                                    VarInter.id = data.TicketInvgate;
                                    VarInter.date = ConvertToTimestamp(dt).ToString();

                                    response_ = incidentes.PutIncidente(VarInter);
                                }

                                //AgregaNotas
                                if (!string.IsNullOrEmpty(request.Notas))
                                {
                                    IncidentesCommentPostRequest VarComent = new IncidentesCommentPostRequest();

                                    VarComent.request_id = Convert.ToInt32(response_.Ticket);
                                    VarComent.comment = request.Notas;
                                    VarComent.author_id = 1;
                                    VarComent.is_solution = false;

                                    response_ = comments.PostIncidenteComment(VarComent);
                                }

                            }

                            //Bitacora
                            bitacora.ActualizaIncidente(request, data.TicketInvgate, out string Result);

                            //Si no actualizamos nada en Invgate
                            if (string.IsNullOrEmpty(response_.Estado))
                            {
                                response_.Ticket = data.TicketInvgate.ToString();
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
            }

            return response_;
        }


        [WebMethod]
        [SoapHeader("Autenticacion")]
        public Entities.Intermedio.Result Incidente_Update_Prioridad(ActualizaPriorizacion request)
        {
            Entities.Intermedio.Result response_ = new Entities.Intermedio.Result();

            try
            {
                if (Autenticacion != null)
                {
                    if (Autenticacion.IsValid())
                    {
                        //Obtiene id Invgate
                        Ticket data = bitacora.Get(request.TicketIMSS);

                        if (data.TicketInvgate > 0)
                        {
                            IncidentesPutRequest VarInter = new IncidentesPutRequest();
                            DateTime dt = Convert.ToDateTime(Convert.ToDateTime(request.FechaCambio.ToUpper().Replace("P.M", "").Replace("A.M", "")).ToShortDateString());

                            if(!string.IsNullOrEmpty(request.Urgencia))
                            {
                                //Otiene urgencia
                                int IdPrioridad = catalogos.GetUrgencia(Convert.ToInt32(request.Urgencia));

                                VarInter.priority_id = IdPrioridad;
                                VarInter.id = data.TicketInvgate;
                                VarInter.date = ConvertToTimestamp(dt).ToString();

                                response_ = incidentes.PutIncidente(VarInter);
                            }               

                            //Bitacora
                            bitacora.ActualizaPriorizacion(request, data.TicketInvgate, out string Result);

                            //Si no actualizamos nada en Invgate
                            if (string.IsNullOrEmpty(response_.Estado))
                            {
                                response_.Ticket = data.TicketInvgate.ToString();
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
                        Ticket data = bitacora.Get(request.TicketIMSS);

                        if (data.TicketInvgate > 0)
                        {
                            IncidentesPutRequest VarInter = new IncidentesPutRequest();
                            DateTime dt = Convert.ToDateTime(Convert.ToDateTime(request.FechaCambio.ToUpper().Replace("P.M", "").Replace("A.M", "")).ToShortDateString());

                            //Obtiene Categoria
                            CategoriastInvgate ci = new CategoriastInvgate();
                            string concat = "CAT PROD" + "|" +//"MESA DE SERVICIO IMSS" + "|" +
                                    request.CategoriaOpe01 + "|" +
                                    request.CategoriaOpe02 + "|" +
                                    request.CategoriaOpe03 + "|" +
                                    request.CategoriaPro01 + "|" +
                                    request.CategoriaPro02 + "|" +
                                    request.CategoriaPro03 + "|" +
                                    data.NombreProducto;

                            VarInter.category_id = ci.GetCategoria(concat);
                            VarInter.id = data.TicketInvgate;
                            VarInter.date = ConvertToTimestamp(dt).ToString();

                            response_ = incidentes.PutIncidente(VarInter);

                            //Bitacora
                            bitacora.ActualizaCategoria(request, data.TicketInvgate, out string Result);
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
                        Ticket data = bitacora.Get(request.TicketIMSS);

                        if (data.TicketInvgate > 0)
                        {
                            IncidentesCommentPostRequest VarInter = new IncidentesCommentPostRequest();

                            VarInter.request_id = data.TicketInvgate;
                            VarInter.comment = request.Notas;
                            VarInter.author_id = 1;
                            VarInter.is_solution = false;

                            response_ = comments.PostIncidenteComment(VarInter);

                            //Bitacora
                            bitacora.AgregaNota(request, data.TicketInvgate, out string Result);
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
            }

            return response_;
        }


        [WebMethod]
        [SoapHeader("Autenticacion")]
        public Entities.Intermedio.Result Orden_Add(CreaTicket request)
        {
            Entities.Intermedio.Result response_ = new Entities.Intermedio.Result();

            try
            {
                if (Autenticacion != null)
                {
                    if (Autenticacion.IsValid())
                    {
                        //Valida que no exista el ticket de IMSS
                        if (!bitacoraWO.Existe(request.TicketIMSS))
                        {
                            IncidentesPostRequest VarInter = new IncidentesPostRequest();
                            DateTime dt = Convert.ToDateTime(Convert.ToDateTime(request.FechaCreacion.ToUpper().Replace("P.M", "").Replace("A.M", "")).ToShortDateString());
                            VarInter.customer_id = 1;
                            VarInter.attachments = null;
                            VarInter.date = ConvertToTimestamp(dt).ToString();
                            VarInter.related_to = null;
                            VarInter.priority_id = 1;
                            VarInter.creator_id = 1240;
                            VarInter.type_id = 2; //Orden Trabajo
                            CategoriastInvgate ci = new CategoriastInvgate();
                            string concat = "CAT PROD" + "|" +//"MESA DE SERVICIO IMSS" + "|" +
                                    request.CategoriaOpe01 + "|" +
                                    request.CategoriaOpe02 + "|" +
                                    request.CategoriaOpe03 + "|" +
                                    request.CategoriaPro01 + "|" +
                                    request.CategoriaPro02 + "|" +
                                    request.CategoriaPro03 + "|" +
                                    request.NombreProducto;

                            VarInter.category_id = ci.GetCategoria(concat);
                            VarInter.description = request.Descripcion;
                            VarInter.title = request.Resumen;
                            VarInter.source_id = 2;

                            response_ = incidentes.PostIncidente(VarInter);
                            ///////////Attachments
                            ///
                            //incidentes.PostAttachments()
                            List<HttpPostedFileBase> files_ = new List<HttpPostedFileBase>();
                            byte[] img1 = request.Adjunto01 != String.Empty ? Convert.FromBase64String(request.Adjunto01) : null;
                            byte[] img2 = request.Adjunto02 != String.Empty ? Convert.FromBase64String(request.Adjunto02) : null;
                            byte[] img3 = request.Adjunto03 != String.Empty ? Convert.FromBase64String(request.Adjunto03) : null;



                            if (img1 != null)
                            {
                                files_.Add((HttpPostedFileBase)new MemoryPostedFile(img1, request.AdjuntoName01));
                            }
                            if (img2 != null)
                            {
                                files_.Add((HttpPostedFileBase)new MemoryPostedFile(img2, request.AdjuntoName02));
                            }
                            if (img3 != null)
                            {
                                files_.Add((HttpPostedFileBase)new MemoryPostedFile(img3, request.AdjuntoName03));
                            }

                            incidentes.PostAttachments(files_.ToArray(), Convert.ToInt32(response_.Ticket));

                            //AgregaNotas
                            if (!string.IsNullOrEmpty(request.Notas))
                            {
                                IncidentesCommentPostRequest VarComent = new IncidentesCommentPostRequest();

                                VarComent.request_id = Convert.ToInt32(response_.Ticket);
                                VarComent.comment = request.Notas;
                                VarComent.author_id = 1;
                                VarComent.is_solution = false;

                                response_ = comments.PostIncidenteComment(VarComent);
                            }

                            //Bitacora
                            //IncidenteData bitacora = new IncidenteData();
                            bitacoraWO.Crear(request, Convert.ToInt32(response_.Ticket), out string Result);
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
            }

            return response_;
        }


        [WebMethod]
        [SoapHeader("Autenticacion")]
        public Entities.Intermedio.Result Orden_Update(ActualizaTicket request)
        {
            Entities.Intermedio.Result response_ = new Entities.Intermedio.Result();

            try
            {
                if (Autenticacion != null)
                {
                    if (Autenticacion.IsValid())
                    {
                        //Obtiene id Invgate
                        Ticket data = bitacoraWO.Get(request.TicketIMSS);

                        if (data.TicketInvgate > 0)
                        {
                            //Valida el estatus, si es Terminado
                            if (request.EstadoNuevo == "5")//Terminado
                            {
                                IncidentesCommentPostRequest VarComent = new IncidentesCommentPostRequest();

                                VarComent.request_id = data.TicketInvgate;
                                VarComent.comment = request.Notas;
                                VarComent.author_id = 1;
                                VarComent.is_solution = true;

                                response_ = comments.PostIncidenteComment(VarComent);
                            }
                            else
                            {
                                IncidentesPutRequest VarInter = new IncidentesPutRequest();
                                DateTime dt = Convert.ToDateTime(Convert.ToDateTime(request.FechaCambio.ToUpper().Replace("P.M", "").Replace("A.M", "")).ToShortDateString());

                                if(!string.IsNullOrEmpty(request.Prioridad))
                                {
                                    //Otiene urgencia
                                    int IdPrioridad = catalogos.GetPrioridad(Convert.ToInt32(request.Prioridad));

                                    VarInter.priority_id = IdPrioridad;
                                    VarInter.id = data.TicketInvgate;
                                    VarInter.date = ConvertToTimestamp(dt).ToString();

                                    response_ = incidentes.PutIncidente(VarInter);
                                }                            

                                //AgregaNotas
                                if (!string.IsNullOrEmpty(request.Notas))
                                {
                                    IncidentesCommentPostRequest VarComent = new IncidentesCommentPostRequest();

                                    VarComent.request_id = Convert.ToInt32(response_.Ticket);
                                    VarComent.comment = request.Notas;
                                    VarComent.author_id = 1;
                                    VarComent.is_solution = false;

                                    response_ = comments.PostIncidenteComment(VarComent);
                                }
                            }

                            //Bitacora
                            bitacoraWO.ActualizaOrdenTrabajo(request, data.TicketInvgate, out string Result);

                            //Si no actualizamos nada en Invgate
                            if (string.IsNullOrEmpty(response_.Estado))
                            {
                                response_.Ticket = data.TicketInvgate.ToString();
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
            }

            return response_;
        }


        [WebMethod]
        [SoapHeader("Autenticacion")]
        public Entities.Intermedio.Result Orden_Update_Prioridad(ActualizaPriorizacion request)
        {
            Entities.Intermedio.Result response_ = new Entities.Intermedio.Result();

            try
            {
                if (Autenticacion != null)
                {
                    if (Autenticacion.IsValid())
                    {
                        //Obtiene id Invgate
                        Ticket data = bitacoraWO.Get(request.TicketIMSS);

                        if (data.TicketInvgate > 0)
                        {
                            IncidentesPutRequest VarInter = new IncidentesPutRequest();
                            DateTime dt = Convert.ToDateTime(Convert.ToDateTime(request.FechaCambio.ToUpper().Replace("P.M", "").Replace("A.M", "")).ToShortDateString());

                            if (!string.IsNullOrEmpty(request.Prioridad))
                            {
                                //Otiene urgencia
                                int IdPrioridad = catalogos.GetPrioridad(Convert.ToInt32(request.Prioridad));

                                VarInter.priority_id = IdPrioridad;
                                VarInter.id = data.TicketInvgate;
                                VarInter.date = ConvertToTimestamp(dt).ToString();

                                response_ = incidentes.PutIncidente(VarInter);
                            }
                                
                            //Bitacora
                            bitacoraWO.ActualizaPriorizacion(request, data.TicketInvgate, out string Result);

                            //Si no actualizamos nada en Invgate
                            if (string.IsNullOrEmpty(response_.Estado))
                            {
                                response_.Ticket = data.TicketInvgate.ToString();
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
                        Ticket data = bitacoraWO.Get(request.TicketIMSS);

                        if (data.TicketInvgate > 0)
                        {
                            IncidentesPutRequest VarInter = new IncidentesPutRequest();
                            DateTime dt = Convert.ToDateTime(Convert.ToDateTime(request.FechaCambio.ToUpper().Replace("P.M", "").Replace("A.M", "")).ToShortDateString());

                            //Obtiene Categoria
                            CategoriastInvgate ci = new CategoriastInvgate();
                            string concat = "CAT PROD" + "|" +//"MESA DE SERVICIO IMSS" + "|" +
                                    request.CategoriaOpe01 + "|" +
                                    request.CategoriaOpe02 + "|" +
                                    request.CategoriaOpe03 + "|" +
                                    request.CategoriaPro01 + "|" +
                                    request.CategoriaPro02 + "|" +
                                    request.CategoriaPro03 + "|" +
                                    data.NombreProducto;

                            VarInter.category_id = ci.GetCategoria(concat);
                            VarInter.id = data.TicketInvgate;
                            VarInter.date = ConvertToTimestamp(dt).ToString();

                            response_ = incidentes.PutIncidente(VarInter);

                            //Bitacora
                            bitacoraWO.ActualizaCategoria(request, data.TicketInvgate, out string Result);
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
                        Ticket data = bitacoraWO.Get(request.TicketIMSS);

                        if (data.TicketInvgate > 0)
                        {
                            IncidentesCommentPostRequest VarInter = new IncidentesCommentPostRequest();

                            VarInter.request_id = data.TicketInvgate;
                            VarInter.comment = request.Notas;
                            VarInter.author_id = 1;
                            VarInter.is_solution = false;

                            response_ = comments.PostIncidenteComment(VarInter);

                            //Bitacora
                            bitacoraWO.AgregaNota(request, data.TicketInvgate, out string Result);
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
            }

            return response_;
        }


    }
}
