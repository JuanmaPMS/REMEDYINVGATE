using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Entities.Invgate;
using ServiceInvgate;
using ServiceBitacora;
using TaskIncidencias.Models;
using Helix = TaskIncidencias.WS_Helix;
using SB = ServiceBitacora;

namespace TaskIncidencias
{
    public class ProcesaRegistroHelix
    {
        private const long MaxBytes = 10L * 1024 * 1024;
        private readonly ServiciosHelix helix = new ServiciosHelix();
        private readonly IncidentesInvgate incidentes = new IncidentesInvgate();
        private readonly LogTask log = new LogTask();

        public Resultado IncidenteActualiza(int id, int idEstatus)
        {
            try
            {
                SB.Incidente bitacora = new SB.IncidenteData().GetIdIMSS(id);
                if (bitacora == null || bitacora.TicketInvgate <= 0)
                {
                    return TicketNoEncontrado();
                }

                if (IncidenteEnEstatusResolved(bitacora))
                {
                    return Exito("OK");
                }

                if (idEstatus != 1 && idEstatus != 2 && idEstatus != 3)
                {
                    return Exito("OK");
                }

                int idEstatusImss = new SB.CatalogosData().GetEstatusIncidenteIMSS(idEstatus);
                return EnviarActualizacionIncidente(id, bitacora.TicketRemedy, idEstatusImss, null, null, null, null, null);
            }
            catch (Exception ex)
            {
                return Error("IncidenteActualiza", id, ex);
            }
        }

        public Resultado IncidenteActualizaCategorizacion(int id, int idCategorizacion)
        {
            return AccionNoImplementada();
        }

        public Resultado IncidenteActualizaPrioridad(int id, int idPrioridad)
        {
            return AccionNoImplementada();
        }

        public Resultado IncidenteAdicionaNotas(int id, string nota)
        {
            return ProcesarNotaIncidente(id, nota, null, "IncidenteAdicionaNotas");
        }

        public Resultado IncidenteAdicionaNotasAdjunto(int id, string nota)
        {
            return ProcesarNotaIncidente(id, nota, ExtraerArchivos(nota), "IncidenteAdicionaNotasAdjunto");
        }

        public Resultado IncidenteAdicionaAdjunto(int id, int idFile)
        {
            try
            {
                SB.Incidente bitacora = new SB.IncidenteData().GetIdIMSS(id);
                if (bitacora == null || bitacora.TicketInvgate <= 0)
                {
                    return TicketNoEncontrado();
                }

                AttachmentResponse adjunto = incidentes.GetAttachments(idFile);
                List<AttachmentResponse> adjuntos = AdjuntoValido(adjunto);
                return EnviarNotaIncidente(id, bitacora.TicketRemedy, "Adjuntos Ticket: " + bitacora.TicketRemedy, adjuntos);
            }
            catch (Exception ex)
            {
                return Error("IncidenteAdicionaAdjunto", id, ex);
            }
        }

        public Resultado WOActualiza(int id, int idEstatus)
        {
            try
            {
                SB.OrdenTrabajo bitacora = new SB.OrdenTrabajoData().GetIdIMSS(id);
                if (bitacora == null || bitacora.TicketInvgate <= 0)
                {
                    return TicketNoEncontrado();
                }

                if (OrdenTrabajoEnEstatusComplete(bitacora))
                {
                    return Exito("OK");
                }

                if (idEstatus != 1 && idEstatus != 2 && idEstatus != 3)
                {
                    return Exito("OK");
                }

                int idEstatusImss = new SB.CatalogosData().GetEstatusWOIMSS(idEstatus);
                return EnviarActualizacionOrdenTrabajo(id, bitacora.TicketRemedy, idEstatusImss, null, null);
            }
            catch (Exception ex)
            {
                return Error("WOActualiza", id, ex);
            }
        }

        public Resultado WOActualizaCategorizacion(int id, int idCategorizacion)
        {
            return AccionNoImplementada();
        }

        public Resultado WOActualizaPrioridad(int id, int idPrioridad)
        {
            return AccionNoImplementada();
        }

        public Resultado WOAdicionaNotas(int id, string nota)
        {
            return ProcesarNotaOrdenTrabajo(id, nota, null, "WOAdicionaNotas");
        }

        public Resultado WOAdicionaNotasAdjunto(int id, string nota)
        {
            return ProcesarNotaOrdenTrabajo(id, nota, ExtraerArchivos(nota), "WOAdicionaNotasAdjunto");
        }

