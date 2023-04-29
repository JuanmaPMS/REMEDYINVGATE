using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ServiceBitacora
{
    public class NotificacionData
    {
        private InterEntities ctx = null;
        private LogTask log_ = new LogTask();
        public NotificacionData()
        {
            this.ctx = new InterEntities();
        }

        public List<Notificacion> Get()
        {
            List<Notificacion> notificaciones = new List<Notificacion>();
            try
            {
                notificaciones = ctx.Notificacion.Where(x => x.Enviado == false).ToList();
            }
            catch (Exception ex)
            {
                log_.LogMsg("Error|Get Notificaciones:" + ex.Message);
            }
            return notificaciones;
        }

        public void Procesado(int id)
        {
            try
            {
                Notificacion notificacion = ctx.Notificacion.Where(x => x.id == id).FirstOrDefault();
                if (notificacion != null)
                {
                    notificacion.Enviado = true;
                    ctx.Entry(notificacion).State = EntityState.Modified;
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log_.LogMsg("Error|Actualiza Procesado:" + ex.Message);
            }
        }

        public List<NotificacionAgente> GetAgentes(string grupo)
        {
            List<NotificacionAgente> agentes = new List<NotificacionAgente>();
            try
            {
                agentes = ctx.NotificacionAgente.Where(x => x.GrupoSoporte == grupo && x.Activo == true).ToList();
            }
            catch (Exception ex)
            {
                log_.LogMsg("Error|Get Agentes:" + ex.Message);
            }
            return agentes;
        }

        public List<NotificacionConsultor> GetConsultores(string grupo)
        {
            List<NotificacionConsultor> consultores = new List<NotificacionConsultor>();
            try
            {
                consultores = ctx.NotificacionConsultor.Where(x => x.GrupoSoporte == grupo && x.Activo == true).ToList();
            }
            catch (Exception ex)
            {
                log_.LogMsg("Error|Get Consultores:" + ex.Message);
            }
            return consultores;
        }

    }
}
