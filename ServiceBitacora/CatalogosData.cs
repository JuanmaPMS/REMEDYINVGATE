using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Intermedio;
using System.Data.SqlClient;
using System.Net.NetworkInformation;

namespace ServiceBitacora
{
    public class CatalogosData
    {
        private InterEntities ctx = null;

        public CatalogosData()
        {
            this.ctx = new InterEntities();
        }

        public int GetUrgenciaInvgate(int idUrgencia)
        {
            int id = 0;
            try
            {
                id = ctx.Database.SqlQuery<int>("EXEC [dbo].[SP_OBTIENE_URGENCIA_INVGATE] @UrgenciaId", new SqlParameter("@UrgenciaId", idUrgencia)).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return id;
        }

        public int GetPrioridadInvgate(int idPrioridad)
        {
            int id = 0;
            try
            {
                id = ctx.Database.SqlQuery<int>("EXEC [dbo].[SP_OBTIENE_PRIORIDAD_INVGATE] @PrioridadId", new SqlParameter("@PrioridadId", idPrioridad)).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return id;
        }

        public int GetEstatusIncidenteInvgate(int estatus)
        {
            int id = 0;

            try
            {
                CatEstadoIncidente catEstado = ctx.CatEstadoIncidente.Where(x => x.ClaveRemedy == estatus).FirstOrDefault();
                if (catEstado != null)
                {
                    id = catEstado.ClaveInvgate.Value;
                }
            }
            catch (Exception ex)
            {

            }
            return id;
        }

        public int GetEstatusWOInvgate(int idEstatus)
        {
            int id = 0;

            try
            {
                CatEstadoWO catEstado = ctx.CatEstadoWO.Where(x => x.ClaveRemedy == idEstatus).FirstOrDefault();
                if (catEstado != null)
                {
                    id = catEstado.ClaveInvgate.Value;
                }
            }
            catch (Exception ex)
            {

            }
            return id;
        }

        public int GetUrgenciaIncidenteIMSS(int idPrioridad)
        {
            int id = 0;
            try
            {
                CatUrgenciaIncidente catUrgencia = ctx.CatUrgenciaIncidente.Where(x => x.ClaveInvgate == idPrioridad).FirstOrDefault();
                if (catUrgencia != null)
                {
                    id = catUrgencia.ClaveRemedy.Value;
                }
            }
            catch (Exception ex)
            {

            }
            return id;
        }

        public int GetPrioridadWOIMSS(int idPrioridad)
        {
            int id = 0;
            try
            {
                CatPrioridadWO catPrioridad = ctx.CatPrioridadWO.Where(x => x.ClaveInvgate == idPrioridad).FirstOrDefault();
                if (catPrioridad != null)
                {
                    id = catPrioridad.ClaveRemedy.Value;
                }
            }
            catch (Exception ex)
            {

            }
            return id;
        }

        public int GetEstatusIncidenteIMSS(int idEstatus)
        {
            int id = 0;

            try
            {
                CatEstadoIncidente catEstado = ctx.CatEstadoIncidente.Where(x => x.ClaveInvgate == idEstatus).FirstOrDefault();
                if (catEstado != null)
                {
                    id = catEstado.ClaveRemedy.Value;
                }
            }
            catch (Exception ex)
            {

            }
            return id;
        }

        public int GetEstatusWOIMSS(int idEstatus)
        {
            int id = 0;

            try
            {
                CatEstadoWO catEstado = ctx.CatEstadoWO.Where(x => x.ClaveInvgate == idEstatus).FirstOrDefault();
                if (catEstado != null)
                {
                    id = catEstado.ClaveRemedy.Value;
                }
            }
            catch (Exception ex)
            {

            }
            return id;
        }
    }
}
