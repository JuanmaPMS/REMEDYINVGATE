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

namespace ServiceInvgate
{
    public class IncidentesInvgate
    {

        string user = ConfigurationManager.AppSettings["usernameInvgate"];
        string password = ConfigurationManager.AppSettings["passwordInvgate"];
        string UrlServicios = ConfigurationManager.AppSettings["UrlInvgate"];

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

        public object PostIncidente(IncidentesPostRequest incidente)
        {

            var a = JsonConvert.SerializeObject(incidente);
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

        public object PutIncidente(IncidentesPutRequest incidente)
        {
            var a = JsonConvert.SerializeObject(incidente);
            object data;
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

    }
}
