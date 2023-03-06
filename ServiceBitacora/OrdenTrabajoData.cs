using Entities.Intermedio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBitacora
{
    public class OrdenTrabajoData
    {
        private InterEntities ctx = null;
        private Log log_ = new Log();

        public OrdenTrabajoData()
        {
            this.ctx = new InterEntities();
        }

        public Ticket Get(string ticketImss)
        {
            Ticket result = new Ticket();
            try
            {
                result = ctx.Database.SqlQuery<Ticket>("EXEC [dbo].[SP_OBTIENE_TICKET_INVGATE_WO] @TicketIMSS", new SqlParameter("@TicketIMSS", ticketImss)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                log_.LogMsg("Error|Get Ticket Invgate BD:" + ex.Message);
            }
            return result;
        }

        public OrdenTrabajo Get(int ticketInvgate)
        {
            OrdenTrabajo datos = new OrdenTrabajo();
            try
            {
                datos = ctx.OrdenTrabajo.Where(x => x.TicketInvgate == ticketInvgate).FirstOrDefault();
            }
            catch(Exception ex)
            {
                log_.LogMsg("Error|Get Ticket IMSS BD:" + ex.Message);
            }
            return datos;
        }

        public bool Existe(string ticketImss)
        {
            try
            {
                OrdenTrabajo wo = ctx.OrdenTrabajo.Where(x => x.TicketRemedy.Trim().ToUpper() == ticketImss.Trim().ToUpper()).FirstOrDefault();
                if (wo != null)
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

        public int Crear(CreaTicketWO ticket, int idInvgate, out string Result)
        {
            int id = 0;
            try
            {
                List<SqlParameter> sqParam = new List<SqlParameter> {
                                    new SqlParameter("@TicketRemedy", ticket.TicketIMSS),
                                    new SqlParameter("@TicketInvgate", idInvgate),
                                    new SqlParameter("@Descripcion", ticket.Descripcion == null ? string.Empty : ticket.Descripcion),
                                    new SqlParameter("@Resumen", ticket.Resumen == null ? string.Empty : ticket.Resumen),
                                    new SqlParameter("@Prioridad", ticket.Prioridad),
                                    new SqlParameter("@FuenteReportada", ticket.FuenteReportada == null ? string.Empty : ticket.FuenteReportada),
                                    new SqlParameter("@NombreProducto", ticket.NombreProducto == null ? string.Empty : ticket.NombreProducto),
                                    new SqlParameter("@GrupoSoporte", ticket.GrupoSoporte == null ? string.Empty : ticket.GrupoSoporte),
                                    new SqlParameter("@CategoriaOpe01", ticket.CategoriaOpe01 == null ? string.Empty : ticket.CategoriaOpe01),
                                    new SqlParameter("@CategoriaOpe02", ticket.CategoriaOpe02 == null ? string.Empty : ticket.CategoriaOpe02),
                                    new SqlParameter("@CategoriaOpe03", ticket.CategoriaOpe03 == null ? string.Empty : ticket.CategoriaOpe03),
                                    new SqlParameter("@CategoriaPro01", ticket.CategoriaPro01 == null ? string.Empty : ticket.CategoriaPro01),
                                    new SqlParameter("@CategoriaPro02", ticket.CategoriaPro02 == null ? string.Empty : ticket.CategoriaPro02),
                                    new SqlParameter("@CategoriaPro03", ticket.CategoriaPro03 == null ? string.Empty : ticket.CategoriaPro03),
                                    new SqlParameter("@Estado", ticket.EstadoNuevo == null ? string.Empty : ticket.EstadoNuevo),
                                    new SqlParameter("@FechaCreacion", ticket.FechaCreacion == null ? string.Empty : ticket.FechaCreacion.ToString()),
                                    new SqlParameter("@Cliente", ticket.Cliente == null ? string.Empty : ticket.Cliente),
                                    new SqlParameter("@VIP", ticket.VIP.ToUpper() == "SI" ? 1 : 0),
                                    new SqlParameter("@Sensibilidad", ticket.Sensibilidad == null ? string.Empty : ticket.Sensibilidad),
                                    new SqlParameter("@Usuario", "WsIntermediario"),
                                    new SqlParameter("@Nota", ticket.Notas == null ? string.Empty : ticket.Notas),new SqlParameter("@ArchivoName01", ticket.AdjuntoName01 == null ? string.Empty : ticket.AdjuntoName01),
                                    new SqlParameter("@ArchivoSize01", ticket.AdjuntoSize01 == null ? 0 : ticket.AdjuntoSize01),
                                    new SqlParameter("@ArchivoName02", ticket.AdjuntoName02 == null ? string.Empty : ticket.AdjuntoName02),
                                    new SqlParameter("@ArchivoSize02", ticket.AdjuntoSize02 == null ? 0: ticket.AdjuntoSize02),
                                    new SqlParameter("@ArchivoName03", ticket.AdjuntoName03 == null ? string.Empty : ticket.AdjuntoName03),
                                    new SqlParameter("@ArchivoSize03", ticket.AdjuntoSize03 == null ? 0 : ticket.AdjuntoSize03)
                };

                if (ticket.Adjunto01 == null)
                    sqParam.Add(new SqlParameter("@Archivo01", DBNull.Value));
                else
                    sqParam.Add(new SqlParameter("@Archivo01", ticket.Adjunto01));
                if (ticket.Adjunto02 == null)
                    sqParam.Add(new SqlParameter("@Archivo02", DBNull.Value));
                else
                    sqParam.Add(new SqlParameter("@Archivo02", ticket.Adjunto02));
                if (ticket.Adjunto03 == null)
                    sqParam.Add(new SqlParameter("@Archivo03", DBNull.Value));
                else
                    sqParam.Add(new SqlParameter("@Archivo03", ticket.Adjunto03));

                id = ctx.Database.ExecuteSqlCommand("EXEC [dbo].[SP_INSERT_WO] @TicketRemedy,@TicketInvgate,@Descripcion,@Resumen,@Prioridad,@FuenteReportada,@NombreProducto,@GrupoSoporte,@CategoriaOpe01,@CategoriaOpe02,@CategoriaOpe03,@CategoriaPro01,@CategoriaPro02,@CategoriaPro03,@Estado,@FechaCreacion,@Cliente,@VIP,@Sensibilidad,@Usuario,@Nota,@Archivo01,@ArchivoName01,@ArchivoSize01,@Archivo02,@ArchivoName02,@ArchivoSize02,@Archivo03,@ArchivoName03,@ArchivoSize03 ", sqParam.ToArray());
                Result = "Exito: Orden de Trabajo registrada en bitácora.";
            }
            catch (Exception ex)
            {
                Result = "Error:" + ex.Message;
                log_.LogMsg("Error|Crea Ticket BD:" + ex.Message);
            }

            return id;
        }

        public int ActualizaOrdenTrabajo(ActualizaTicketWO ticket, int idInvgate, out string Result)
        {
            int id = 0;
            try
            {
                List<SqlParameter> sqParam = new List<SqlParameter> {
                                    new SqlParameter("@TicketRemedy", ticket.TicketIMSS),
                                    new SqlParameter("@TicketInvgate", idInvgate),
                                    new SqlParameter("@FechaCambio", ticket.FechaCambio == null ? string.Empty : ticket.FechaCambio.ToString() ),
                                    new SqlParameter("@Nota", ticket.Notas == null ? string.Empty : ticket.Notas),
                                    new SqlParameter("@Usuario", "WsIntermediario")

                };

                if (ticket.Prioridad == null)
                    sqParam.Add(new SqlParameter("@Prioridad", DBNull.Value));
                else
                    sqParam.Add(new SqlParameter("@Prioridad", ticket.Prioridad));
                if (!string.IsNullOrEmpty(ticket.EstadoNuevo))
                    sqParam.Add(new SqlParameter("@Estado", ticket.EstadoNuevo));
                else
                    sqParam.Add(new SqlParameter("@Estado", DBNull.Value));

                if (!string.IsNullOrEmpty(ticket.Motivo))
                    sqParam.Add(new SqlParameter("@Motivo", ticket.Motivo));
                else
                    sqParam.Add(new SqlParameter("@Motivo", DBNull.Value));

                id = ctx.Database.ExecuteSqlCommand("EXEC [dbo].[SP_UPDATE_WO] @TicketRemedy,@TicketInvgate,@FechaCambio,@Prioridad,@Estado,@Motivo,@Nota,@Usuario ", sqParam.ToArray());
                Result = "Exito: Actualización de Orden de Trabajo registrado en bitácora.";
            }
            catch (Exception ex)
            {
                log_.LogMsg("Error|Actualiza OC BD:" + ex.Message);
                Result = "Error:" + ex.Message;
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
                                    new SqlParameter("@Usuario", "WsIntermediario")
                };

                if (!string.IsNullOrEmpty(ticket.GrupoSoporte))
                    sqParam.Add(new SqlParameter("@GrupoSoporte", ticket.GrupoSoporte));
                else
                    sqParam.Add(new SqlParameter("@GrupoSoporte", DBNull.Value));

                id = ctx.Database.ExecuteSqlCommand("EXEC [dbo].[SP_UPDATE_CATEGORIA_WO] @TicketRemedy,@TicketInvgate,@FechaCambio,@GrupoSoporte,@CategoriaOpe01,@CategoriaOpe02,@CategoriaOpe03,@CategoriaPro01,@CategoriaPro02,@CategoriaPro03,@Usuario ", sqParam.ToArray());
                Result = "Exito: Actualización de Orden de Trabajo registrado en bitácora.";
            }
            catch (Exception ex)
            {
                Result = "Error:" + ex.Message;
                log_.LogMsg("Error|Actualiza Categoria OC BD:" + ex.Message);
            }

            return id;
        }

        public int ActualizaPriorizacion(ActualizaPriorizacionWO ticket, int idInvgate, out string Result)
        {
            int id = 0;
            try
            {
                SqlParameter[] sqParam = new SqlParameter[] {
                                    new SqlParameter("@TicketRemedy", ticket.TicketIMSS),
                                    new SqlParameter("@TicketInvgate", idInvgate),
                                    new SqlParameter("@FechaCambio", ticket.FechaCambio == null ? string.Empty : ticket.FechaCambio.ToString()),
                                    new SqlParameter("@Prioridad", ticket.Prioridad),
                                    new SqlParameter("@Usuario", "WsIntermediario")
                };

                id = ctx.Database.ExecuteSqlCommand("EXEC [dbo].[SP_UPDATE_PRIORIDAD_WO] @TicketRemedy,@TicketInvgate,@FechaCambio,@Prioridad,@Usuario ", sqParam);
                Result = "Exito: Actualización de Orden de Trabajo registrado en bitácora.";
            }
            catch (Exception ex)
            {
                Result = "Error:" + ex.Message;
                log_.LogMsg("Error|Actualiza Priorizacion OC BD:" + ex.Message);
            }

            return id;
        }

        public int AgregaNota(AgregaNota ticket, int idInvgate, out string Result)
        {
            int id = 0;
            try
            {
                List<SqlParameter> sqParam = new List<SqlParameter> {
                                    new SqlParameter("@TicketRemedy", ticket.TicketIMSS),
                                    new SqlParameter("@TicketInvgate", idInvgate),
                                    new SqlParameter("@Nota", ticket.Notas == null ? string.Empty : ticket.Notas),
                                    new SqlParameter("@Usuario", "WsIntermediario"),
                                    new SqlParameter("@ArchivoName01", ticket.AdjuntoName01 == null ? string.Empty : ticket.AdjuntoName01),
                                    new SqlParameter("@ArchivoSize01", ticket.AdjuntoSize01 == null ? 0 : ticket.AdjuntoSize01),
                                    new SqlParameter("@ArchivoName02", ticket.AdjuntoName02 == null ? string.Empty : ticket.AdjuntoName02),
                                    new SqlParameter("@ArchivoSize02", ticket.AdjuntoSize02 == null ? 0 : ticket.AdjuntoSize02),
                                    new SqlParameter("@ArchivoName03", ticket.AdjuntoName03 == null ? string.Empty : ticket.AdjuntoName03),
                                    new SqlParameter("@ArchivoSize03", ticket.AdjuntoSize03 == null ? 0 : ticket.AdjuntoSize03)
                };

                if (ticket.Adjunto01 == null)
                    sqParam.Add(new SqlParameter("@Archivo01", DBNull.Value));
                else
                    sqParam.Add(new SqlParameter("@Archivo01", ticket.Adjunto01));
                if (ticket.Adjunto02 == null)
                    sqParam.Add(new SqlParameter("@Archivo02", DBNull.Value));
                else
                    sqParam.Add(new SqlParameter("@Archivo02", ticket.Adjunto02));
                if (ticket.Adjunto03 == null)
                    sqParam.Add(new SqlParameter("@Archivo03", DBNull.Value));
                else
                    sqParam.Add(new SqlParameter("@Archivo03", ticket.Adjunto03));

                id = ctx.Database.ExecuteSqlCommand("EXEC [dbo].[SP_INSERT_NOTA_WO] @TicketRemedy,@TicketInvgate,@Nota,@Usuario,@Archivo01,@ArchivoName01,@ArchivoSize01,@Archivo02,@ArchivoName02,@ArchivoSize02,@Archivo03,@ArchivoName03,@ArchivoSize03 ", sqParam.ToArray());
                Result = "Exito: Nota registrada en bitácora.";
            }
            catch (Exception ex)
            {
                Result = "Error:" + ex.Message;
                log_.LogMsg("Error|Agrega Nota OC BD:" + ex.Message);
            }

            return id;
        }
    }
}
