using SB = ServiceBitacora;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskIncidencias.Models;
using ServiceInvgate;
using TaskIncidencias.WS_Remedy;
using Entities;
using System.Text.RegularExpressions;

namespace TaskIncidencias
{
    public class ProcesaRegistro
    {
        ServiciosImss imss = new ServiciosImss();

        public Resultado IncidenteActualiza(int id, int idEstatus, int idCategorizacion)
        {
            Resultado result = new Resultado();

            try
            {
                //Consulta incidente en bitacora
                SB.IncidenteData data = new SB.IncidenteData();
                SB.CatalogosData catalogos = new SB.CatalogosData();
                SB.Incidente bitacora = data.GetIdIMSS(id);

                if (bitacora.TicketInvgate != null)
                {
                    //Obtiene Estatus
                    int idEstatusImss = catalogos.GetEstatusIncidenteIMSS(idEstatus);

                    if(idEstatusImss > 0)
                    {
                        WS_Remedy.Incidente _request = new WS_Remedy.Incidente();
                        _request.IDTicketInvgate = id.ToString();
                        _request.IDTicketRemedy = bitacora.TicketRemedy;
                        _request.EstadoNuevo = idEstatusImss;

                        if (idEstatusImss == 3)//Pendiente
                        {
                            _request.MotivoEstado = 6000;//Retenc.de contacto de soporte
                        }

                        if (idEstatusImss == 4)//Cerrado
                        {
                            //Obtiene categorizacion de cierre
                            CategoriastInvgate categoria = new CategoriastInvgate();
                            Entities.Categorizacion _categorizacion = categoria.GetCategorizacion(idCategorizacion);

                            _request.CatCierreOperacion01 = _categorizacion.CatOperacion01;
                            _request.CatCierreOperacion02 = _categorizacion.CatOperacion02;
                            _request.CatCierreOperacion03 = _categorizacion.CatOperacion03;
                            _request.CatCierreProducto01 = _categorizacion.CatProducto01;
                            _request.CatCierreProducto02 = _categorizacion.CatProducto02;
                            _request.CatCierreProducto03 = _categorizacion.CatProducto03;
                            _request.MotivoEstado = 21000; //Solucionado
                            _request.Resolucion = "Solucionado";
                        }

                        WS_Remedy.Result exec = imss.IncidenteActualiza(_request);

                        result.Success = exec.Estatus;
                        result.Message = exec.Resultado;
                    }
                    else
                    {
                        result.Success = true;
                        result.Message = "El estatus no existe en IMSS.";
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = "No se encontró el ticket de Invgate en la bitácora.";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public Resultado IncidenteActualizaCategorizacion(int id, int idCategorizacion)
        {
            Resultado result = new Resultado();

            try
            {
                //Consulta incidente en bitacora
                SB.IncidenteData data = new SB.IncidenteData();
                SB.CatalogosData catalogos = new SB.CatalogosData();
                SB.Incidente bitacora = data.GetIdIMSS(id);

                if (bitacora.TicketInvgate != null)
                {
                    //Obtiene categorizacion
                    CategoriastInvgate categoria = new CategoriastInvgate();
                    Entities.Categorizacion _categorizacion = categoria.GetCategorizacion(idCategorizacion);

                    WS_Remedy.Categorizacion _request = new WS_Remedy.Categorizacion();
                    _request.IDTicketInvgate = id.ToString();
                    _request.IDTicketRemedy = bitacora.TicketRemedy;
                    _request.CatOperacion01 = _categorizacion.CatOperacion01;
                    _request.CatOperacion02 = _categorizacion.CatOperacion02;
                    _request.CatOperacion03 = _categorizacion.CatOperacion03;
                    _request.CatProducto01 = _categorizacion.CatProducto01;
                    _request.CatProducto02 = _categorizacion.CatProducto02;
                    _request.CatProducto03 = _categorizacion.CatProducto03;

                    WS_Remedy.Result exec = imss.IncidenteActualizaCategorizacion(_request);

                    result.Success = exec.Estatus;
                    result.Message = exec.Resultado;

                }
                else
                {
                    result.Success = false;
                    result.Message = "No se encontró el ticket de Invgate en la bitácora.";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public Resultado IncidenteActualizaPrioridad(int id, int idPrioridad)
        {
            Resultado result = new Resultado();

            try
            {
                //Consulta incidente en bitacora
                SB.IncidenteData data = new SB.IncidenteData();
                SB.CatalogosData catalogos = new SB.CatalogosData();
                SB.Incidente bitacora = data.GetIdIMSS(id);

                if (bitacora.TicketInvgate != null)
                {
                    //Obtiene Prioridad
                    int idPrioridadImss = catalogos.GetUrgenciaIncidenteIMSS(idPrioridad);

                    if(idPrioridadImss > 0)
                    {
                        WS_Remedy.Prioridad _request = new WS_Remedy.Prioridad();
                        _request.IDTicketInvgate = id.ToString();
                        _request.IDTicketRemedy = bitacora.TicketRemedy;
                        _request.Urgencia = idPrioridadImss;
                        _request.Impacto = Convert.ToInt32(bitacora.Impacto);


                        WS_Remedy.Result exec = imss.IncidenteActualizaPrioridad(_request);

                        result.Success = exec.Estatus;
                        result.Message = exec.Resultado;
                    }
                    else
                    {
                        result.Success = true;
                        result.Message = "La prioridad no existe en IMSS.";
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = "No se encontró el ticket de Invgate en la bitácora.";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }

        static string CleanInput(string strIn)
        {
            // Replace invalid characters with empty strings.
            try
            {
                //return Regex.Replace(strIn, @"[^\w\.@-]", "",
                //                     RegexOptions.None, TimeSpan.FromSeconds(1.5));
                return Regex.Replace(strIn, "<.*?>", string.Empty);

            }
            // If we timeout when replacing invalid characters,
            // we should return Empty.
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }

        public Resultado IncidenteAdicionaNotas(int id, string nota)
        {
            Resultado result = new Resultado();

            try
            {
                //Consulta incidente en bitacora
                SB.IncidenteData data = new SB.IncidenteData();
                SB.CatalogosData catalogos = new SB.CatalogosData();
                SB.Incidente bitacora = data.GetIdIMSS(id);

                if (bitacora.TicketInvgate != null)
                {
                    
                    WS_Remedy.Comentario _request = new WS_Remedy.Comentario();
                    _request.IDTicketInvgate = id.ToString();
                    _request.IDTicketRemedy = bitacora.TicketRemedy;
                    _request.Notas = CleanInput(nota);
                    //_request.Adjunto01 = "";
                    //_request.AdjuntoName01 = "";
                    //_request.AdjuntoSize01 = "";
                    //_request.Adjunto02 = "";
                    //_request.AdjuntoName02 = "";
                    //_request.AdjuntoSize02 = "";
                    //_request.Adjunto03 = "";
                    //_request.AdjuntoName03 = "";
                    //_request.AdjuntoSize03 = "";

                    WS_Remedy.Result exec = imss.IncidenteAdicionaNotas(_request);

                    result.Success = exec.Estatus;
                    result.Message = exec.Resultado;

                }
                else
                {
                    result.Success = false;
                    result.Message = "No se encontró el ticket de Invgate en la bitácora.";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public Resultado WOActualiza(int id, int idEstatus)
        {
            Resultado result = new Resultado();

            try
            {
                //Consulta orden de trabajo en bitacora
                SB.OrdenTrabajoData data = new SB.OrdenTrabajoData();
                SB.CatalogosData catalogos = new SB.CatalogosData();
                SB.OrdenTrabajo bitacora = data.Get(id);

                if (bitacora.TicketInvgate != null)
                {
                    //Obtiene Estatus
                    int idEstatusImss = catalogos.GetEstatusWOIMSS(idEstatus);

                    if(idEstatusImss > 0)
                    {
                        WS_Remedy.OrdenTrabajo _request = new WS_Remedy.OrdenTrabajo();
                        _request.IDTicketInvgate = id.ToString();
                        _request.IDTicketRemedy = bitacora.TicketRemedy;
                        _request.EstadoNuevo = idEstatusImss;

                        if (idEstatusImss == 1)//Pendiente
                        {
                            _request.MotivoEstado = 31000;//En espera
                        }

                        if (idEstatusImss == 5)//Terminado
                        {
                            _request.MotivoEstado = 5000; //Con éxito
                            _request.Resolucion = "Solucionado";
                        }

                        WS_Remedy.Result exec = imss.OrdenTrabajoActualiza(_request);

                        result.Success = exec.Estatus;
                        result.Message = exec.Resultado;
                    }
                    else
                    {
                        result.Success = true;
                        result.Message = "El estatus no existe en IMSS.";
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = "No se encontró el ticket de Invgate en la bitácora.";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public Resultado WOActualizaCategorizacion(int id, int idCategorizacion)
        {
            Resultado result = new Resultado();

            try
            {
                //Consulta incidente en bitacora
                SB.OrdenTrabajoData data = new SB.OrdenTrabajoData();
                SB.CatalogosData catalogos = new SB.CatalogosData();
                SB.OrdenTrabajo bitacora = data.Get(id);

                if (bitacora.TicketInvgate != null)
                {
                    //Obtiene categorizacion
                    CategoriastInvgate categoria = new CategoriastInvgate();
                    Entities.Categorizacion _categorizacion = categoria.GetCategorizacion(idCategorizacion);

                    WS_Remedy.Categorizacion _request = new WS_Remedy.Categorizacion();
                    _request.IDTicketInvgate = id.ToString();
                    _request.IDTicketRemedy = bitacora.TicketRemedy;
                    _request.CatOperacion01 = _categorizacion.CatOperacion01;
                    _request.CatOperacion02 = _categorizacion.CatOperacion02;
                    _request.CatOperacion03 = _categorizacion.CatOperacion03;
                    _request.CatProducto01 = _categorizacion.CatProducto01;
                    _request.CatProducto02 = _categorizacion.CatProducto02;
                    _request.CatProducto03 = _categorizacion.CatProducto03;

                    WS_Remedy.Result exec = imss.OrdenTrabajoActualizaCategorizacion(_request);

                    result.Success = exec.Estatus;
                    result.Message = exec.Resultado;

                }
                else
                {
                    result.Success = false;
                    result.Message = "No se encontró el ticket de Invgate en la bitácora.";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public Resultado WOActualizaPrioridad(int id, int idPrioridad)
        {
            Resultado result = new Resultado();

            try
            {
                //Consulta incidente en bitacora
                SB.OrdenTrabajoData data = new SB.OrdenTrabajoData();
                SB.CatalogosData catalogos = new SB.CatalogosData();
                SB.OrdenTrabajo bitacora = data.Get(id);

                if (bitacora.TicketInvgate != null)
                {
                    //Obtiene Prioridad
                    int idPrioridadImss = catalogos.GetPrioridadWOIMSS(idPrioridad);

                    if(idPrioridad > 0)
                    {
                        WS_Remedy.PrioridadOT _request = new WS_Remedy.PrioridadOT();
                        _request.IDTicketInvgate = id.ToString();
                        _request.IDTicketRemedy = bitacora.TicketRemedy;
                        _request.Prioridad = idPrioridadImss;

                        WS_Remedy.Result exec = imss.OrdenTrabajoActualizaPrioridad(_request);

                        result.Success = exec.Estatus;
                        result.Message = exec.Resultado;
                    }
                    else
                    {
                        result.Success = true;
                        result.Message = "La prioridad no existe en IMSS.";
                    }              
                }
                else
                {
                    result.Success = false;
                    result.Message = "No se encontró el ticket de Invgate en la bitácora.";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public Resultado WOAdicionaNotas(int id, string nota)
        {
            Resultado result = new Resultado();

            try
            {
                //Consulta incidente en bitacora
                SB.OrdenTrabajoData data = new SB.OrdenTrabajoData();
                SB.CatalogosData catalogos = new SB.CatalogosData();
                SB.OrdenTrabajo bitacora = data.Get(id);

                if (bitacora.TicketInvgate != null)
                {
                    WS_Remedy.Comentario _request = new WS_Remedy.Comentario();
                    _request.IDTicketInvgate = id.ToString();
                    _request.IDTicketRemedy = bitacora.TicketRemedy;
                    _request.Notas = CleanInput(nota);
                    //_request.Adjunto01 = "";
                    //_request.AdjuntoName01 = "";
                    //_request.AdjuntoSize01 = "";
                    //_request.Adjunto02 = "";
                    //_request.AdjuntoName02 = "";
                    //_request.AdjuntoSize02 = "";
                    //_request.Adjunto03 = "";
                    //_request.AdjuntoName03 = "";
                    //_request.AdjuntoSize03 = "";

                    WS_Remedy.Result exec = imss.OrdenTrabajoAdicionaNotas(_request);

                    result.Success = exec.Estatus;
                    result.Message = exec.Resultado;

                }
                else
                {
                    result.Success = false;
                    result.Message = "No se encontró el ticket de Invgate en la bitácora.";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }

    }
}
