using Entities;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;
using Entities.Intermedio;
using System.Reflection;
using Entities.Invgate;

namespace ServiceInvgate
{
    public class IncidentesInvgate
    {

        string user = ConfigurationManager.AppSettings["usernameInvgate"];
        string password = ConfigurationManager.AppSettings["passwordInvgate"];
        string UrlServicios = ConfigurationManager.AppSettings["UrlInvgate"];
        string ApiAttachments = ConfigurationManager.AppSettings["RutaApiAttachments"];
        string UrlServiciosCtrl = ConfigurationManager.AppSettings["UrlServiceControl"];

        public object GetIncidente(IncidentesGetRequest incidente)
        {
            object data;
            try
            {

                var properties = from p in incidente.GetType().GetProperties()
                                 where p.GetValue(incidente, null) != null
                                 select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(incidente).ToString());

                string parametros = String.Join("&", properties.ToArray());

                var client = new RestClient(UrlServicios+ "/incident" + "?" + parametros);

                var request = new RestRequest("", Method.Get);
                client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(user, password);

                var response = client.Execute(request);

                Console.WriteLine(response.Content);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    data = JsonConvert.DeserializeObject(response.Content);
                }
                else
                {
                    data = new { Error = "Ocurrio un error: " + response.ErrorMessage };
                }
            }
            catch (Exception ex)
            {
                data = new { Error = "Ocurrio un error: " + ex.Message };
            }
            return data;
        }

        public Entities.Intermedio.Result PostIncidente(IncidentesPostRequest incidente)
        {
            IncidentesPostResponse postResponse = new IncidentesPostResponse();
            Entities.Intermedio.Result Resultado = new Entities.Intermedio.Result();


            object data;
            try
            {
                var client = new RestClient(UrlServicios + "/incident" );
                var request = new RestRequest("", Method.Post);
                client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(user, password);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", incidente, ParameterType.RequestBody);
                var response = client.Execute(request);

                Console.WriteLine(response.Content);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    postResponse = JsonConvert.DeserializeObject<IncidentesPostResponse>(response.Content);
                    Resultado.Ticket = postResponse.request_id.ToString();
                    Resultado.Resultado = "Transacción exitosa, registro agregado a Invgate People Media";
                    Resultado.Estado = "Exito";

                }
                else
                {

                    Resultado.Ticket = String.Empty;
                    Resultado.Resultado = response.ErrorMessage;
                    Resultado.Estado = "Error";
                }
            }
            catch (Exception ex)
            {
                Resultado.Ticket = String.Empty;
                Resultado.Resultado = "Ocurrio un error: " + ex.Message;
                Resultado.Estado = "Error";

            }
        
            return Resultado;
        }

        public String PostAttachments(HttpPostedFileBase[] Files, int incidenteID)
        {
            var client = new RestClient(ApiAttachments);
            var request = new RestRequest("",Method.Post);
            request.AddParameter("IncidentId", incidenteID.ToString());
            foreach (HttpPostedFileBase file_unit in Files)
            {
                MemoryStream target = new MemoryStream();
                file_unit.InputStream.CopyTo(target);
                byte[] data = target.ToArray();
                request.AddFile("files", data, file_unit.FileName);
            }
            var response = client.Execute(request);
            return response.StatusCode.ToString();
        }


        public Entities.Intermedio.Result PutIncidente(IncidentesPutRequest incidente)
        {
            IncidentesPutResponse postResponse = new IncidentesPutResponse();
            Entities.Intermedio.Result Resultado = new Entities.Intermedio.Result();
            try
            {
                var client = new RestClient(UrlServicios + "/incident");
                var request = new RestRequest("", Method.Put);
                client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(user, password);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", incidente, ParameterType.RequestBody);
                var response = client.Execute(request);

                Console.WriteLine(response.Content);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    postResponse = JsonConvert.DeserializeObject<IncidentesPutResponse>(response.Content);
                    Resultado.Ticket = postResponse.id.ToString();
                    Resultado.Resultado = "Transacción exitosa, registro agregado a Invgate People Media";
                    Resultado.Estado = "Exito";
                }
                else
                {
                    Resultado.Ticket = String.Empty;
                    Resultado.Resultado = response.ErrorException.Message;
                    Resultado.Estado = "Error";
                }
            }
            catch (Exception ex)
            {
                //data = new { Error = "Ocurrio un error: " + ex.Message };
                Resultado.Ticket = String.Empty;
                Resultado.Resultado = "Ocurrio un error: " + ex.Message;
                Resultado.Estado = "Error";
            }
            return Resultado;
        }

        public Entities.Intermedio.Result PutIncidenteStatus(IncidentPutRequest incidente)
        {
            Entities.Intermedio.Result Resultado = new Entities.Intermedio.Result();
            try
            {
                var client = new RestClient(UrlServiciosCtrl + "/ActualizarStatus");
                var request = new RestRequest("", Method.Post);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", incidente, ParameterType.RequestBody);
                var response = client.Execute(request);

                Console.WriteLine(response.Content);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Resultado.Ticket = incidente.id.ToString();
                    Resultado.Resultado = "Transacción exitosa, registro agregado a Invgate People Media";
                    Resultado.Estado = "Exito";
                }
                else
                {
                    Resultado.Ticket = String.Empty;
                    Resultado.Resultado = response.ErrorException.Message;
                    Resultado.Estado = "Error";
                }
            }
            catch (Exception ex)
            {
                Resultado.Ticket = String.Empty;
                Resultado.Resultado = "Ocurrio un error: " + ex.Message;
                Resultado.Estado = "Error";
            }
            return Resultado;
        }

        public Entities.Intermedio.Result PutIncidentePriority(IncidentPutRequest incidente)
        {
            Entities.Intermedio.Result Resultado = new Entities.Intermedio.Result();
            try
            {
                var client = new RestClient(UrlServiciosCtrl + "/ActualizarPrioridad");
                var request = new RestRequest("", Method.Post);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", incidente, ParameterType.RequestBody);
                var response = client.Execute(request);

                Console.WriteLine(response.Content);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Resultado.Ticket = incidente.id.ToString();
                    Resultado.Resultado = "Transacción exitosa, registro agregado a Invgate People Media";
                    Resultado.Estado = "Exito";
                }
                else
                {
                    Resultado.Ticket = String.Empty;
                    Resultado.Resultado = response.ErrorException.Message;
                    Resultado.Estado = "Error";
                }
            }
            catch (Exception ex)
            {
                Resultado.Ticket = String.Empty;
                Resultado.Resultado = "Ocurrio un error: " + ex.Message;
                Resultado.Estado = "Error";
            }
            return Resultado;
        }

        public Entities.Intermedio.Result PutIncidenteCategory(IncidentPutRequest incidente)
        {
            Entities.Intermedio.Result Resultado = new Entities.Intermedio.Result();
            try
            {
                var client = new RestClient(UrlServiciosCtrl + "/ActualizarCategoria");
                var request = new RestRequest("", Method.Post);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", incidente, ParameterType.RequestBody);
                var response = client.Execute(request);

                Console.WriteLine(response.Content);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Resultado.Ticket = incidente.id.ToString();
                    Resultado.Resultado = "Transacción exitosa, registro agregado a Invgate People Media";
                    Resultado.Estado = "Exito";
                }
                else
                {
                    Resultado.Ticket = String.Empty;
                    Resultado.Resultado = response.ErrorException.Message;
                    Resultado.Estado = "Error";
                }
            }
            catch (Exception ex)
            {
                Resultado.Ticket = String.Empty;
                Resultado.Resultado = "Ocurrio un error: " + ex.Message;
                Resultado.Estado = "Error";
            }
            return Resultado;
        }

        public Entities.Intermedio.Result PutStatusIncidente(int IncidenteId, int StatusId)
        {
            Entities.Intermedio.Result Resultado = new Entities.Intermedio.Result();
            try
            {
                var options = new RestClientOptions(ApiAttachments)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest(ApiAttachments + "?id=" + IncidenteId + "&statusId=" + StatusId, Method.Put);


                var response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Resultado.Ticket = IncidenteId.ToString();
                    Resultado.Resultado = "Transacción exitosa, registro actualizado en Invgate People Media";
                    Resultado.Estado = "Exito";
                }
                else
                {
                    Resultado.Ticket = String.Empty;
                    Resultado.Resultado = response.ErrorException.Message;
                    Resultado.Estado = "Error";
                }
            }
            catch (Exception ex)
            {
                Resultado.Ticket = String.Empty;
                Resultado.Resultado = "Ocurrio un error: " + ex.Message;
                Resultado.Estado = "Error";
            }
            return Resultado;
        }

    }
}
