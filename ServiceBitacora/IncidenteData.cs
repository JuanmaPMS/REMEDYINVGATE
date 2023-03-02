using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Intermedio;
using System.Data.SqlClient;
using Entities;

namespace ServiceBitacora
{
    public class IncidenteData
    {
        private InterEntities ctx = null;
        private Log log_ = new Log();

        public IncidenteData()
        {
            this.ctx = new InterEntities();
        }

        public Ticket Get(string ticketImss)
        {
            Ticket result = new Ticket();
            try
            {
                result = ctx.Database.SqlQuery<Ticket>("EXEC [dbo].[SP_OBTIENE_TICKET_INVGATE_INCIDENTE] @TicketIMSS", new SqlParameter("@TicketIMSS", ticketImss)).FirstOrDefault();
            }
            catch(Exception ex)
            {
                log_.LogMsg("Error|Get Ticket Invgate BD:" + ex.Message);
            }
            return result;
        }

        public Incidente Get(int ticketInvgate)
        {
            Incidente datos = new Incidente();
            try
            {
                datos = ctx.Incidente.Where(x => x.TicketInvgate == ticketInvgate).FirstOrDefault();
            }
            catch (Exception ex)
            {
                log_.LogMsg("Error|Get Ticket IMSS BD:" + ex.Message);
            }
            return datos;
        }

