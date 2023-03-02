using Entities.Intermedio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBitacora
{
    public class Log
    {
        private string m_exePath = ConfigurationManager.AppSettings["Ruta_Log"];

        public void LogMsg(string logMessage)
        {
            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + "log" + DateTime.Today.ToString("dd-MM-yyyy") + ".txt"))
                {
                    LogWrite(logMessage, w);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void LogCreaTicket(CreaTicket entity)
        {
            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + "log" + DateTime.Today.ToString("dd-MM-yyyy") + ".txt"))
                {
                    LogWriteEntity(entity, w);
                }
            }
            catch
            {
            }
        }

        private void LogWriteEntity(CreaTicket entity, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : Crea Ticket");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                txtWriter.WriteLine(" TipoOperacion:{0}", entity.TipoOperacion);
                txtWriter.WriteLine(" NombreProveedor:{0}", entity.NombreProveedor);
                txtWriter.WriteLine(" TicketIMSS:{0}", entity.TicketIMSS);
                txtWriter.WriteLine(" Descripcion:{0}", entity.Descripcion);
                txtWriter.WriteLine(" Resumen:{0}", entity.Resumen);
                txtWriter.WriteLine(" Impacto:{0}", entity.Impacto);
                txtWriter.WriteLine(" Urgencia:{0}", entity.Urgencia);
                txtWriter.WriteLine(" Prioridad:{0}", entity.Prioridad);
                txtWriter.WriteLine(" TipoIncidencia:{0}", entity.TipoIncidencia);
                txtWriter.WriteLine(" FuenteReportada:{0}", entity.FuenteReportada);
                txtWriter.WriteLine(" NombreProducto:{0}", entity.NombreProducto);
                txtWriter.WriteLine(" GrupoSoporte:{0}", entity.GrupoSoporte);
                txtWriter.WriteLine(" CategoriaOpe01:{0}", entity.CategoriaOpe01);
                txtWriter.WriteLine(" CategoriaOpe02:{0}", entity.CategoriaOpe02);
                txtWriter.WriteLine(" CategoriaOpe03:{0}", entity.CategoriaOpe03);
                txtWriter.WriteLine(" CategoriaPro01:{0}", entity.CategoriaPro01);
                txtWriter.WriteLine(" CategoriaPro02:{0}", entity.CategoriaPro02);
                txtWriter.WriteLine(" CategoriaPro03:{0}", entity.CategoriaPro03);
                txtWriter.WriteLine(" EstadoNuevo:{0}", entity.EstadoNuevo);
                txtWriter.WriteLine(" Notas:{0}", entity.Notas);
                txtWriter.WriteLine(" FechaCreacion:{0}", entity.FechaCreacion);
                txtWriter.WriteLine(" Adjunto01:{0}", entity.Adjunto01);
                txtWriter.WriteLine(" AdjuntoName01:{0}", entity.AdjuntoName01);
                txtWriter.WriteLine(" AdjuntoSize01:{0}", entity.AdjuntoSize01);
                txtWriter.WriteLine(" Adjunto02:{0}", entity.Adjunto02);
                txtWriter.WriteLine(" AdjuntoName02:{0}", entity.AdjuntoName02);
                txtWriter.WriteLine(" AdjuntoSize02:{0}", entity.AdjuntoSize02);
                txtWriter.WriteLine(" Adjunto03:{0}", entity.Adjunto03);
                txtWriter.WriteLine(" AdjuntoName03:{0}", entity.AdjuntoName03);
                txtWriter.WriteLine(" AdjuntoSize03:{0}", entity.AdjuntoSize03);
                txtWriter.WriteLine(" Cliente:{0}", entity.Cliente);
                txtWriter.WriteLine(" VIP:{0}", entity.VIP);
                txtWriter.WriteLine(" Sensibilidad:{0}", entity.Sensibilidad);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
            }
        }

        private void LogWrite(string logMessage, TextWriter txtWriter)
        {
            try
            {
                //txtWriter.Write("\r\nLog Entry : ");
                //txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                //    DateTime.Now.ToLongDateString());
                //txtWriter.WriteLine("  :");
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
            }
        }
    }
}
