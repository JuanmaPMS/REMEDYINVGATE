using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBitacora
{
    public class LogTask
    {
        private string m_exePath = ConfigurationManager.AppSettings["Ruta_Log"];

        public void LogMsg(string logMessage)
        {
            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + "logTask" + DateTime.Today.ToString("dd-MM-yyyy") + ".txt"))
                {
                    LogWrite(logMessage, w);
                }
            }
            catch// (Exception ex)
            {
            }
        }
        private void LogWrite(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry Task: ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                //txtWriter.WriteLine("  :");
                txtWriter.WriteLine("  {0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch// (Exception ex)
            {
            }
        }
    }
}
