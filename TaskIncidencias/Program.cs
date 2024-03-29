﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Invgate;
using ServiceBitacora;
using ServiceInvgate;
using TaskIncidencias.Models;

namespace TaskIncidencias
{
    internal class Program
    {
        static void Main(string[] args)
        {
            LogTask log = new LogTask();
            try
            {
                RegistrosInvgate registros = new RegistrosInvgate();
                ProcesaRegistro procesa = new ProcesaRegistro();              

                List<RegistrosRequest> list = registros.Get().OrderBy(x => x.fecha).ToList();

                List<int> procesados = new List<int>();
                foreach (RegistrosRequest req in list)
                {
                    Resultado resultado = new Resultado();
                    if (req.idTipoSolicitud == 1) //Incidentes
                    {
                        switch (req.tipoControl)
                        {
                            case "ESTATUS":
                                resultado = procesa.IncidenteActualiza(req.idIncidencia, req.identificadorNum);
                                break;
                            case "CATEGORIZACION":
                                resultado = procesa.IncidenteActualizaCategorizacion(req.idIncidencia, req.identificadorNum);
                                break;
                            case "PRIORIDAD":
                                resultado = procesa.IncidenteActualizaPrioridad(req.idIncidencia, req.identificadorNum);
                                break;
                            case "NOTA":
                                resultado = procesa.IncidenteAdicionaNotas(req.idIncidencia, req.identificadorAlfa);
                                break;
                            case "NOTA_ADJUNTO":
                                resultado = procesa.IncidenteAdicionaNotasAdjunto(req.idIncidencia, req.identificadorAlfa);
                                break;
                            case "ADJUNTO":
                                resultado = procesa.IncidenteAdicionaAdjunto(req.idIncidencia, req.identificadorNum);
                                break;
                        }
                    }
                    else if (req.idTipoSolicitud == 2) //WO
                    {
                        switch (req.tipoControl)
                        {
                            case "ESTATUS":
                                resultado = procesa.WOActualiza(req.idIncidencia, req.identificadorNum);
                                break;
                            case "CATEGORIZACION":
                                resultado = procesa.WOActualizaCategorizacion(req.idIncidencia, req.identificadorNum);
                                break;
                            case "PRIORIDAD":
                                resultado = procesa.WOActualizaPrioridad(req.idIncidencia, req.identificadorNum);
                                break;
                            case "NOTA":
                                resultado = procesa.WOAdicionaNotas(req.idIncidencia, req.identificadorAlfa);
                                break;
                            case "NOTA_ADJUNTO":
                                resultado = procesa.WOAdicionaNotasAdjunto(req.idIncidencia, req.identificadorAlfa);
                                break;
                            case "ADJUNTO":
                                resultado = procesa.WOAdicionaAdjunto(req.idIncidencia, req.identificadorNum);
                                break;
                        }
                    }

                    if (resultado.Success)
                    { procesados.Add(req.id); }
                    else
                    { log.LogMsg("Error|" + req.tipoControl + "|Id: " + req.idIncidencia.ToString() + "|" + resultado.Message); }
                }

                bool actualiza = false;
                if (procesados.Count > 0)
                    actualiza = registros.Procesados(procesados);

                Console.WriteLine(actualiza);

                ////Notificaciones
                //NotificacionData notifica = new NotificacionData();
                //EnviaNotificacion asigna =  new EnviaNotificacion();
                //List<Notificacion> asignaciones = notifica.Get();
                //foreach(Notificacion notificacion in asignaciones)
                //{
                //    asigna.AsignaIncidente(notificacion.id, notificacion.TicketInvgate, notificacion.Tipo.Value);
                //}
            }
            catch(Exception ex) 
            {
                log.LogMsg("Error|ProgramTask|" + ex.Message);
            }

        }
    }
}