        public bool Existe(string ticketImss)
        {
            try
            {
                Incidente incidente = ctx.Incidente.Where(x => x.TicketRemedy.Trim().ToUpper() == ticketImss.Trim().ToUpper()).FirstOrDefault();
                if (incidente != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                log_.LogMsg("Error|Existe Ticket IMSS BD:" + ex.Message);
                return false;
            }
        }

        public int Crear(CreaTicket ticket, int idInvgate, out string Result)
        {
            int id = 0;
            try
            {
                SqlParameter[] sqParam = new SqlParameter[] {
                                    new SqlParameter("@TicketRemedy", ticket.TicketIMSS),
                                    new SqlParameter("@TicketInvgate", idInvgate),
                                    new SqlParameter("@Descripcion", ticket.Descripcion == null ? string.Empty : ticket.Descripcion),
                                    new SqlParameter("@Resumen", ticket.Resumen == null ? string.Empty : ticket.Resumen),
                                    new SqlParameter("@Impacto", ticket.Impacto == null ? string.Empty : ticket.Impacto),
                                    new SqlParameter("@Urgencia", ticket.Urgencia == null ? string.Empty : ticket.Urgencia),
                                    new SqlParameter("@TipoIncidencia", ticket.TipoIncidencia == null ? string.Empty : ticket.TipoIncidencia),
                                    new SqlParameter("@FuenteReportada", ticket.FuenteReportada== null ? string.Empty : ticket.FuenteReportada),
                                    new SqlParameter("@NombreProducto", ticket.NombreProducto == null ? string.Empty : ticket.NombreProducto),
                                    new SqlParameter("@GrupoSoporte", ticket.GrupoSoporte== null ? string.Empty : ticket.GrupoSoporte),
                                    new SqlParameter("@CategoriaOpe01", ticket.CategoriaOpe01 == null ? string.Empty : ticket.CategoriaOpe01),
                                    new SqlParameter("@CategoriaOpe02", ticket.CategoriaOpe02 == null ? string.Empty : ticket.CategoriaOpe02),
                                    new SqlParameter("@CategoriaOpe03", ticket.CategoriaOpe03 == null ? string.Empty : ticket.CategoriaOpe03),
                                    new SqlParameter("@CategoriaPro01", ticket.CategoriaPro01 == null ? string.Empty : ticket.CategoriaPro01),
                                    new SqlParameter("@CategoriaPro02", ticket.CategoriaPro02 == null ? string.Empty : ticket.CategoriaPro02),
                                    new SqlParameter("@CategoriaPro03", ticket.CategoriaPro03 == null ? string.Empty : ticket.CategoriaPro03),
                                    new SqlParameter("@Estado", ticket.EstadoNuevo == null ? "1" : ticket.EstadoNuevo),
                                    new SqlParameter("@FechaCreacion", ticket.FechaCreacion == null ? string.Empty : ticket.FechaCreacion.ToString()),
                                    new SqlParameter("@Cliente", ticket.Cliente == null ? string.Empty : ticket.Cliente),
                                    new SqlParameter("@VIP", ticket.VIP.ToUpper() == "SI" ? 1 : 0),
                                    new SqlParameter("@Sensibilidad", ticket.Sensibilidad == null ? string.Empty : ticket.Sensibilidad),
                                    new SqlParameter("@Usuario", "WsIntermediario"),
                                    new SqlParameter("@Nota", ticket.Notas == null ? string.Empty : ticket.Notas),
                                    new SqlParameter("@Archivo01", ticket.Adjunto01 == null ? string.Empty : ticket.Adjunto01),
                                    new SqlParameter("@ArchivoName01", ticket.AdjuntoName01 == null ? string.Empty : ticket.AdjuntoName01),
                                    new SqlParameter("@ArchivoSize01", ticket.AdjuntoSize01 == null ? string.Empty : ticket.AdjuntoSize01),
                                    new SqlParameter("@Archivo02", ticket.Adjunto02 == null ? string.Empty : ticket.Adjunto02),
                                    new SqlParameter("@ArchivoName02", ticket.AdjuntoName02 == null ? string.Empty : ticket.AdjuntoName02),
                                    new SqlParameter("@ArchivoSize02", ticket.AdjuntoSize02 == null ? string.Empty : ticket.AdjuntoSize02),
                                    new SqlParameter("@Archivo03", ticket.Adjunto03 == null ? string.Empty : ticket.Adjunto03),
                                    new SqlParameter("@ArchivoName03", ticket.AdjuntoName03 == null ? string.Empty : ticket.AdjuntoName03),
                                    new SqlParameter("@ArchivoSize03", ticket.AdjuntoSize03 == null ? string.Empty : ticket.AdjuntoSize03)
                };

                id = ctx.Database.ExecuteSqlCommand("EXEC [dbo].[SP_INSERT_INCIDENTE] @TicketRemedy,@TicketInvgate,@Descripcion,@Resumen,@Impacto,@Urgencia,@TipoIncidencia,@FuenteReportada,@NombreProducto,@GrupoSoporte,@CategoriaOpe01,@CategoriaOpe02,@CategoriaOpe03,@CategoriaPro01,@CategoriaPro02,@CategoriaPro03,@Estado,@FechaCreacion,@Cliente,@VIP,@Sensibilidad,@Usuario,@Nota,@Archivo01,@ArchivoName01,@ArchivoSize01,@Archivo02,@ArchivoName02,@ArchivoSize02,@Archivo03,@ArchivoName03,@ArchivoSize03 ", sqParam);
                Result = "Exito: Incidente registrado en bitácora.";
            }
            catch (Exception ex)
            {
                Result = "Error:" + ex.Message;
                log_.LogMsg("Error|Crea Ticket BD:" + ex.Message);
            }

            return id;
        }

        public int ActualizaIncidente(ActualizaTicket ticket, int idInvgate, out string Result)
        {
            int id = 0;
            try
            {
                List<SqlParameter> sqParam = new List<SqlParameter> {
                                    new SqlParameter("@TicketRemedy", ticket.TicketIMSS),
                                    new SqlParameter("@TicketInvgate", idInvgate),
                                    new SqlParameter("@FechaCambio", ticket.FechaCambio == null ? string.Empty : ticket.FechaCambio.ToString()),
                                    new SqlParameter("@Nota", ticket.Notas == null ? string.Empty : ticket.Notas),
                                    new SqlParameter("@Usuario", "WsIntermediario")
                                    
                };

                if (!string.IsNullOrEmpty(ticket.Impacto))
                    sqParam.Add(new SqlParameter("@Impacto", ticket.Impacto));
                else
                    sqParam.Add(new SqlParameter("@Impacto", DBNull.Value));

                if (!string.IsNullOrEmpty(ticket.Urgencia))
                    sqParam.Add(new SqlParameter("@Urgencia", ticket.Urgencia));
                else
                    sqParam.Add(new SqlParameter("@Urgencia", DBNull.Value));

                if (!string.IsNullOrEmpty(ticket.EstadoNuevo))
                    sqParam.Add(new SqlParameter("@Estado", ticket.EstadoNuevo));
                else
                    sqParam.Add(new SqlParameter("@Estado", DBNull.Value));

                if (!string.IsNullOrEmpty(ticket.Motivo))
                    sqParam.Add(new SqlParameter("@Motivo", ticket.Motivo));
                else
                    sqParam.Add(new SqlParameter("@Motivo", DBNull.Value));

                id = ctx.Database.ExecuteSqlCommand("EXEC [dbo].[SP_UPDATE_INCIDENTE] @TicketRemedy,@TicketInvgate,@FechaCambio,@Impacto,@Urgencia,@Estado,@Motivo,@Nota,@Usuario ", sqParam.ToArray());
                Result = "Exito: Actualización de incidente registrado en bitácora.";
            }
            catch (Exception ex)
            {
                Result = "Error:" + ex.Message;
                log_.LogMsg("Error|Actualiza Incidente BD:" + ex.Message);
            }

            return id;
        }

        public int ActualizaCategoria(ActualizaCategorizacion ticket, int idInvgate, out string Result)
        {
            int id = 0;
            try
            {
                List<SqlParameter> sqParam = new List<SqlParameter> {
                                    new SqlParameter("@TicketRemedy", ticket.TicketIMSS),
                                    new SqlParameter("@TicketInvgate", idInvgate),
                                    new SqlParameter("@FechaCambio", ticket.FechaCambio == null ? string.Empty : ticket.FechaCambio.ToString()),
                                    new SqlParameter("@CategoriaOpe01", ticket.CategoriaOpe01 == null ? string.Empty : ticket.CategoriaOpe01),
                                    new SqlParameter("@CategoriaOpe02", ticket.CategoriaOpe02 == null ? string.Empty : ticket.CategoriaOpe02),
                                    new SqlParameter("@CategoriaOpe03", ticket.CategoriaOpe03 == null ? string.Empty : ticket.CategoriaOpe03),
                                    new SqlParameter("@CategoriaPro01", ticket.CategoriaPro01 == null ? string.Empty : ticket.CategoriaPro01),
                                    new SqlParameter("@CategoriaPro02", ticket.CategoriaPro02 == null ? string.Empty : ticket.CategoriaPro02),
                                    new SqlParameter("@CategoriaPro03", ticket.CategoriaPro03 == null ? string.Empty : ticket.CategoriaPro03),
                                    new SqlParameter("@Usuario", "")
                };

                if (!string.IsNullOrEmpty(ticket.GrupoSoporte))
                    sqParam.Add(new SqlParameter("@GrupoSoporte", ticket.GrupoSoporte));
                else
                    sqParam.Add(new SqlParameter("@GrupoSoporte", DBNull.Value));

                id = ctx.Database.ExecuteSqlCommand("EXEC [dbo].[SP_UPDATE_CATEGORIA_INCIDENTE] @TicketRemedy,@TicketInvgate,@FechaCambio,@GrupoSoporte,@CategoriaOpe01,@CategoriaOpe02,@CategoriaOpe03,@CategoriaPro01,@CategoriaPro02,@CategoriaPro03,@Usuario ", sqParam.ToArray());
                Result = "Exito: Actualización de incidente registrado en bitácora.";
            }
            catch (Exception ex)
            {
                Result = "Error:" + ex.Message;
                log_.LogMsg("Error|Actualiza Categoria Incidente BD:" + ex.Message);
            }

            return id;
        }

        public int ActualizaPriorizacion(ActualizaPriorizacion ticket, int idInvgate, out string Result)
        {
            int id = 0;
            try
            {
                List<SqlParameter> sqParam = new List<SqlParameter> {
                                    new SqlParameter("@TicketRemedy", ticket.TicketIMSS),
                                    new SqlParameter("@TicketInvgate", idInvgate),
                                    new SqlParameter("@FechaCambio", ticket.FechaCambio == null ? string.Empty : ticket.FechaCambio.ToString()),
                                    new SqlParameter("@Usuario", "WsIntermediario")
                };

                if (!string.IsNullOrEmpty(ticket.Impacto))
                    sqParam.Add(new SqlParameter("@Impacto", ticket.Impacto));
                else
                    sqParam.Add(new SqlParameter("@Impacto", DBNull.Value));
                if (!string.IsNullOrEmpty(ticket.Urgencia))
                    sqParam.Add(new SqlParameter("@Urgencia", ticket.Urgencia));
                else
                    sqParam.Add(new SqlParameter("@Urgencia", DBNull.Value));

                id = ctx.Database.ExecuteSqlCommand("EXEC [dbo].[SP_UPDATE_PRIORIDAD_INCIDENTE] @TicketRemedy,@TicketInvgate,@FechaCambio,@Impacto,@Urgencia,@Usuario ", sqParam.ToArray());
                Result = "Exito: Actualización de incidente registrado en bitácora.";
            }
            catch (Exception ex)
            {
                Result = "Error:" + ex.Message;
                log_.LogMsg("Error|Actualiza Priorizacion Incidente BD:" + ex.Message);
            }

            return id;
        }

        public int AgregaNota(AgregaNota ticket, int idInvgate, out string Result)
        {
            int id = 0;
            try
            {
                SqlParameter[] sqParam = new SqlParameter[] {
                                    new SqlParameter("@TicketRemedy", ticket.TicketIMSS),
                                    new SqlParameter("@TicketInvgate", idInvgate),
                                    new SqlParameter("@Nota", ticket.Notas == null ? string.Empty : ticket.Notas),
                                    new SqlParameter("@Usuario", "WsIntermediario")
                };

                id = ctx.Database.ExecuteSqlCommand("EXEC [dbo].[SP_INSERT_NOTA_INCIDENTE] @TicketRemedy,@TicketInvgate,@Nota,@Usuario ", sqParam);
                Result = "Exito: Nota registrada en bitácora.";
            }
            catch (Exception ex)
            {
                Result = "Error:" + ex.Message;
                log_.LogMsg("Error|Agrega Nota Incidente BD:" + ex.Message);
            }

            return id;
        }


    }
}
