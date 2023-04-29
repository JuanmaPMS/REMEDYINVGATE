using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Entities.Intermedio;

namespace Email
{
    public class Notificacion
    {
        public bool Accesos(EmailBody body)
        {
            bool status = false;
            try
            {
                string mensaje = string.Empty;
                string _asunto = body.TicketIMSS + " - Ticket asignado";
                string[] Destinatarios = body.Destinatarios.ToArray();
                string[] DestinatariosCC = body.DestinatariosCC.ToArray();
                string[] adjuntos = new string[0];


                Email oEmail = new Email(_asunto, Destinatarios, DestinatariosCC, _adjuntos: adjuntos);
                oEmail.EnviarEmail(GetViewAcceso(body), out status, out mensaje);
            }
            catch
            {
                status = false;
            }
            return status;
        }

        private AlternateView GetViewAcceso(EmailBody model)
        {
            AlternateView vistaEmail;
            string plantilla = string.Empty;
            plantilla = "/Plantillas/Asignacion.html";

            //Body
            string mailBody = string.Empty;
            StreamReader format = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + plantilla);
            mailBody = format.ReadToEnd();

            if (!string.IsNullOrEmpty(model.Prioridad))
                mailBody = mailBody.Replace("@@Prioridad@@", model.Prioridad);
            if (!string.IsNullOrEmpty(model.Titulo))
                mailBody = mailBody.Replace("@@Titulo@@", model.Titulo);
            if (!string.IsNullOrEmpty(model.Descripcion))
                mailBody = mailBody.Replace("@@Descripcion@@", model.Descripcion);
            if (!string.IsNullOrEmpty(model.TicketIMSS))
                mailBody = mailBody.Replace("@@TicketIMSS@@", model.TicketIMSS);
            if (!string.IsNullOrEmpty(model.Requerimiento))
                mailBody = mailBody.Replace("@@Requerimiento@@", model.Requerimiento);
            if (!string.IsNullOrEmpty(model.Aplicativo))
                mailBody = mailBody.Replace("@@Aplicativo@@", model.Aplicativo);
            mailBody = mailBody.Replace("@@Hora@@", DateTime.Now.ToShortTimeString());

            format.Close();
            //Fin Body

            //LinkedResource imageLogo = new LinkedResource(AppDomain.CurrentDomain.BaseDirectory + "/Plantillas/img/logoGR.PNG", "image/jpeg");
            //imageLogo.ContentId = "logo";

            vistaEmail = AlternateView.CreateAlternateViewFromString(mailBody, Encoding.UTF8, "text/html");
            //vistaEmail.LinkedResources.Add(imageLogo);

            return vistaEmail;
        }
    }
}
