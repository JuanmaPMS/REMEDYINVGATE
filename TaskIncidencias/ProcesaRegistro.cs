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
using System.Net.NetworkInformation;
using Newtonsoft.Json;
using Entities.Invgate;
using ServiceBitacora;

namespace TaskIncidencias
{
    public class ProcesaRegistro
    {
        ServiciosImss imss = new ServiciosImss();
        IncidentesInvgate incidentes = new IncidentesInvgate();
        LogTask log = new LogTask();

        public Resultado IncidenteActualiza(int id, int idEstatus)
        {
            Resultado result = new Resultado();

            try
            {
                //Consulta incidente en bitacora
                SB.IncidenteData data = new SB.IncidenteData();
                SB.CatalogosData catalogos = new SB.CatalogosData();
                SB.Incidente bitacora = data.GetIdIMSS(id);

                if (bitacora != null && bitacora.TicketInvgate > 0)
                {
                    //Solo se consideran Nuevo, Abierto, Pendiente
                    if(idEstatus == 1 || idEstatus == 2 || idEstatus == 3)
                    {
                        //Obtiene Estatus
                        int idEstatusImss = catalogos.GetEstatusIncidenteIMSS(idEstatus);

                        if (idEstatusImss > 0)
                        {
                            WS_Remedy.Incidente _request = new WS_Remedy.Incidente();
                            _request.IDTicketInvgate = id.ToString();
                            _request.IDTicketRemedy = bitacora.TicketRemedy;
                            _request.EstadoNuevo = idEstatusImss;

                            WS_Remedy.Result exec = imss.IncidenteActualiza(_request);

                            if(exec.Resultado.Contains("ERROR: El cambio de estado a 'En curso' no está permitido en el Incidente actual"))
                            {
                                result.Success = true;
                                result.Message = exec.Resultado;
                            }
                            else
                            {
                                result.Success = exec.Estatus;
                                result.Message = exec.Resultado;
                            }                   
                        }
                        else
                        {
                            result.Success = true;
                            result.Message = "El estatus no existe en IMSS.";
                        }
                    }
                    else
                    {
                        result.Success = true;
                        result.Message = "OK";
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
                log.LogMsg("Error|IncidenteActualiza|Id: " + id.ToString() + "|" + ex.Message);
            }

            return result;
        }

        //SE DEBE MODIFICA PARA ADAPTAR LA NUEVA FORMA DE RECATEGORIZAR DE INVGATE A IMSS
        public Resultado IncidenteActualizaCategorizacion(int id, int idCategorizacion)
        {
            Resultado result = new Resultado();

            try
            {
                //Consulta incidente en bitacora
                SB.IncidenteData data = new SB.IncidenteData();
                SB.CatalogosData catalogos = new SB.CatalogosData();
                SB.Incidente bitacora = data.GetIdIMSS(id);

                if (bitacora != null && bitacora.TicketInvgate > 0)
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
                log.LogMsg("Error|IncidenteActualizaCategorizacion|Id: " + id.ToString() + "|" + ex.Message);
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

                if (bitacora != null && bitacora.TicketInvgate > 0)
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
                log.LogMsg("Error|IncidenteActualizaPrioridad|Id: " + id.ToString() + "|" + ex.Message);
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
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }

        public Resultado IncidenteAdicionaNotas(int id,  string nota)
        {
            Resultado result = new Resultado();

            try
            {
                //Consulta incidente en bitacora
                SB.IncidenteData data = new SB.IncidenteData();
                SB.CatalogosData catalogos = new SB.CatalogosData();
                SB.Incidente bitacora = data.GetIdIMSS(id);

                if (bitacora != null && bitacora.TicketInvgate > 0)
                {
                    //Tratamiento de texto
                    string _nota = CleanInput(nota);
                    string[] arrNota = _nota.Split(new[] { "||" }, StringSplitOptions.None);

                    if(_nota.Contains("@@R"))//Resuelto
                    {
                        //Obtiene Estatus
                        int idEstatusInvgate = 5; //Solucionado
                        int idEstatusImss = catalogos.GetEstatusIncidenteIMSS(idEstatusInvgate); 
                        //Obtiene Motivo Estado
                        int idMotivo = Convert.ToInt32(arrNota[0].Substring(3).Trim());
                        //Obtiene categorizacion de cierre
                        string[] arrCategoria = arrNota[1].Split(new[] { "|" }, StringSplitOptions.None);
                        //int idCategorizacion = Convert.ToInt32(arrNota[1].Replace("Categoria:","").Trim());
                        //CategoriastInvgate categoria = new CategoriastInvgate();
                        //Entities.Categorizacion _categorizacion = categoria.GetCategorizacion(idCategorizacion);

                        WS_Remedy.Incidente _request = new WS_Remedy.Incidente();
                        _request.IDTicketInvgate = id.ToString();
                        _request.IDTicketRemedy = bitacora.TicketRemedy;
                        _request.EstadoNuevo = idEstatusImss;
                        //Se envia categorizacion que tenemos guardada
                        _request.CatCierreOperacion01 = arrCategoria[1];
                        _request.CatCierreOperacion02 = arrCategoria[2];
                        _request.CatCierreOperacion03 = arrCategoria[3];
                        _request.CatCierreProducto01 = bitacora.CategoriaPro01;
                        _request.CatCierreProducto02 = bitacora.CategoriaPro02;
                        _request.CatCierreProducto03 = bitacora.CategoriaPro03;
                        _request.MotivoEstado = idMotivo;
                        _request.Resolucion = arrNota[2].Trim();

                        WS_Remedy.Result exec = imss.IncidenteActualiza(_request);

                        result.Success = exec.Estatus;
                        result.Message = exec.Resultado;

                        if (result.Success)//Actualiza estatus en Invgate
                        {
                            IncidentPutRequest VarInter = new IncidentPutRequest();
                            VarInter.id = id;
                            VarInter.statusId = idEstatusInvgate;

                            Entities.Intermedio.Result response_ = incidentes.PutIncidenteStatus(VarInter);
                        }
                    }
                    else if (_nota.Contains("@@P"))//Pendiente
                    {
                        //Obtiene Estatus
                        int idEstatusInvgate = 4; //En Espera
                        int idEstatusImss = catalogos.GetEstatusIncidenteIMSS(idEstatusInvgate); 
                        //Obtiene Motivo Estado
                        int idMotivo = Convert.ToInt32(arrNota[0].Substring(3, 5).Trim());

                        WS_Remedy.Incidente _request = new WS_Remedy.Incidente();
                        _request.IDTicketInvgate = id.ToString();
                        _request.IDTicketRemedy = bitacora.TicketRemedy;
                        _request.EstadoNuevo = idEstatusImss;
                        _request.MotivoEstado = idMotivo;
                       
                        WS_Remedy.Result exec = imss.IncidenteActualiza(_request);    

                        result.Success = exec.Estatus;
                        result.Message = exec.Resultado;

                        if (result.Success)
                        {
                            WS_Remedy.Comentario _coment = new WS_Remedy.Comentario();
                            _coment.IDTicketInvgate = id.ToString();
                            _coment.IDTicketRemedy = bitacora.TicketRemedy;
                            _coment.Notas = arrNota[0].Substring(8).Trim();

                            WS_Remedy.Result exCom = imss.IncidenteAdicionaNotas(_coment);

                            //Actualiza estatus en Invgate
                            IncidentPutRequest VarInter = new IncidentPutRequest();
                            VarInter.id = id;
                            VarInter.statusId = idEstatusInvgate;

                            Entities.Intermedio.Result response_ = incidentes.PutIncidenteStatus(VarInter);
                        }
                    }
                    else
                    {
                        WS_Remedy.Comentario _request = new WS_Remedy.Comentario();
                        _request.IDTicketInvgate = id.ToString();
                        _request.IDTicketRemedy = bitacora.TicketRemedy;
                        _request.Notas = arrNota[0];

                        WS_Remedy.Result exec = imss.IncidenteAdicionaNotas(_request);
                        result.Success = exec.Estatus;
                        result.Message = exec.Resultado;
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
                log.LogMsg("Error|IncidenteAdicionaNotas|Id: " + id.ToString() + "|" + ex.Message);
            }

            return result;
        }

        public Resultado IncidenteAdicionaNotasAdjunto(int id, string nota)
        {
            Resultado result = new Resultado();

            try
            {
                //Consulta incidente en bitacora
                SB.IncidenteData data = new SB.IncidenteData();
                SB.CatalogosData catalogos = new SB.CatalogosData();
                SB.Incidente bitacora = data.GetIdIMSS(id);

                if (bitacora != null && bitacora.TicketInvgate > 0)
                {
                    //Tratamiento de texto
                    string _nota = CleanInput(nota);
                    string[] arrNota = _nota.Split(new[] { "||" }, StringSplitOptions.None);

                    if (_nota.Contains("@@R"))//Resuelto
                    {
                        string files = arrNota[3].Replace("Files:", "");
                        files = files.Substring(0, files.Length - 1);

                        //Obtiene Estatus
                        int idEstatusInvgate = 5;//Solucionado
                        int idEstatusImss = catalogos.GetEstatusIncidenteIMSS(idEstatusInvgate); 
                        //Obtiene Motivo Estado
                        int idMotivo = Convert.ToInt32(arrNota[0].Substring(3).Trim());
                        ////Obtiene categorizacion de cierre
                        string[] arrCategoria = arrNota[1].Split(new[] { "|" }, StringSplitOptions.None);
                        //int idCategorizacion = Convert.ToInt32(arrNota[1].Replace("Categoria:", "").Trim());
                        //CategoriastInvgate categoria = new CategoriastInvgate();
                        //Entities.Categorizacion _categorizacion = categoria.GetCategorizacion(idCategorizacion);

                        WS_Remedy.Incidente _request = new WS_Remedy.Incidente();
                        _request.IDTicketInvgate = id.ToString();
                        _request.IDTicketRemedy = bitacora.TicketRemedy;
                        _request.EstadoNuevo = idEstatusImss;
                        //Se envia categorizacion que tenemos guardada
                        _request.CatCierreOperacion01 = arrCategoria[1];
                        _request.CatCierreOperacion02 = arrCategoria[2];
                        _request.CatCierreOperacion03 = arrCategoria[3];
                        _request.CatCierreProducto01 = bitacora.CategoriaPro01;
                        _request.CatCierreProducto02 = bitacora.CategoriaPro02;
                        _request.CatCierreProducto03 = bitacora.CategoriaPro03;
                        _request.MotivoEstado = idMotivo;
                        _request.Resolucion = arrNota[2].Trim();

                        WS_Remedy.Result exec = imss.IncidenteActualiza(_request);

                        result.Success = exec.Estatus;
                        result.Message = exec.Resultado;

                        if (result.Success)
                        {
                            //Si hay archivos, envia nota
                            if (!string.IsNullOrEmpty(files))
                            {
                                WS_Remedy.Comentario _coment = new WS_Remedy.Comentario();
                                _coment.IDTicketInvgate = id.ToString();
                                _coment.IDTicketRemedy = bitacora.TicketRemedy;
                                _coment.Notas = "Adjuntos cierrre de ticket.";

                                string[] arrFiles = files.Split(',');

                                if (arrFiles.Length >= 3)
                                {
                                    dynamic file1 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[0]));

                                    if (file1 != null)
                                    {
                                        if (Convert.ToBoolean(file1.success))
                                        {
                                            _coment.Adjunto01 = file1.attach;
                                            _coment.AdjuntoName01 = file1.name;
                                            _coment.AdjuntoSize01 = file1.size;
                                        }
                                    }


                                    dynamic file2 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[1]));

                                    if (file2 != null)
                                    {
                                        if (Convert.ToBoolean(file2.success))
                                        {
                                            _coment.Adjunto02 = file2.attach;
                                            _coment.AdjuntoName02 = file2.name;
                                            _coment.AdjuntoSize02 = file2.size;
                                        }
                                    }

                                    dynamic file3 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[2]));

                                    if (file3 != null)
                                    {
                                        if (Convert.ToBoolean(file3.success))
                                        {
                                            _coment.Adjunto03 = file3.attach;
                                            _coment.AdjuntoName03 = file3.name;
                                            _coment.AdjuntoSize03 = file3.size;
                                        }
                                    }

                                }

                                if (arrFiles.Length == 2)
                                {
                                    dynamic file1 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[0]));

                                    if (file1 != null)
                                    {
                                        if (Convert.ToBoolean(file1.success))
                                        {
                                            _coment.Adjunto01 = file1.attach;
                                            _coment.AdjuntoName01 = file1.name;
                                            _coment.AdjuntoSize01 = file1.size;
                                        }
                                    }


                                    dynamic file2 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[1]));

                                    if (file2 != null)
                                    {
                                        if (Convert.ToBoolean(file2.success))
                                        {
                                            _coment.Adjunto02 = file2.attach;
                                            _coment.AdjuntoName02 = file2.name;
                                            _coment.AdjuntoSize02 = file2.size;
                                        }
                                    }
                                }

                                if (arrFiles.Length == 1)
                                {
                                    dynamic file1 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[0]));

