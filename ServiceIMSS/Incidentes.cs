using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Xml;
using Entities;
using RestSharp;

namespace ServiceIMSS
{
    public class Incidentes
    {
        string user = ConfigurationManager.AppSettings["user"];
        string password = ConfigurationManager.AppSettings["password"];
        string NombreProveedor = ConfigurationManager.AppSettings["Proveedor"];
        string UrCliente = ConfigurationManager.AppSettings["UrlCliente"];
        public Result ActualizaCategorizacion(Categorizacion data)
        {
            Result result = new Result();

            try 
            {
                var client = new RestClient(UrCliente + "?server=remedy&webService=NXR_WS_GestionDeIncidentesIMSS_PM");
                var request = new RestRequest("", Method.Post);
                request.AddHeader("Content-Type", "text/xml charset=utf-8");
                request.AddHeader("SOAPAction", "urn:NXR_WS_GestionDeIncidentesIMSS_PM/Actualiza_Categorizacion_IMSS");
                var body = @"<?xml version=""1.0"" encoding=""utf-8""?>" +
                @"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:NXR_WS_GestionDeIncidentesIMSS_PM"">" +
                @"  <soap:Header>" +
                @"      <urn:AuthenticationInfo>" +
                @"         <urn:userName>" + user + "</urn:userName>" +
                @"         <urn:password>" + password + "</urn:password>" +
                @"         <urn:authentication></urn:authentication>" +
                @"         <urn:locale></urn:locale>" +
                @"         <urn:timeZone></urn:timeZone>" +
                @"      </urn:AuthenticationInfo>" +
                @"   </soap:Header>" +
                @"   <soap:Body>" +
                @"      <urn:Actualiza_Categorizacion_IMSS>" +
                @"         <urn:TipoDeOperacion>A_CAT_INC</urn:TipoDeOperacion>" +
                @"         <urn:NombreProveedor>" + NombreProveedor + "</urn:NombreProveedor>" +
                @"         <urn:IDTicketIMSS>" + data.IDTicketRemedy + "</urn:IDTicketIMSS>" +
                @"         <urn:IDTicketProveedor>" + data.IDTicketInvgate + "</urn:IDTicketProveedor>" +
                @"         <urn:CatOper01>" + data.CatOperacion01 + "</urn:CatOper01>" +
                @"         <urn:CatOper02>" + data.CatOperacion02 + "</urn:CatOper02>" +
                @"         <urn:CatOper03>" + data.CatOperacion03 + "</urn:CatOper03>" +
                @"         <urn:CatProd01>" + data.CatProducto01 + "</urn:CatProd01>" +
                @"         <urn:CatProd02>" + data.CatProducto02 + "</urn:CatProd02>" +
                @"         <urn:CatProd03>" + data.CatProducto03 + "</urn:CatProd03>" +
                @"         <urn:FechaActualizacion>"+ DateTime.Now.ToString()+"</urn:FechaActualizacion>" +
                @"         <urn:Aplicativo></urn:Aplicativo>" +
                @"         <urn:Coordinación></urn:Coordinación>" +
                @"         <urn:CoordinaciónTécnica></urn:CoordinaciónTécnica>" +
                @"         <urn:División></urn:División>" +
                @"         <urn:PK></urn:PK>" +
                @"      </urn:Actualiza_Categorizacion_IMSS>" +
                @"   </soap:Body>" +
                @"</soap:Envelope>" +
                @"";
                request.AddParameter("", body, ParameterType.RequestBody);
                var response = client.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string transaccion = string.Empty;
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(response.Content);

                    XmlNodeList xmlNode = xmlDocument.GetElementsByTagName("ns0:Actualiza_Categorizacion_IMSSResponse");

                    foreach (XmlElement node in xmlNode)
                    {
                        transaccion = node.LastChild.InnerText;
                        result.Resultado = node.FirstChild.InnerText;
                    }
                    if (transaccion == "Error")
                        result.Estatus = false;
                    else
                        result.Estatus = true;
                }
                else
                {
                    result.Estatus = false;
                    result.Resultado = "No se obtuvo respuesta del servidor.";
                }
            }
            catch (Exception ex) 
            {
                result.Estatus = false;
                result.Resultado = ex.ToString();
            }
            return result;
        }