        public Resultado WOAdicionaAdjunto(int id, int idFile)
        {
            try
            {
                SB.OrdenTrabajo bitacora = new SB.OrdenTrabajoData().GetIdIMSS(id);
                if (bitacora == null || bitacora.TicketInvgate <= 0)
                {
                    return TicketNoEncontrado();
                }

                AttachmentResponse adjunto = incidentes.GetAttachments(idFile);
                List<AttachmentResponse> adjuntos = AdjuntoValido(adjunto);
                return EnviarNotaOrdenTrabajo(id, bitacora.TicketRemedy, "Adjuntos Ticket: " + bitacora.TicketRemedy, adjuntos);
            }
            catch (Exception ex)
            {
                return Error("WOAdicionaAdjunto", id, ex);
            }
        }

        private Resultado ProcesarNotaIncidente(int id, string nota, string archivos, string operacion)
        {
            if (EsNotaAutomatica(nota))
            {
                return Exito("Nota automatica");
            }

            try
            {
                SB.Incidente bitacora = new SB.IncidenteData().GetIdIMSS(id);
                if (bitacora == null || bitacora.TicketInvgate <= 0)
                {
                    return TicketNoEncontrado();
                }

                string notaLimpia = LimpiarNota(nota);
                string[] partes = notaLimpia.Split(new[] { "||" }, StringSplitOptions.None);
                List<AttachmentResponse> adjuntos = ObtenerAdjuntos(archivos ?? ExtraerArchivos(notaLimpia));

                if (notaLimpia.Contains("@@R"))
                {
                    if (IncidenteEnEstatusResolved(bitacora))
                    {
                        return EnviarNotaIncidente(id, bitacora.TicketRemedy, TextoResolucion(partes), adjuntos);
                    }

                    int idEstatusImss = new SB.CatalogosData().GetEstatusIncidenteIMSS(5);
                    int idMotivo = Convert.ToInt32(partes[0].Substring(3,5).Trim());
                    string[] categoriasResolucion = partes[1].Split(new[] { "|" }, StringSplitOptions.None);
                    Resultado resultado = EnviarActualizacionIncidente(id, bitacora.TicketRemedy, idEstatusImss, idMotivo, TextoResolucion(partes), categoriasResolucion[1], categoriasResolucion[2], categoriasResolucion[3]);
                    if (resultado.Success)
                    {
                        EnviarNotaIncidente(id, bitacora.TicketRemedy, TextoResolucion(partes), adjuntos);
                        ActualizarEstadoInvgate(id, 5);
                    }
                    return resultado;
                }

                if (notaLimpia.Contains("@@P"))
                {
                    if (IncidenteEnEstatusResolved(bitacora))
                    {
                        return EnviarNotaIncidente(id, bitacora.TicketRemedy, TextoPendiente(partes), adjuntos);
                    }

                    int idEstatusImss = new SB.CatalogosData().GetEstatusIncidenteIMSS(4);
                    int idMotivo = Convert.ToInt32(partes[0].Substring(3, 5).Trim());
                    Resultado resultado = EnviarActualizacionIncidente(id, bitacora.TicketRemedy, idEstatusImss, idMotivo, null, null, null, null);
                    if (resultado.Success)
                    {
                        EnviarNotaIncidente(id, bitacora.TicketRemedy, TextoPendiente(partes), adjuntos);
                        ActualizarEstadoInvgate(id, 4);
                    }
                    return resultado;
                }

                return EnviarNotaIncidente(id, bitacora.TicketRemedy, partes[0], adjuntos);
            }
            catch (Exception ex)
            {
                return Error(operacion, id, ex);
            }
        }

