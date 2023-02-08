using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Intermedio;
using System.Data.SqlClient;

namespace ServiceBitacora
{
    public class CatalogosData
    {
        private InterEntities ctx = null;

        public CatalogosData()
        {
            this.ctx = new InterEntities();
        }

        public int GetUrgencia(int idRemedy)
        {
            int id = 0;
            try
            {
                id = ctx.Database.SqlQuery<int>("EXEC [dbo].[SP_OBTIENE_URGENCIA_INVGATE] @UrgenciaId", new SqlParameter("@UrgenciaId", idRemedy)).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return id;
        }

        public int GetPrioridad(int idRemedy)
        {
            int id = 0;
            try
            {
                id = ctx.Database.SqlQuery<int>("EXEC [dbo].[SP_OBTIENE_PRIORIDAD_INVGATE] @PrioridadId", new SqlParameter("@PrioridadId", idRemedy)).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return id;
        }
    }
}