                                    if (file1 != null)
                                    {
                                        if (Convert.ToBoolean(file1.success))
                                        {
                                            _coment.Adjunto01 = file1.attach;
                                            _coment.AdjuntoName01 = file1.name;
                                            _coment.AdjuntoSize01 = file1.size;
                                        }
                                    }
                                }

                                WS_Remedy.Result exCom = imss.IncidenteAdicionaNotas(_coment);
                            }

                            //Actualiza estatus en Invgate
                            IncidentPutRequest VarInter = new IncidentPutRequest();
                            VarInter.id = id;
                            VarInter.statusId = idEstatusInvgate;

                            Entities.Intermedio.Result response_ = incidentes.PutIncidenteStatus(VarInter);
                        }
                    }
                    else if (_nota.Contains("@@P"))//Pendiente
                    {
                        string files = arrNota[1].Replace("Files:", "");
                        files = files.Substring(0, files.Length - 1);
                        //Obtiene Estatus
                        int idEstatusInvgate = 4;//En Espera
                        int idEstatusImss = catalogos.GetEstatusIncidenteIMSS(idEstatusInvgate); 
                        //Obtiene Motivo Estado
                        int idMotivo = Convert.ToInt32(arrNota[0].Substring(3, 5).Trim());

                        WS_Remedy.Incidente _request = new WS_Remedy.Incidente();
                        _request.IDTicketInvgate = id.ToString();
                        _request.IDTicketRemedy = bitacora.TicketRemedy;
                        _request.EstadoNuevo = idEstatusImss;
                        _request.MotivoEstado = idMotivo;

                        WS_Remedy.Result exec = imss.IncidenteActualiza(_request);


                        result.Success = exec.Estatus;
                        result.Message = exec.Resultado;

                        if (result.Success)
                        {
                            //Si hay archivos, envia nota
                            if (!string.IsNullOrEmpty(files))
                            {
                                WS_Remedy.Comentario _coment = new WS_Remedy.Comentario();
                                _coment.IDTicketInvgate = id.ToString();
                                _coment.IDTicketRemedy = bitacora.TicketRemedy;
                                _coment.Notas = arrNota[0].Substring(8).Trim() == string.Empty ? "Adjuntos actualización estatus." : arrNota[1].Trim();

                                string[] arrFiles = files.Split(',');

                                if (arrFiles.Length >= 3)
                                {
                                    dynamic file1 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[0]));

                                    if (file1 != null)
                                    {
                                        if (Convert.ToBoolean(file1.success))
                                        {
                                            _coment.Adjunto01 = file1.attach;
                                            _coment.AdjuntoName01 = file1.name;
                                            _coment.AdjuntoSize01 = file1.size;
                                        }
                                    }


                                    dynamic file2 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[1]));

                                    if (file2 != null)
                                    {
                                        if (Convert.ToBoolean(file2.success))
                                        {
                                            _coment.Adjunto02 = file2.attach;
                                            _coment.AdjuntoName02 = file2.name;
                                            _coment.AdjuntoSize02 = file2.size;
                                        }
                                    }

                                    dynamic file3 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[2]));

                                    if (file3 != null)
                                    {
                                        if (Convert.ToBoolean(file3.success))
                                        {
                                            _coment.Adjunto03 = file3.attach;
                                            _coment.AdjuntoName03 = file3.name;
                                            _coment.AdjuntoSize03 = file3.size;
                                        }
                                    }

                                }

                                if (arrFiles.Length == 2)
                                {
                                    dynamic file1 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[0]));

                                    if (file1 != null)
                                    {
                                        if (Convert.ToBoolean(file1.success))
                                        {
                                            _coment.Adjunto01 = file1.attach;
                                            _coment.AdjuntoName01 = file1.name;
                                            _coment.AdjuntoSize01 = file1.size;
                                        }
                                    }


                                    dynamic file2 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[1]));

                                    if (file2 != null)
                                    {
                                        if (Convert.ToBoolean(file2.success))
                                        {
                                            _coment.Adjunto02 = file2.attach;
                                            _coment.AdjuntoName02 = file2.name;
                                            _coment.AdjuntoSize02 = file2.size;
                                        }
                                    }
                                }

                                if (arrFiles.Length == 1)
                                {
                                    dynamic file1 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[0]));

                                    if (file1 != null)
                                    {
                                        if (Convert.ToBoolean(file1.success))
                                        {
                                            _coment.Adjunto01 = file1.attach;
                                            _coment.AdjuntoName01 = file1.name;
                                            _coment.AdjuntoSize01 = file1.size;
                                        }
                                    }
                                }

                                WS_Remedy.Result exCom = imss.IncidenteAdicionaNotas(_coment);
                            }
                            //Actualiza estatus en Invgate
                            IncidentPutRequest VarInter = new IncidentPutRequest();
                            VarInter.id = id;
                            VarInter.statusId = idEstatusInvgate;

                            Entities.Intermedio.Result response_ = incidentes.PutIncidenteStatus(VarInter);
                        }
                    }
                    else
                    {
                        string files = arrNota[1].Replace("Files:", "");
                        files = files.Substring(0, files.Length - 1);

                        WS_Remedy.Comentario _request = new WS_Remedy.Comentario();
                        _request.IDTicketInvgate = id.ToString();
                        _request.IDTicketRemedy = bitacora.TicketRemedy;
                        _request.Notas = arrNota[0];

                        
                        if (!string.IsNullOrEmpty(files))
                        {
                            string[] arrFiles = files.Split(',');

                            if (arrFiles.Length >= 3)
                            {
                                dynamic file1 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[0]));

                                if (file1 != null)
                                {
                                    if (Convert.ToBoolean(file1.success))
                                    {
                                        _request.Adjunto01 = file1.attach;
                                        _request.AdjuntoName01 = file1.name;
                                        _request.AdjuntoSize01 = file1.size;
                                    }
                                }


                                dynamic file2 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[1]));

                                if (file2 != null)
                                {
                                    if (Convert.ToBoolean(file2.success))
                                    {
                                        _request.Adjunto02 = file2.attach;
                                        _request.AdjuntoName02 = file2.name;
                                        _request.AdjuntoSize02 = file2.size;
                                    }
                                }

                                dynamic file3 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[2]));

                                if (file3 != null)
                                {
                                    if (Convert.ToBoolean(file3.success))
                                    {
                                        _request.Adjunto03 = file3.attach;
                                        _request.AdjuntoName03 = file3.name;
                                        _request.AdjuntoSize03 = file3.size;
                                    }
                                }
                            }

                            if (arrFiles.Length == 2)
                            {
                                dynamic file1 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[0]));

                                if (file1 != null)
                                {
                                    if (Convert.ToBoolean(file1.success))
                                    {
                                        _request.Adjunto01 = file1.attach;
                                        _request.AdjuntoName01 = file1.name;
                                        _request.AdjuntoSize01 = file1.size;
                                    }
                                }


                                dynamic file2 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[1]));

                                if (file2 != null)
                                {
                                    if (Convert.ToBoolean(file2.success))
                                    {
                                        _request.Adjunto02 = file2.attach;
                                        _request.AdjuntoName02 = file2.name;
                                        _request.AdjuntoSize02 = file2.size;
                                    }
                                }
                            }

                            if (arrFiles.Length == 1)
                            {
                                dynamic file1 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[0]));

                                if (file1 != null)
                                {
                                    if (Convert.ToBoolean(file1.success))
                                    {
                                        _request.Adjunto01 = file1.attach;
                                        _request.AdjuntoName01 = file1.name;
                                        _request.AdjuntoSize01 = file1.size;
                                    }
                                }
                            }

                        }

                        WS_Remedy.Result exec = imss.IncidenteAdicionaNotas(_request);
                        result.Success = exec.Estatus;
                        result.Message = exec.Resultado;
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
                log.LogMsg("Error|IncidenteAdicionaNotasAdjunto|Id: " + id.ToString() + "|" + ex.Message);
            }

            return result;
        }

        public Resultado IncidenteAdicionaAdjunto(int id, int idFile)
        {
            Resultado result = new Resultado();

            try
            {
                //Consulta incidente en bitacora
                SB.IncidenteData data = new SB.IncidenteData();
                SB.CatalogosData catalogos = new SB.CatalogosData();
                SB.Incidente bitacora = data.GetIdIMSS(id);

                if (bitacora != null && bitacora.TicketInvgate > 0)
                {
                    WS_Remedy.Comentario _request = new WS_Remedy.Comentario();
                    _request.IDTicketInvgate = id.ToString();
                    _request.IDTicketRemedy = bitacora.TicketRemedy;
                    _request.Notas = "Adjuntos Ticket: " + bitacora.TicketRemedy;


                    dynamic file = incidentes.GetAttachments(idFile);

                    if (file != null)
                    {
                        if (Convert.ToBoolean(file.success))
                        {
                            _request.Adjunto01 = file.attach;
                            _request.AdjuntoName01 = file.name;
                            _request.AdjuntoSize01 = file.size;
                        }
                    }


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
                log.LogMsg("Error|IncidenteAdicionaAdjunto|Id: " + id.ToString() + "|" + ex.Message);
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
                SB.OrdenTrabajo bitacora = data.GetIdIMSS(id);

                if (bitacora != null && bitacora.TicketInvgate > 0)
                {
                    //Solo se consideran Nuevo, Abierto, Pendiente
                    if (idEstatus == 1 || idEstatus == 2 || idEstatus == 3)
                    {
                        //Obtiene Estatus
                        int idEstatusImss = catalogos.GetEstatusWOIMSS(idEstatus);

                        if (idEstatusImss > 0)
                        {
                            WS_Remedy.OrdenTrabajo _request = new WS_Remedy.OrdenTrabajo();
                            _request.IDTicketInvgate = id.ToString();
                            _request.IDTicketRemedy = bitacora.TicketRemedy;
                            _request.EstadoNuevo = idEstatusImss;

                            WS_Remedy.Result exec = imss.OrdenTrabajoActualiza(_request);
                            Console.WriteLine(exec.Resultado);


                            if (exec.Resultado.Contains("ERROR: El cambio de estado a 'En curso' no está permitido en la Orden de trabajo actual"))
                            { 
                                result.Success = true;
                                result.Message = exec.Resultado;
                            }
                            else
                            {
                                result.Success = exec.Estatus;
                                result.Message = exec.Resultado;
                            }

                        }
                        else
                        {
                            result.Success = true;
                            result.Message = "El estatus no existe en IMSS.";
                        }
                    }
                    else
                    {
                        result.Success = true;
                        result.Message = "OK";
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
                log.LogMsg("Error|WOActualiza|Id: " + id.ToString() + "|" + ex.Message);
            }

            return result;
        }

        //SE DEBE MODIFICA PARA ADAPTAR LA NUEVA FORMA DE RECATEGORIZAR DE INVGATE A IMSS
        public Resultado WOActualizaCategorizacion(int id, int idCategorizacion)
        {
            Resultado result = new Resultado();

            try
            {
                //Consulta incidente en bitacora
                SB.OrdenTrabajoData data = new SB.OrdenTrabajoData();
                SB.CatalogosData catalogos = new SB.CatalogosData();
                SB.OrdenTrabajo bitacora = data.GetIdIMSS(id);

                if (bitacora != null && bitacora.TicketInvgate > 0)
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
                log.LogMsg("Error|WOActualizaCategorizacion|Id: " + id.ToString() + "|" + ex.Message);
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
                SB.OrdenTrabajo bitacora = data.GetIdIMSS(id);

                if (bitacora != null && bitacora.TicketInvgate > 0)
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
                log.LogMsg("Error|WOActualizaPrioridad|Id: " + id.ToString() + "|" + ex.Message);
            }

            return result;
        }

        public Resultado WOAdicionaNotas(int id, string nota)
        {
            Resultado result = new Resultado();

            try
            {
                //Consulta orden en bitacora
                SB.OrdenTrabajoData data = new SB.OrdenTrabajoData();
                SB.CatalogosData catalogos = new SB.CatalogosData();
                SB.OrdenTrabajo bitacora = data.GetIdIMSS(id);

                if (bitacora != null && bitacora.TicketInvgate > 0)
                {
                    //Tratamiento de texto
                    string _nota = CleanInput(nota);
                    string[] arrNota = _nota.Split(new[] { "||" }, StringSplitOptions.None);

                    if (_nota.Contains("@@R"))//Resuelto
                    {
                        //Obtiene Estatus
                        int idEstatusInvgate = 5;//Solucionado
                        int idEstatusImss = catalogos.GetEstatusWOIMSS(idEstatusInvgate);
                        //Obtiene Motivo Estado
                        int idMotivo = Convert.ToInt32(arrNota[0].Substring(3, 5).Trim());

                        WS_Remedy.OrdenTrabajo _request = new WS_Remedy.OrdenTrabajo();
                        _request.IDTicketInvgate = id.ToString();
                        _request.IDTicketRemedy = bitacora.TicketRemedy;
                        _request.EstadoNuevo = idEstatusImss;
                        _request.MotivoEstado = idMotivo;
                        _request.Resolucion = arrNota[0].Substring(8).Trim();

                        WS_Remedy.Result exec = imss.OrdenTrabajoActualiza(_request);

                        result.Success = exec.Estatus;
                        result.Message = exec.Resultado;

                        if (result.Success)//Actualiza estatus en Invgate
                        {
                            IncidentPutRequest VarInter = new IncidentPutRequest();
                            VarInter.id = id;
                            VarInter.statusId = idEstatusInvgate;

                            Entities.Intermedio.Result response_ = incidentes.PutIncidenteStatus(VarInter);
                        }
                    }
                    else if (_nota.Contains("@@P"))//Pendiente
                    {
                        //Obtiene Estatus
                        int idEstatusInvgate = 4;//En Espera
                        int idEstatusImss = catalogos.GetEstatusWOIMSS(4);
                        //Obtiene Motivo Estado
                        int idMotivo = Convert.ToInt32(arrNota[0].Substring(3, 5).Trim());

                        WS_Remedy.OrdenTrabajo _request = new WS_Remedy.OrdenTrabajo();
                        _request.IDTicketInvgate = id.ToString();
                        _request.IDTicketRemedy = bitacora.TicketRemedy;
                        _request.EstadoNuevo = idEstatusImss;
                        _request.MotivoEstado = idMotivo;

                        WS_Remedy.Result exec = imss.OrdenTrabajoActualiza(_request);

                        result.Success = exec.Estatus;
                        result.Message = exec.Resultado;

                        if (result.Success)//Actualiza estatus en Invgate
                        {
                            WS_Remedy.Comentario _coment = new WS_Remedy.Comentario();
                            _coment.IDTicketInvgate = id.ToString();
                            _coment.IDTicketRemedy = bitacora.TicketRemedy;
                            _coment.Notas = arrNota[0].Substring(8).Trim();

                            WS_Remedy.Result exCom = imss.OrdenTrabajoAdicionaNotas(_coment);

                            IncidentPutRequest VarInter = new IncidentPutRequest();
                            VarInter.id = id;
                            VarInter.statusId = idEstatusInvgate;

                            Entities.Intermedio.Result response_ = incidentes.PutIncidenteStatus(VarInter);
                        }
                    }
                    else
                    {
                        WS_Remedy.Comentario _request = new WS_Remedy.Comentario();
                        _request.IDTicketInvgate = id.ToString();
                        _request.IDTicketRemedy = bitacora.TicketRemedy;
                        _request.Notas = arrNota[0];

                        WS_Remedy.Result exec = imss.OrdenTrabajoAdicionaNotas(_request);

                        result.Success = exec.Estatus;
                        result.Message = exec.Resultado;
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
                log.LogMsg("Error|WOAdicionaNotas|Id: " + id.ToString() + "|" + ex.Message);
            }

            return result;
        }

        public Resultado WOAdicionaNotasAdjunto(int id, string nota)
        {
            Resultado result = new Resultado();

            try
            {
                //Consulta orden en bitacora
                SB.OrdenTrabajoData data = new SB.OrdenTrabajoData();
                SB.CatalogosData catalogos = new SB.CatalogosData();
                SB.OrdenTrabajo bitacora = data.GetIdIMSS(id);

                if (bitacora != null && bitacora.TicketInvgate > 0)
                {
                    //Tratamiento de texto
                    string _nota = CleanInput(nota);
                    string[] arrNota = _nota.Split(new[] { "||" }, StringSplitOptions.None);

                    if (_nota.Contains("@@R"))//Resuelto
                    {
                        string files = arrNota[1].Replace("Files:", "");
                        files = files.Substring(0, files.Length - 1);

                        //Obtiene Estatus
                        int idEstatusInvgate = 5;//Solucionado
                        int idEstatusImss = catalogos.GetEstatusWOIMSS(idEstatusInvgate);
                        //Obtiene Motivo Estado
                        int idMotivo = Convert.ToInt32(arrNota[0].Substring(3, 5).Trim());
                        
                        WS_Remedy.OrdenTrabajo _request = new WS_Remedy.OrdenTrabajo();
                        _request.IDTicketInvgate = id.ToString();
                        _request.IDTicketRemedy = bitacora.TicketRemedy;
                        _request.EstadoNuevo = idEstatusImss;
                        _request.MotivoEstado = idMotivo;
                        _request.Resolucion = arrNota[0].Substring(8).Trim();

                        WS_Remedy.Result exec = imss.OrdenTrabajoActualiza(_request);

                        result.Success = exec.Estatus;
                        result.Message = exec.Resultado;

                        if (result.Success)
                        {
                            //Si hay archivos, envia nota
                            if (!string.IsNullOrEmpty(files))
                            {
                                WS_Remedy.Comentario _coment = new WS_Remedy.Comentario();
                                _coment.IDTicketInvgate = id.ToString();
                                _coment.IDTicketRemedy = bitacora.TicketRemedy;
                                _coment.Notas = "Adjuntos cierre de WO.";

                                string[] arrFiles = files.Split(',');

                                if (arrFiles.Length >= 3)
                                {
                                    dynamic file1 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[0]));

                                    if (file1 != null)
                                    {
                                        if (Convert.ToBoolean(file1.success))
                                        {
                                            _coment.Adjunto01 = file1.attach;
                                            _coment.AdjuntoName01 = file1.name;
                                            _coment.AdjuntoSize01 = file1.size;
                                        }
                                    }


                                    dynamic file2 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[1]));

                                    if (file2 != null)
                                    {
                                        if (Convert.ToBoolean(file2.success))
                                        {
                                            _coment.Adjunto02 = file2.attach;
                                            _coment.AdjuntoName02 = file2.name;
                                            _coment.AdjuntoSize02 = file2.size;
                                        }
                                    }

                                    dynamic file3 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[2]));

                                    if (file3 != null)
                                    {
                                        if (Convert.ToBoolean(file3.success))
                                        {
                                            _coment.Adjunto03 = file3.attach;
                                            _coment.AdjuntoName03 = file3.name;
                                            _coment.AdjuntoSize03 = file3.size;
                                        }
                                    }

                                }

                                if (arrFiles.Length == 2)
                                {
                                    dynamic file1 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[0]));

                                    if (file1 != null)
                                    {
                                        if (Convert.ToBoolean(file1.success))
                                        {
                                            _coment.Adjunto01 = file1.attach;
                                            _coment.AdjuntoName01 = file1.name;
                                            _coment.AdjuntoSize01 = file1.size;
                                        }
                                    }


                                    dynamic file2 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[1]));

                                    if (file2 != null)
                                    {
                                        if (Convert.ToBoolean(file2.success))
                                        {
                                            _coment.Adjunto02 = file2.attach;
                                            _coment.AdjuntoName02 = file2.name;
                                            _coment.AdjuntoSize02 = file2.size;
                                        }
                                    }
                                }

                                if (arrFiles.Length == 1)
                                {
                                    dynamic file1 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[0]));

                                    if (file1 != null)
                                    {
                                        if (Convert.ToBoolean(file1.success))
                                        {
                                            _coment.Adjunto01 = file1.attach;
                                            _coment.AdjuntoName01 = file1.name;
                                            _coment.AdjuntoSize01 = file1.size;
                                        }
                                    }
                                }

                                WS_Remedy.Result exCom = imss.OrdenTrabajoAdicionaNotas(_coment);

                            }

                            //Actualiza estatus en Invgate
                            IncidentPutRequest VarInter = new IncidentPutRequest();
                            VarInter.id = id;
                            VarInter.statusId = idEstatusInvgate;

                            Entities.Intermedio.Result response_ = incidentes.PutIncidenteStatus(VarInter);
                        }
                    }
                    else if (_nota.Contains("@@P"))//Pendiente
                    {
                        string files = arrNota[1].Replace("Files:", "");
                        files = files.Substring(0, files.Length - 1);

                        //Obtiene Estatus
                        int idEstatusInvgate = 4;//En Espera
                        int idEstatusImss = catalogos.GetEstatusWOIMSS(idEstatusInvgate); 
                        //Obtiene Motivo Estado
                        int idMotivo = Convert.ToInt32(arrNota[0].Substring(3, 5).Trim());

                        WS_Remedy.OrdenTrabajo _request = new WS_Remedy.OrdenTrabajo();
                        _request.IDTicketInvgate = id.ToString();
                        _request.IDTicketRemedy = bitacora.TicketRemedy;
                        _request.EstadoNuevo = idEstatusImss;
                        _request.MotivoEstado = idMotivo;

                        WS_Remedy.Result exec = imss.OrdenTrabajoActualiza(_request);

                        result.Success = exec.Estatus;
                        result.Message = exec.Resultado;

                        if (result.Success)
                        {
                            //Si hay archivos, envia nota
                            if (!string.IsNullOrEmpty(files))
                            {
                                WS_Remedy.Comentario _coment = new WS_Remedy.Comentario();
                                _coment.IDTicketInvgate = id.ToString();
                                _coment.IDTicketRemedy = bitacora.TicketRemedy;
                                _coment.Notas = arrNota[0].Substring(8).Trim() == string.Empty ? "Adjuntos actualización estatus." : arrNota[0].Substring(8).Trim();

                                string[] arrFiles = files.Split(',');

                                if (arrFiles.Length >= 3)
                                {
                                    dynamic file1 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[0]));

                                    if (file1 != null)
                                    {
                                        if (Convert.ToBoolean(file1.success))
                                        {
                                            _coment.Adjunto01 = file1.attach;
                                            _coment.AdjuntoName01 = file1.name;
                                            _coment.AdjuntoSize01 = file1.size;
                                        }
                                    }


                                    dynamic file2 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[1]));

                                    if (file2 != null)
                                    {
                                        if (Convert.ToBoolean(file2.success))
                                        {
                                            _coment.Adjunto02 = file2.attach;
                                            _coment.AdjuntoName02 = file2.name;
                                            _coment.AdjuntoSize02 = file2.size;
                                        }
                                    }

                                    dynamic file3 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[2]));

                                    if (file3 != null)
                                    {
                                        if (Convert.ToBoolean(file3.success))
                                        {
                                            _coment.Adjunto03 = file3.attach;
                                            _coment.AdjuntoName03 = file3.name;
                                            _coment.AdjuntoSize03 = file3.size;
                                        }
                                    }

                                }

                                if (arrFiles.Length == 2)
                                {
                                    dynamic file1 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[0]));

                                    if (file1 != null)
                                    {
                                        if (Convert.ToBoolean(file1.success))
                                        {
                                            _coment.Adjunto01 = file1.attach;
                                            _coment.AdjuntoName01 = file1.name;
                                            _coment.AdjuntoSize01 = file1.size;
                                        }
                                    }


                                    dynamic file2 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[1]));

                                    if (file2 != null)
                                    {
                                        if (Convert.ToBoolean(file2.success))
                                        {
                                            _coment.Adjunto02 = file2.attach;
                                            _coment.AdjuntoName02 = file2.name;
                                            _coment.AdjuntoSize02 = file2.size;
                                        }
                                    }
                                }

                                if (arrFiles.Length == 1)
                                {
                                    dynamic file1 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[0]));

                                    if (file1 != null)
                                    {
                                        if (Convert.ToBoolean(file1.success))
                                        {
                                            _coment.Adjunto01 = file1.attach;
                                            _coment.AdjuntoName01 = file1.name;
                                            _coment.AdjuntoSize01 = file1.size;
                                        }
                                    }
                                }

                                WS_Remedy.Result exCom = imss.OrdenTrabajoAdicionaNotas(_coment);
                            }

                            //Actualiza estatus en Invgate
                            IncidentPutRequest VarInter = new IncidentPutRequest();
                            VarInter.id = id;
                            VarInter.statusId = idEstatusInvgate;

                            Entities.Intermedio.Result response_ = incidentes.PutIncidenteStatus(VarInter);
                        }
                    }
                    else
                    {
                        string files = arrNota[1].Replace("Files:", "");
                        files = files.Substring(0, files.Length - 1);

                        WS_Remedy.Comentario _request = new WS_Remedy.Comentario();
                        _request.IDTicketInvgate = id.ToString();
                        _request.IDTicketRemedy = bitacora.TicketRemedy;
                        _request.Notas = arrNota[0];


                        if (!string.IsNullOrEmpty(files))
                        {
                            string[] arrFiles = files.Split(',');

                            if (arrFiles.Length >= 3)
                            {
                                dynamic file1 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[0]));

                                if (file1 != null)
                                {
                                    if (Convert.ToBoolean(file1.success))
                                    {
                                        _request.Adjunto01 = file1.attach;
                                        _request.AdjuntoName01 = file1.name;
                                        _request.AdjuntoSize01 = file1.size;
                                    }
                                }


                                dynamic file2 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[1]));

                                if (file2 != null)
                                {
                                    if (Convert.ToBoolean(file2.success))
                                    {
                                        _request.Adjunto02 = file2.attach;
                                        _request.AdjuntoName02 = file2.name;
                                        _request.AdjuntoSize02 = file2.size;
                                    }
                                }

                                dynamic file3 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[2]));

                                if (file3 != null)
                                {
                                    if (Convert.ToBoolean(file3.success))
                                    {
                                        _request.Adjunto03 = file3.attach;
                                        _request.AdjuntoName03 = file3.name;
                                        _request.AdjuntoSize03 = file3.size;
                                    }
                                }
                            }

                            if (arrFiles.Length == 2)
                            {
                                dynamic file1 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[0]));

                                if (file1 != null)
                                {
                                    if (Convert.ToBoolean(file1.success))
                                    {
                                        _request.Adjunto01 = file1.attach;
                                        _request.AdjuntoName01 = file1.name;
                                        _request.AdjuntoSize01 = file1.size;
                                    }
                                }


                                dynamic file2 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[1]));

                                if (file2 != null)
                                {
                                    if (Convert.ToBoolean(file2.success))
                                    {
                                        _request.Adjunto02 = file2.attach;
                                        _request.AdjuntoName02 = file2.name;
                                        _request.AdjuntoSize02 = file2.size;
                                    }
                                }
                            }

                            if (arrFiles.Length == 1)
                            {
                                dynamic file1 = incidentes.GetAttachments(Convert.ToInt32(arrFiles[0]));

                                if (file1 != null)
                                {
                                    if (Convert.ToBoolean(file1.success))
                                    {
                                        _request.Adjunto01 = file1.attach;
                                        _request.AdjuntoName01 = file1.name;
                                        _request.AdjuntoSize01 = file1.size;
                                    }
                                }
                            }

                        }

                        WS_Remedy.Result exec = imss.OrdenTrabajoAdicionaNotas(_request);
                        result.Success = exec.Estatus;
                        result.Message = exec.Resultado;
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
                log.LogMsg("Error|WOAdicionaNotasAdjunto|Id: " + id.ToString() + "|" + ex.Message);
            }

            return result;
        }

        public Resultado WOAdicionaAdjunto(int id, int idFile)
        {
            Resultado result = new Resultado();

            try
            {
                //Consulta orden en bitacora
                SB.OrdenTrabajoData data = new SB.OrdenTrabajoData();
                SB.CatalogosData catalogos = new SB.CatalogosData();
                SB.OrdenTrabajo bitacora = data.GetIdIMSS(id);

                if (bitacora != null && bitacora.TicketInvgate > 0)
                {
                    WS_Remedy.Comentario _request = new WS_Remedy.Comentario();
                    _request.IDTicketInvgate = id.ToString();
                    _request.IDTicketRemedy = bitacora.TicketRemedy;
                    _request.Notas = "Adjuntos Ticket: " + bitacora.TicketRemedy;


                    dynamic file = incidentes.GetAttachments(idFile);

                    if (file != null)
                    {
                        if (Convert.ToBoolean(file.success))
                        {
                            _request.Adjunto01 = file.attach;
                            _request.AdjuntoName01 = file.name;
                            _request.AdjuntoSize01 = file.size;
                        }
                    }


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
                log.LogMsg("Error|WOAdicionaAdjunto|Id: " + id.ToString() + "|" + ex.Message);
            }

            return result;
        }

    }
}