        private Resultado ProcesarNotaOrdenTrabajo(int id, string nota, string archivos, string operacion)
        {
            if (EsNotaAutomatica(nota))
            {
                return Exito("Nota automatica");
            }

            try
            {
                SB.OrdenTrabajo bitacora = new SB.OrdenTrabajoData().GetIdIMSS(id);
                if (bitacora == null || bitacora.TicketInvgate <= 0)
                {
                    return TicketNoEncontrado();
                }

                string notaLimpia = LimpiarNota(nota);
                string[] partes = notaLimpia.Split(new[] { "||" }, StringSplitOptions.None);
                List<AttachmentResponse> adjuntos = ObtenerAdjuntos(archivos ?? ExtraerArchivos(notaLimpia));

                if (notaLimpia.Contains("@@R"))
                {
                    if (OrdenTrabajoEnEstatusComplete(bitacora))
                    {
                        return EnviarNotaOrdenTrabajo(id, bitacora.TicketRemedy, TextoResolucion(partes), adjuntos);
                    }

                    int idEstatusImss = new SB.CatalogosData().GetEstatusWOIMSS(5);
                    int idMotivo = Convert.ToInt32(partes[0].Substring(3, 5).Trim());
                    Resultado resultado = EnviarActualizacionOrdenTrabajo(id, bitacora.TicketRemedy, idEstatusImss, idMotivo, TextoResolucion(partes));
                    if (resultado.Success)
                    {
                        EnviarNotaOrdenTrabajo(id, bitacora.TicketRemedy, TextoResolucion(partes), adjuntos);
                        ActualizarEstadoInvgate(id, 5);
                    }
                    return resultado;
                }

                if (notaLimpia.Contains("@@P"))
                {
                    if (OrdenTrabajoEnEstatusComplete(bitacora))
                    {
                        return EnviarNotaOrdenTrabajo(id, bitacora.TicketRemedy, TextoPendiente(partes), adjuntos);
                    }

                    int idEstatusImss = new SB.CatalogosData().GetEstatusWOIMSS(4);
                    int idMotivo = Convert.ToInt32(partes[0].Substring(3, 5).Trim());
                    Resultado resultado = EnviarActualizacionOrdenTrabajo(id, bitacora.TicketRemedy, idEstatusImss, idMotivo, null);
                    if (resultado.Success)
                    {
                        EnviarNotaOrdenTrabajo(id, bitacora.TicketRemedy, TextoPendiente(partes), adjuntos);
                        ActualizarEstadoInvgate(id, 4);
                    }
                    return resultado;
                }

                return EnviarNotaOrdenTrabajo(id, bitacora.TicketRemedy, partes[0], adjuntos);
            }
            catch (Exception ex)
            {
                return Error(operacion, id, ex);
            }
        }

        private Resultado EnviarActualizacionIncidente(int id, string ticketImss, int idEstatusImss, int? motivo, string resolucion, string categoriaResolucion1, string categoriaResolucion2, string categoriaResolucion3)
        {
            if (idEstatusImss <= 0)
            {
                return Exito("El estatus no existe en IMSS.");
            }

            Helix.ActualizacionIncidencia solicitud = new Helix.ActualizacionIncidencia
            {
                TipoOperacion = "Actualizacion",
                TipoTicket = "Incidencia",
                IdIncidencia = ticketImss,
                TicketProveedor = id.ToString(),
                EstadoNuevo = idEstatusImss.ToString(),
                MotivoEstadoNuevo = motivo.HasValue ? motivo.Value.ToString() : null,
                CategoriaResolucion1 = categoriaResolucion1,
                CategoriaResolucion2 = categoriaResolucion2,
                CategoriaResolucion3 = categoriaResolucion3,
                Resolucion = resolucion
            };

            Resultado resultado = DesdeRespuesta(helix.IncidenteActualiza(solicitud));
            if (EsCambioEstadoNoPermitidoIncidente(resultado.Message))
            {
                resultado.Success = true;
            }
            return resultado;
        }

        private Resultado EnviarActualizacionOrdenTrabajo(int id, string ticketImss, int idEstatusImss, int? motivo, string notaResolucion)
        {
            if (idEstatusImss <= 0)
            {
                return Exito("El estatus no existe en IMSS.");
            }

            Helix.ActualizacionOrdenTrabajo solicitud = new Helix.ActualizacionOrdenTrabajo
            {
                TipoOperacion = "Actualizacion",
                TipoTicket = "Orden de Trabajo",
                IdOrdenTrabajo = ticketImss,
                TicketProveedor = id.ToString(),
                EstadoNuevo = idEstatusImss.ToString(),
                MotivoEstadoNuevo = motivo.HasValue ? motivo.Value.ToString() : null,
                NotaResolucion = notaResolucion
            };

            Resultado resultado = DesdeRespuesta(helix.OrdenTrabajoActualiza(solicitud));
            if (EsCambioEstadoNoPermitidoOrdenTrabajo(resultado.Message))
            {
                resultado.Success = true;
            }
            return resultado;
        }