        public Result ActualizaIncidente(Incidente data)
        {
            Result result = new Result();

            try
            {
                var client = new RestClient(UrCliente + "?server=remedy&webService=NXR_WS_GestionDeIncidentesIMSS_PM");
                var request = new RestRequest("", Method.Post);
                request.AddHeader("Content-Type", "text/xml charset=utf-8");
                request.AddHeader("SOAPAction", "urn:NXR_WS_GestionDeIncidentesIMSS_PM/Actualiza_Incidentes_IMSS");
                var body = @"<?xml version=""1.0"" encoding=""utf-8""?>" +
                @"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:NXR_WS_GestionDeIncidentesIMSS_PM"">" +
                @"  <soap:Header>" +
                @"      <urn:AuthenticationInfo>" +
                @"         <urn:userName>" + user + "</urn:userName>" +
                @"         <urn:password>" + password + "</urn:password>" +
                @"         <urn:authentication></urn:authentication>" +
                @"         <urn:locale></urn:locale>" +
                @"         <urn:timeZone></urn:timeZone>" +
                @"      </urn:AuthenticationInfo>" +
                @"   </soap:Header>" +
                @"   <soap:Body>" +
                @"      <urn:Actualiza_Incidentes_IMSS>" +
                @"         <urn:TipoDeOperacion>A_INC</urn:TipoDeOperacion>" +
                @"         <urn:NombreProveedor>" + NombreProveedor + "</urn:NombreProveedor>" +
                @"         <urn:IDTicketIMSS>" + data.IDTicketRemedy + "</urn:IDTicketIMSS>" +
                @"         <urn:IDTicketProveedor>" + data.IDTicketInvgate + "</urn:IDTicketProveedor>" +
                @"         <urn:CloseCatOper01>" + data.CatCierreOperacion01 + "</urn:CloseCatOper01>" +
                @"         <urn:CloseCatOper02>" + data.CatCierreOperacion02 + "</urn:CloseCatOper02>" +
                @"         <urn:CloseCatOper03>" + data.CatCierreOperacion03 + "</urn:CloseCatOper03>" +
                @"         <urn:CloseCatProd01>" + data.CatCierreProducto01 + "</urn:CloseCatProd01>" +
                @"         <urn:CloseCatProd02>" + data.CatCierreProducto02 + "</urn:CloseCatProd02>" +
                @"         <urn:CloseCatProd03>" + data.CatCierreProducto03 + "</urn:CloseCatProd03>" +
                @"         <urn:EstadoNuevo>" + data.EstadoNuevo + "</urn:EstadoNuevo>" +
                @"         <urn:MotivoEstado>" + data.MotivoEstado + "</urn:MotivoEstado>" +
                @"         <urn:Resolucion>" + data.Resolucion + "</urn:Resolucion>" +
                @"         <urn:FechaActualizacion>" + DateTime.Now.ToString() + "</urn:FechaActualizacion>" +
                @"         <urn:TiempoAtencion></urn:TiempoAtencion>" +
                @"         <urn:TiempoResolucion></urn:TiempoResolucion>" +
                @"      </urn:Actualiza_Incidentes_IMSS>" +
                @"   </soap:Body>" +
                @"</soap:Envelope>" +
                @"";
                request.AddParameter("", body, ParameterType.RequestBody);
                var response = client.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string transaccion = string.Empty;
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(response.Content);

                    XmlNodeList xmlNode = xmlDocument.GetElementsByTagName("ns0:Actualiza_Incidentes_IMSSResponse");

                    foreach (XmlElement node in xmlNode)
                    {
                        transaccion = node.LastChild.InnerText;
                        result.Resultado = node.FirstChild.InnerText;
                    }
                    if (transaccion == "Error")
                        result.Estatus = false;
                    else
                        result.Estatus = true;
                }
                else
                {
                    result.Estatus = false;
                    result.Resultado = "No se obtuvo respuesta del servidor.";
                }
            }
            catch (Exception ex)
            {
                result.Estatus = false;
                result.Resultado = ex.ToString();
            }
            return result;
        }

        public Result ActualizaPrioridad(Prioridad data)
        {
            Result result = new Result();

            try
            {
                var client = new RestClient(UrCliente + "?server=remedy&webService=NXR_WS_GestionDeIncidentesIMSS_PM");
                var request = new RestRequest("", Method.Post);
                request.AddHeader("Content-Type", "text/xml charset=utf-8");
                request.AddHeader("SOAPAction", "urn:NXR_WS_GestionDeIncidentesIMSS_PM/Actualiza_Prioridad_IMSS");
                var body = @"<?xml version=""1.0"" encoding=""utf-8""?>" +
                @"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:NXR_WS_GestionDeIncidentesIMSS_PM"">" +
                @"  <soap:Header>" +
                @"      <urn:AuthenticationInfo>" +
                @"         <urn:userName>" + user + "</urn:userName>" +
                @"         <urn:password>" + password + "</urn:password>" +
                @"         <urn:authentication></urn:authentication>" +
                @"         <urn:locale></urn:locale>" +
                @"         <urn:timeZone></urn:timeZone>" +
                @"      </urn:AuthenticationInfo>" +
                @"   </soap:Header>" +
                @"   <soap:Body>" +
                @"      <urn:Actualiza_Prioridad_IMSS>" +
                @"         <urn:TipoDeOperacion>A_PRY_INC</urn:TipoDeOperacion>" +
                @"         <urn:NombreProveedor>" + NombreProveedor + "</urn:NombreProveedor>" +
                @"         <urn:IDTicketIMSS>" + data.IDTicketRemedy + "</urn:IDTicketIMSS>" +
                @"         <urn:IDTicketProveedor>" + data.IDTicketInvgate + "</urn:IDTicketProveedor>" +
                @"         <urn:Impacto>" + data.Impacto.ToString() + "</urn:Impacto>" +
                @"         <urn:Urgencia>" + data.Urgencia.ToString() + "</urn:Urgencia>" +
                @"         <urn:FechaActualizacion>" + DateTime.Now.ToString() + "</urn:FechaActualizacion>" +
                @"      </urn:Actualiza_Prioridad_IMSS>" +
                @"   </soap:Body>" +
                @"</soap:Envelope>" +
                @"";
                request.AddParameter("", body, ParameterType.RequestBody);
                var response = client.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string transaccion = string.Empty;
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(response.Content);

                    XmlNodeList xmlNode = xmlDocument.GetElementsByTagName("ns0:Actualiza_Prioridad_IMSSResponse");

                    foreach (XmlElement node in xmlNode)
                    {
                        transaccion = node.LastChild.InnerText;
                        result.Resultado = node.FirstChild.InnerText;
                    }
                    if (transaccion == "Error")
                        result.Estatus = false;
                    else
                        result.Estatus = true;
                }
                else
                {
                    result.Estatus = false;
                    result.Resultado = "No se obtuvo respuesta del servidor.";
                }
            }
            catch (Exception ex)
            {
                result.Estatus = false;
                result.Resultado = ex.ToString();
            }
            return result;
        }

        public Result AdicionaNotas(Comentario data)
        {
            Result result = new Result();

            try
            {
                var client = new RestClient(UrCliente + "?server=remedy&webService=NXR_WS_GestionDeIncidentesIMSS_PM");
                var request = new RestRequest("", Method.Post);
                request.AddHeader("Content-Type", "text/xml charset=utf-8");
                request.AddHeader("SOAPAction", "urn:NXR_WS_GestionDeIncidentesIMSS_PM/Adiciona_Notas_Incidentes_IMSS");
                var body = @"<?xml version=""1.0"" encoding=""utf-8""?>" +
                @"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:NXR_WS_GestionDeIncidentesIMSS_PM"">" +
                @"  <soap:Header>" +
                @"      <urn:AuthenticationInfo>" +
                @"         <urn:userName>" + user + "</urn:userName>" +
                @"         <urn:password>" + password + "</urn:password>" +
                @"         <urn:authentication></urn:authentication>" +
                @"         <urn:locale></urn:locale>" +
                @"         <urn:timeZone></urn:timeZone>" +
                @"      </urn:AuthenticationInfo>" +
                @"   </soap:Header>" +
                @"   <soap:Body>" +
                @"      <urn:Adiciona_Notas_Incidentes_IMSS>" +
                @"         <urn:TipoDeOperacion>C_IT_INC</urn:TipoDeOperacion>" +
                @"         <urn:NombreProveedor>" + NombreProveedor + "</urn:NombreProveedor>" +
                @"         <urn:IDTicketIMSS>" + data.IDTicketRemedy + "</urn:IDTicketIMSS>" +
                @"         <urn:IDTicketProveedor>" + data.IDTicketInvgate + "</urn:IDTicketProveedor>" +
                @"         <urn:Notas>" + data.Notas + "</urn:Notas>" +
                @"         <urn:Adjunto01_attachmentName>" + data.AdjuntoName01 + "</urn:Adjunto01_attachmentName>" +
                @"         <urn:Adjunto01_attachmentOrigSize>" + data.AdjuntoSize01 + "</urn:Adjunto01_attachmentOrigSize>" +
                @"         <urn:Adjunto01_attachmentData>" + data.Adjunto01 + "</urn:Adjunto01_attachmentData>" +
                @"         <urn:Adjunto02_attachmentName>" + data.AdjuntoName02 + "</urn:Adjunto02_attachmentName>" +
                @"         <urn:Adjunto02_attachmentOrigSize>" + data.AdjuntoSize02 + "</urn:Adjunto02_attachmentOrigSize>" +
                @"         <urn:Adjunto02_attachmentData>" + data.Adjunto02 + "</urn:Adjunto02_attachmentData>" +
                @"         <urn:Adjunto03_attachmentName>" + data.AdjuntoName03 + "</urn:Adjunto03_attachmentName>" +
                @"         <urn:Adjunto03_attachmentOrigSize>" + data.AdjuntoSize03 + "</urn:Adjunto03_attachmentOrigSize>" +
                @"         <urn:Adjunto03_attachmentData>" + data.Adjunto03 + "</urn:Adjunto03_attachmentData>" +
                @"      </urn:Adiciona_Notas_Incidentes_IMSS>" +
                @"   </soap:Body>" +
                @"</soap:Envelope>" +
                @"";
                request.AddParameter("", body, ParameterType.RequestBody);
                var response = client.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string transaccion = string.Empty;
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(response.Content);

                    XmlNodeList xmlNode = xmlDocument.GetElementsByTagName("ns0:Adiciona_Notas_Incidentes_IMSSResponse");

                    foreach (XmlElement node in xmlNode)
                    {
                        transaccion = node.LastChild.InnerText;
                        result.Resultado = node.FirstChild.InnerText;
                    }
                    if (transaccion == "Error")
                        result.Estatus = false;
                    else
                        result.Estatus = true;
                }
                else
                {
                    result.Estatus = false;
                    result.Resultado = "No se obtuvo respuesta del servidor.";
                }
            }
            catch (Exception ex)
            {
                result.Estatus = false;
                result.Resultado = ex.ToString();
            }
            return result;
        }
    }
}
