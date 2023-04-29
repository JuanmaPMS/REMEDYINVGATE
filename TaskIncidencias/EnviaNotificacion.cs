using ServiceBitacora;
using SB = ServiceBitacora;
using ServiceInvgate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskIncidencias.Models;
using Entities.Intermedio;
using Entities.Invgate;
using Email;

namespace TaskIncidencias
{
    public class EnviaNotificacion
    {
        IncidentesInvgate incidentes = new IncidentesInvgate();
        LogTask log = new LogTask();
        SB.IncidenteData dataI = new SB.IncidenteData();
        SB.OrdenTrabajoData dataO = new SB.OrdenTrabajoData();
        SB.NotificacionData notifica = new SB.NotificacionData();   
        Email.Notificacion email = new Email.Notificacion();

        public Resultado AsignaIncidente(int id, int ticket, int tipo)
        {
            Resultado result = new Resultado();

            try
            {
                if(tipo == 1) //Incidente
                {
                    SB.Incidente bitacora = dataI.GetIdIMSS(ticket);

                    if(bitacora != null)
                    {
                        //Obtiene consultores y agentes
                        List<NotificacionConsultor> consultores = notifica.GetConsultores(bitacora.CategoriaPro03);
                        List<NotificacionAgente> agentes = notifica.GetAgentes(bitacora.CategoriaPro03);

                        //Llena listas
                        List<string> destinatarios = new List<string>();
                        List<string> destinatariosCC = new List<string>();
                        List<IncidenteColaborador> colaboradores = new List<IncidenteColaborador>();

                        foreach(NotificacionConsultor consultor in consultores)
                        {
                            destinatarios.Add(consultor.Correo);
                            IncidenteColaborador colaborador = new IncidenteColaborador();
                            colaborador.IncidentId = bitacora.TicketInvgate;
                            colaborador.UserId = consultor.IdUsuarioInvgate;
                            colaborador.Type = 2; //Visor
                            colaboradores.Add(colaborador);
                        }

                        foreach (NotificacionAgente agente in agentes)
                        {
                            destinatariosCC.Add(agente.Correo);
                            IncidenteColaborador colaborador = new IncidenteColaborador();
                            colaborador.IncidentId = bitacora.TicketInvgate;
                            colaborador.UserId = agente.IdUsuarioInvgate;
                            colaborador.Type = 1; //Coordinador
                            colaboradores.Add(colaborador);
                        }

                        EmailBody emailBody = new EmailBody();
                        emailBody.Destinatarios = destinatarios;
                        emailBody.DestinatariosCC = destinatariosCC;
                        emailBody.Prioridad = bitacora.Impacto;
                        emailBody.Titulo = bitacora.Resumen;
                        emailBody.Descripcion = bitacora.Descripcion;
                        emailBody.TicketIMSS = bitacora.TicketRemedy;
                        emailBody.Requerimiento = bitacora.Req;
                        emailBody.Aplicativo = bitacora.CategoriaPro03;

                        if (email.Accesos(emailBody)) //Si envia correo, asigna ticket.
                        {
                            foreach(IncidenteColaborador colaborador in colaboradores)
                            {
                                incidentes.PostColaborador(colaborador);
                            }       
                        }
                    }                  
                    
                }
                else //OrdenTrabajo
                {
                    SB.OrdenTrabajo bitacora = dataO.GetIdIMSS(ticket);

                    if (bitacora != null)
                    {
                        //Obtiene consultores y agentes
                        List<NotificacionConsultor> consultores = notifica.GetConsultores(bitacora.CategoriaPro03);
                        List<NotificacionAgente> agentes = notifica.GetAgentes(bitacora.CategoriaPro03);

                        //Llena listas
                        List<string> destinatarios = new List<string>();
                        List<string> destinatariosCC = new List<string>();
                        List<IncidenteColaborador> colaboradores = new List<IncidenteColaborador>();

                        foreach (NotificacionConsultor consultor in consultores)
                        {
                            destinatarios.Add(consultor.Correo);
                            IncidenteColaborador colaborador = new IncidenteColaborador();
                            colaborador.IncidentId = bitacora.TicketInvgate;
                            colaborador.UserId = consultor.IdUsuarioInvgate;
                            colaborador.Type = 2; //Visor
                            colaboradores.Add(colaborador);
                        }

                        foreach (NotificacionAgente agente in agentes)
                        {
                            destinatariosCC.Add(agente.Correo);
                            IncidenteColaborador colaborador = new IncidenteColaborador();
                            colaborador.IncidentId = bitacora.TicketInvgate;
                            colaborador.UserId = agente.IdUsuarioInvgate;
                            colaborador.Type = 1; //Coordinador
                            colaboradores.Add(colaborador);
                        }

                        EmailBody emailBody = new EmailBody();
                        emailBody.Destinatarios = destinatarios;
                        emailBody.DestinatariosCC = destinatariosCC;
                        emailBody.Prioridad = bitacora.Prioridad;
                        emailBody.Titulo = bitacora.Resumen;
                        emailBody.Descripcion = bitacora.Descripcion;
                        emailBody.TicketIMSS = bitacora.TicketRemedy;
                        emailBody.Requerimiento = bitacora.Req;
                        emailBody.Aplicativo = bitacora.CategoriaPro03;

                        if (email.Accesos(emailBody)) //Si envia correo, asigna ticket.
                        {
                            foreach (IncidenteColaborador colaborador in colaboradores)
                            {
                                incidentes.PostColaborador(colaborador);
                            }
                        }

                    }
                }

                //Actualiza procesado
                notifica.Procesado(id);
            }
            catch (Exception ex)
            {
                log.LogMsg("Error|AsignaIncidente|Id:" + ticket + "|" + ex.Message);
            }

            return result;
        }
    }
}
