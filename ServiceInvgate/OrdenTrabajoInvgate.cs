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
    public class OrdenTrabajoInvgate
    {

        string user = ConfigurationManager.AppSettings["usernameInvgate"];
        string password = ConfigurationManager.AppSettings["passwordInvgate"];
        string UrlServicios = ConfigurationManager.AppSettings["UrlInvgate"];

        public object GetOrdenTrabajo(OrdenTrabajoGetRequest OrdenTrabajo)
        {
            object data;
            try
            {

                var properties = from p in OrdenTrabajo.GetType().GetProperties()
                                 where p.GetValue(OrdenTrabajo, null) != null
                                 select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(OrdenTrabajo).ToString());

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

        public object PostOrdenTrabajo(OrdenTrabajoPostRequest OrdenTrabajo)
        {

            var a = JsonConvert.SerializeObject(OrdenTrabajo);
            object data;
            try
            {
                var client = new RestClient(UrlServicios + "/incident" );
                var request = new RestRequest("", Method.Post);
                client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(user, password);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", OrdenTrabajo, ParameterType.RequestBody);
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

        public object PutOrdenTrabajo(OrdenTrabajoPutRequest OrdenTrabajo)
        {
            var a = JsonConvert.SerializeObject(OrdenTrabajo);
            object data;
            try
            {
                var client = new RestClient(UrlServicios + "/incident");
                var request = new RestRequest("", Method.Put);
                client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(user, password);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", OrdenTrabajo, ParameterType.RequestBody);
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
