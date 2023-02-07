﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Intermedio;
using System.Data.SqlClient;

namespace ServiceBitacora
{
    public class IncidenteData
    {
        private InterEntities ctx = null;

        public IncidenteData()
        {
            this.ctx = new InterEntities();
        }

        public int Get(string ticketImss)
        {
            int id = 0;
            try
            {
                id = ctx.Database.SqlQuery<int>("EXEC [dbo].[SP_OBTIENE_TICKET_INVGATE_INCIDENTE] @TicketIMSS", new SqlParameter("@TicketIMSS", ticketImss)).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return id;
        }

        public int Crear(CreaTicket ticket, int idInvgate, out string Result)
        {
            int id = 0;
            try
            {
                SqlParameter[] sqParam = new SqlParameter[] {
                                    new SqlParameter("@TicketRemedy", ticket.TicketIMSS),
                                    new SqlParameter("@TicketInvgate", idInvgate),
                                    new SqlParameter("@Descripcion", ticket.Descripcion),
                                    new SqlParameter("@Resumen", ticket.Resumen),
                                    new SqlParameter("@Impacto", ticket.Impacto),
                                    new SqlParameter("@Urgencia", ticket.Urgencia),
                                    new SqlParameter("@TipoIncidencia", ticket.TipoIncidencia),
                                    new SqlParameter("@FuenteReportada", ticket.FuenteReportada),
                                    new SqlParameter("@NombreProducto", ticket.NombreProducto),
                                    new SqlParameter("@GrupoSoporte", ticket.GrupoSoporte),
                                    new SqlParameter("@CategoriaOpe01", ticket.CategoriaOpe01),
                                    new SqlParameter("@CategoriaOpe02", ticket.CategoriaOpe02),
                                    new SqlParameter("@CategoriaOpe03", ticket.CategoriaOpe03),
                                    new SqlParameter("@CategoriaPro01", ticket.CategoriaPro01),
                                    new SqlParameter("@CategoriaPro02", ticket.CategoriaPro02),
                                    new SqlParameter("@CategoriaPro03", ticket.CategoriaPro03),
                                    new SqlParameter("@Estado", ticket.EstadoNuevo),
                                    new SqlParameter("@FechaCreacion", ticket.FechaCreacion),
                                    new SqlParameter("@Cliente", ticket.Cliente),
                                    new SqlParameter("@VIP", ticket.VIP.ToUpper() == "SI" ? 1 : 0),
                                    new SqlParameter("@Sensibilidad", ticket.Sensibilidad),
                                    new SqlParameter("@Usuario", ""),
                                    new SqlParameter("@Nota", ticket.Notas)
                };

                id = ctx.Database.ExecuteSqlCommand("EXEC [dbo].[SP_INSERT_INCIDENTE] @TicketRemedy,@TicketInvgate,@Descripcion,@Resumen,@Impacto,@Urgencia,@TipoIncidencia,@FuenteReportada,@NombreProducto,@GrupoSoporte,@CategoriaOpe01,@CategoriaOpe02,@CategoriaOpe03,@CategoriaPro01,@CategoriaPro02,@CategoriaPro03,@Estado,@FechaCreacion,@Cliente,@VIP,@Sensibilidad,@Usuario,@Nota ", sqParam);
                Result = "Exito: Incidente registrado en bitácora.";
            }
            catch (Exception ex)
            {
                Result = "Error:" + ex.Message;
            }

            return id;
        }

        public int ActualizaIncidente(ActualizaTicket ticket, int idInvgate, out string Result)
        {
            int id = 0;
            try
            {
                SqlParameter[] sqParam = new SqlParameter[] {
                                    new SqlParameter("@TicketRemedy", ticket.TicketIMSS),
                                    new SqlParameter("@TicketInvgate", idInvgate),
                                    new SqlParameter("@FechaCambio", ticket.FechaCambio),
                                    new SqlParameter("@Impacto", ticket.Impacto),
                                    new SqlParameter("@Urgencia", ticket.Urgencia),
                                    new SqlParameter("@Estado", ticket.EstadoNuevo),
                                    new SqlParameter("@Motivo", ticket.Motivo),
                                    new SqlParameter("@Nota", ticket.Notas),
                                    new SqlParameter("@Usuario", "")
                                    
                };

                id = ctx.Database.ExecuteSqlCommand("EXEC [dbo].[SP_UPDATE_INCIDENTE] @TicketRemedy,@TicketInvgate,@FechaCambio@Impacto,@Urgencia,@Estado,@Motivo,@Nota,@Usuario ", sqParam);
                Result = "Exito: Actualización de incidente registrado en bitácora.";
            }
            catch (Exception ex)
            {
                Result = "Error:" + ex.Message;
            }

            return id;
        }

        public int ActualizaCategoria(ActualizaCategorizacion ticket, int idInvgate, out string Result)
        {
            int id = 0;
            try
            {
                SqlParameter[] sqParam = new SqlParameter[] {
                                    new SqlParameter("@TicketRemedy", ticket.TicketIMSS),
                                    new SqlParameter("@TicketInvgate", idInvgate),
                                    new SqlParameter("@FechaCambio", ticket.FechaCambio),
                                    new SqlParameter("@GrupoSoporte", ticket.GrupoSoporte),
                                    new SqlParameter("@CategoriaOpe01", ticket.CategoriaOpe01),
                                    new SqlParameter("@CategoriaOpe02", ticket.CategoriaOpe02),
                                    new SqlParameter("@CategoriaOpe03", ticket.CategoriaOpe03),
                                    new SqlParameter("@CategoriaPro01", ticket.CategoriaPro01),
                                    new SqlParameter("@CategoriaPro02", ticket.CategoriaPro02),
                                    new SqlParameter("@CategoriaPro03", ticket.CategoriaPro03),
                                    new SqlParameter("@Usuario", "")
                };

                id = ctx.Database.ExecuteSqlCommand("EXEC [dbo].[SP_UPDATE_CATEGORIA_INCIDENTE] @TicketRemedy,@TicketInvgate,@FechaCambio,@GrupoSoporte,@CategoriaOpe01,@CategoriaOpe02,@CategoriaOpe03,@CategoriaPro01,@CategoriaPro02,@CategoriaPro03,@Usuario ", sqParam);
                Result = "Exito: Actualización de incidente registrado en bitácora.";
            }
            catch (Exception ex)
            {
                Result = "Error:" + ex.Message;
            }

            return id;
        }

        public int ActualizaPriorizacion(ActualizaPriorizacion ticket, int idInvgate, out string Result)
        {
            int id = 0;
            try
            {
                SqlParameter[] sqParam = new SqlParameter[] {
                                    new SqlParameter("@TicketRemedy", ticket.TicketIMSS),
                                    new SqlParameter("@TicketInvgate", idInvgate),
                                    new SqlParameter("@FechaCambio", ticket.FechaCambio),
                                    new SqlParameter("@Impacto", ticket.Impacto),
                                    new SqlParameter("@Urgencia", ticket.Urgencia),
                                    new SqlParameter("@Usuario", "")
                };

                id = ctx.Database.ExecuteSqlCommand("EXEC [dbo].[SP_UPDATE_PRIORIDAD_INCIDENTE] @TicketRemedy,@TicketInvgate,@FechaCambio,@Impacto,@Urgencia,@Usuario ", sqParam);
                Result = "Exito: Actualización de incidente registrado en bitácora.";
            }
            catch (Exception ex)
            {
                Result = "Error:" + ex.Message;
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
                                    new SqlParameter("@Nota", ticket.Notas),
                                    new SqlParameter("@Usuario", "")
                };

                id = ctx.Database.ExecuteSqlCommand("EXEC [dbo].[SP_INSERT_NOTA_INCIDENTE] @TicketRemedy,@TicketInvgate,@Nota,@Usuario ", sqParam);
                Result = "Exito: Nota registrada en bitácora.";
            }
            catch (Exception ex)
            {
                Result = "Error:" + ex.Message;
            }

            return id;
        }


    }
}