        private Resultado EnviarNotaIncidente(int id, string ticketImss, string nota, List<AttachmentResponse> adjuntos)
        {
            Helix.BitacoraTrabajoIncidencia solicitud = new Helix.BitacoraTrabajoIncidencia
            {
                TipoOperacion = "Bitacora de Trabajo",
                TipoTicket = "Incidencia",
                IdIncidencia = ticketImss,
                TicketProveedor = id.ToString(),
                Notas = nota
            };

            AsignarAdjuntos(solicitud, adjuntos);
            return DesdeRespuesta(helix.IncidenteAdicionaNotas(solicitud));
        }

        private Resultado EnviarNotaOrdenTrabajo(int id, string ticketImss, string nota, List<AttachmentResponse> adjuntos)
        {
            Helix.BitacoraTrabajoOrdenTrabajo solicitud = new Helix.BitacoraTrabajoOrdenTrabajo
            {
                TipoOperacion = "Bitacora de Trabajo",
                TipoTicket = "Orden de Trabajo",
                IdOrdenTrabajo = ticketImss,
                TicketProveedor = id.ToString(),
                Notas = nota
            };

            AsignarAdjuntos(solicitud, adjuntos);
            return DesdeRespuesta(helix.OrdenTrabajoAdicionaNotas(solicitud));
        }

        private static void AsignarAdjuntos(Helix.BitacoraTrabajoIncidencia solicitud, List<AttachmentResponse> adjuntos)
        {
            for (int indice = 0; indice < adjuntos.Count && indice < 3; indice++)
            {
                AttachmentResponse adjunto = adjuntos[indice];
                if (indice == 0)
                {
                    solicitud.Adjunto1Datos = adjunto.attach;
                    solicitud.Adjunto1Nombre = adjunto.name;
                    solicitud.Adjunto1Tamano = adjunto.size;
                }
                else if (indice == 1)
                {
                    solicitud.Adjunto2Datos = adjunto.attach;
                    solicitud.Adjunto2Nombre = adjunto.name;
                    solicitud.Adjunto2Tamano = adjunto.size;
                }
                else
                {
                    solicitud.Adjunto3Datos = adjunto.attach;
                    solicitud.Adjunto3Nombre = adjunto.name;
                    solicitud.Adjunto3Tamano = adjunto.size;
                }
            }
        }

        private static void AsignarAdjuntos(Helix.BitacoraTrabajoOrdenTrabajo solicitud, List<AttachmentResponse> adjuntos)
        {
            for (int indice = 0; indice < adjuntos.Count && indice < 3; indice++)
            {
                AttachmentResponse adjunto = adjuntos[indice];
                if (indice == 0)
                {
                    solicitud.Adjunto1Datos = adjunto.attach;
                    solicitud.Adjunto1Nombre = adjunto.name;
                    solicitud.Adjunto1Tamano = adjunto.size;
                }
                else if (indice == 1)
                {
                    solicitud.Adjunto2Datos = adjunto.attach;
                    solicitud.Adjunto2Nombre = adjunto.name;
                    solicitud.Adjunto2Tamano = adjunto.size;
                }
                else
                {
                    solicitud.Adjunto3Datos = adjunto.attach;
                    solicitud.Adjunto3Nombre = adjunto.name;
                    solicitud.Adjunto3Tamano = adjunto.size;
                }
            }
        }

        private List<AttachmentResponse> ObtenerAdjuntos(string archivos)
        {
            List<AttachmentResponse> resultado = new List<AttachmentResponse>();
            if (String.IsNullOrWhiteSpace(archivos))
            {
                return resultado;
            }

            foreach (string valor in archivos.Split(','))
            {
                int idAdjunto;
                if (!Int32.TryParse(valor.Trim(), out idAdjunto))
                {
                    continue;
                }

                AttachmentResponse adjunto = incidentes.GetAttachments(idAdjunto);
                if (adjunto != null && adjunto.success && adjunto.size <= MaxBytes)
                {
                    resultado.Add(adjunto);
                    if (resultado.Count == 3)
                    {
                        break;
                    }
                }
            }

            return resultado;
        }

        private static List<AttachmentResponse> AdjuntoValido(AttachmentResponse adjunto)
        {
            List<AttachmentResponse> resultado = new List<AttachmentResponse>();
            if (adjunto != null && adjunto.success && adjunto.size <= MaxBytes)
            {
                resultado.Add(adjunto);
            }
            return resultado;
        }

