using System;
using System.Configuration;
using System.Net;
using System.Security;
using System.Xml;
using Entities;
using Entities.Helix;
using RestSharp;

namespace ServiceIMSS.Helix
{
    public class OrdenesTrabajoHelix
    {
        private const string EspacioDeNombres = "urn:IMSS_Integraciones_Bandeja_De_Entrada_Ordenes_de_Trabajo";
        private const string NombreServicio = "IMSS_Integraciones_Bandeja_De_Entrada_Ordenes_de_Trabajo";
        private readonly string user = ConfigurationManager.AppSettings["user"];
        private readonly string password = ConfigurationManager.AppSettings["password"];
        private readonly string nombreProveedor = ConfigurationManager.AppSettings["Proveedor"];
        private readonly string urlCliente = ConfigurationManager.AppSettings["UrlCliente"];

        public Result ActualizaOrdenTrabajo(ActualizacionOrdenTrabajo data)
        {
            if (data == null)
            {
                return ResultadoError("Los datos de actualización de la orden de trabajo son requeridos.");
            }

            string body = CrearSobreSoap(
                "Actualiza_Ordenes_de_Trabajo",
                Elemento("Tipo_de_Operacion", data.TipoOperacion) +
                Elemento("Tipo_de_Ticket", data.TipoTicket) +
                Elemento("Nombre_del_Proveedor", nombreProveedor) +
                Elemento("ID_de_la_Orden_de_Trabajo", data.IdOrdenTrabajo) +
                Elemento("Ticket_de_proveedor", data.TicketProveedor) +
                Elemento("Estado_nuevo_de_la_Orden_de_Trabajo", data.EstadoNuevo) +
                Elemento("Motivo_de_estado_nuevo_de_la_Orden_de_Trabajo", data.MotivoEstadoNuevo) +
                Elemento("Nota_de_Resolucion", data.NotaResolucion));

            return EjecutarSolicitud(
                "urn:IMSS_Integraciones_Bandeja_De_Entrada_Ordenes_de_Trabajo/Actualiza_Ordenes_de_Trabajo",
                "Actualiza_Ordenes_de_TrabajoResponse",
                body);
        }

        public Result AdicionaBitacora(BitacoraTrabajoOrdenTrabajo data)
        {
            if (data == null)
            {
                return ResultadoError("Los datos de bitácora de la orden de trabajo son requeridos.");
            }

            string body = CrearSobreSoap(
                "Bitacora_de_Trabajo_Orden_de_Trabajo",
                Elemento("Tipo_de_Operacion", data.TipoOperacion) +
                Elemento("Tipo_de_Ticket", data.TipoTicket) +
                Elemento("Nombre_del_Proveedor", nombreProveedor) +
                Elemento("ID_de_la_Orden_de_Trabajo", data.IdOrdenTrabajo) +
                Elemento("Ticket_de_proveedor", data.TicketProveedor) +
                Elemento("Notas", data.Notas) +
                Elemento("Adjunto_1_Name", data.Adjunto1Nombre) +
                Elemento("Adjunto_1_Data", data.Adjunto1Datos) +
                Elemento("Adjunto_1_Size", TamanoAdjunto(data.Adjunto1Tamano)) +
                Elemento("Adjunto_2_Name", data.Adjunto2Nombre) +
                Elemento("Adjunto_2_Data", data.Adjunto2Datos) +
                Elemento("Adjunto_2_Size", TamanoAdjunto(data.Adjunto2Tamano)) +
                Elemento("Adjunto_3_Name", data.Adjunto3Nombre) +
                Elemento("Adjunto_3_Data", data.Adjunto3Datos) +
                Elemento("Adjunto_3_Size", TamanoAdjunto(data.Adjunto3Tamano)));

            return EjecutarSolicitud(
                "urn:IMSS_Integraciones_Bandeja_De_Entrada_Ordenes_de_Trabajo/Bitacora_de_Trabajo_Orden_de_Trabajo",
                "Bitacora_de_Trabajo_Orden_de_TrabajoResponse",
                body);
        }

        private Result EjecutarSolicitud(string soapAction, string elementoRespuesta, string body)
        {
            if (String.IsNullOrWhiteSpace(urlCliente))
            {
                return ResultadoError("La clave de configuración UrlClienteHelix no tiene un valor configurado.");
            }

            Result result = new Result();

            try
            {
                var client = new RestClient(urlCliente + "&webService=" + NombreServicio);
                var request = new RestRequest("", Method.Post);
                request.AddHeader("Content-Type", "text/xml; charset=utf-8");
                request.AddHeader("SOAPAction", soapAction);
                request.AddParameter("", body, ParameterType.RequestBody);

                var response = client.Execute(request);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return ResultadoError("No se obtuvo respuesta del servidor.");
                }

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(response.Content);

                XmlNode respuesta = xmlDocument.SelectSingleNode("//*[local-name()='" + elementoRespuesta + "']");
                if (respuesta == null)
                {
                    return ResultadoError("La respuesta del servidor no contiene el elemento esperado " + elementoRespuesta + ".");
                }

                XmlNode estado = respuesta.SelectSingleNode("./*[local-name()='Estado_de_la_Transaccion']");
                XmlNode resultado = respuesta.SelectSingleNode("./*[local-name()='Resultado_de_la_Transaccion']");
                result.Estatus = estado != null && !String.Equals(estado.InnerText, "Error", StringComparison.OrdinalIgnoreCase);
                result.Resultado = resultado == null ? (estado == null ? String.Empty : estado.InnerText) : resultado.InnerText;
            }
            catch (Exception ex)
            {
                result.Estatus = false;
                result.Resultado = ex.ToString();
            }

            return result;
        }

        private string CrearSobreSoap(string operacion, string datosOperacion)
        {
            return "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:urn=\"" + EspacioDeNombres + "\">" +
                "<soap:Header>" +
                "<urn:AuthenticationInfo>" +
                Elemento("userName", user) +
                Elemento("password", password) +
                "</urn:AuthenticationInfo>" +
                "</soap:Header>" +
                "<soap:Body>" +
                "<urn:" + operacion + ">" + datosOperacion + "</urn:" + operacion + ">" +
                "</soap:Body>" +
                "</soap:Envelope>";
        }

        private static string Elemento(string nombre, string valor)
        {
            return "<urn:" + nombre + ">" + EscapeXml(valor) + "</urn:" + nombre + ">";
        }

        private static string EscapeXml(string valor)
        {
            return SecurityElement.Escape(valor ?? String.Empty);
        }

        private static string TamanoAdjunto(int? tamano)
        {
            return tamano.HasValue ? tamano.Value.ToString() : String.Empty;
        }

        private static Result ResultadoError(string mensaje)
        {
            return new Result
            {
                Estatus = false,
                Resultado = mensaje
            };
        }
    }
}