        private void ActualizarEstadoInvgate(int id, int idEstatus)
        {
            IncidentPutRequest solicitud = new IncidentPutRequest
            {
                id = id,
                statusId = idEstatus
            };

            incidentes.PutIncidenteStatus(solicitud);
        }

        private static string EstadoIncidente(int idEstatus)
        {
            switch (idEstatus)
            {
                case 1: return "Assigned";
                case 2: return "In Progress";
                case 3:
                case 4: return "Pending";
                case 5: return "Resolved";
                default: return null;
            }
        }

        private static string EstadoOrdenTrabajo(int idEstatus)
        {
            switch (idEstatus)
            {
                case 1: return "Assigned";
                case 2: return "In Progress";
                case 3:
                case 4: return "Pending";
                case 5: return "Completed";
                default: return null;
            }
        }

        private static string LimpiarNota(string nota)
        {
            return Regex.Replace(nota ?? String.Empty, "<.*?>", String.Empty);
        }

        private static string ExtraerArchivos(string nota)
        {
            if (String.IsNullOrWhiteSpace(nota))
            {
                return String.Empty;
            }

            int indice = nota.IndexOf("Files:", StringComparison.OrdinalIgnoreCase);
            if (indice < 0)
            {
                return String.Empty;
            }

            string archivos = nota.Substring(indice + "Files:".Length).Trim();
            return archivos.TrimEnd(']', ')', '}');
        }

        private static string TextoResolucion(string[] partes)
        {
            if (partes.Length > 2 && !String.IsNullOrWhiteSpace(partes[2]))
            {
                return partes[2].Trim();
            }

            return TextoPendiente(partes);
        }

        private static string TextoPendiente(string[] partes)
        {
            string encabezado = partes.Length == 0 ? String.Empty : partes[0];
            return encabezado.Length > 8 ? encabezado.Substring(8).Trim() : encabezado.Trim();
        }

        private static Resultado DesdeRespuesta(Helix.Result respuesta)
        {
            return new Resultado
            {
                Success = respuesta != null && respuesta.Estatus,
                Message = respuesta == null ? "No se obtuvo respuesta del servidor." : respuesta.Resultado
            };
        }

        private static bool EsCambioEstadoNoPermitidoIncidente(string mensaje)
        {
            return EsCambioEstadoNoPermitido(mensaje, "Resolved", "Closed", "In Progress");
        }

        private static bool EsCambioEstadoNoPermitidoOrdenTrabajo(string mensaje)
        {
            return EsCambioEstadoNoPermitido(mensaje, "Completed", "Closed", "Cancelled", "In Progress");
        }

        private static bool EsCambioEstadoNoPermitido(string mensaje, params string[] estatusFinales)
        {
            if (String.IsNullOrEmpty(mensaje) ||
                !mensaje.Contains("ERROR: El cambio de estado") ||
                !mensaje.Contains("no está permitido") ||
                !mensaje.Contains("estatus actual"))
            {
                return false;
            }

            foreach (string estatusFinal in estatusFinales)
            {
                if (mensaje.Contains("estatus actual (" + estatusFinal + ")"))
                {
                    return true;
                }
            }

            return false;
        }

        private static Resultado AccionNoImplementada()
        {
            return Exito("Acción no implementada");
        }

        private static Resultado TicketNoEncontrado()
        {
            return new Resultado
            {
                Success = false,
                Message = "No se encontró el ticket de Invgate en la bitácora."
            };
        }

        private static Resultado Exito(string mensaje)
        {
            return new Resultado
            {
                Success = true,
                Message = mensaje
            };
        }

        private static bool IncidenteEnEstatusResolved(SB.Incidente bitacora)
        {
            return bitacora != null && String.Equals((bitacora.Estado ?? String.Empty).Trim(), "4", StringComparison.Ordinal);
        }

        private static bool OrdenTrabajoEnEstatusComplete(SB.OrdenTrabajo bitacora)
        {
            return bitacora != null && String.Equals((bitacora.Estado ?? String.Empty).Trim(), "5", StringComparison.Ordinal);
        }

        private static bool EsNotaAutomatica(string nota)
        {
            return !String.IsNullOrEmpty(nota) && nota.IndexOf("[No message provided]", StringComparison.Ordinal) >= 0;
        }

        private Resultado Error(string operacion, int id, Exception ex)
        {
            log.LogMsg("Error|" + operacion + "|Id: " + id + "|" + ex.Message);
            return new Resultado
            {
                Success = false,
                Message = ex.Message
            };
        }
    }
}
