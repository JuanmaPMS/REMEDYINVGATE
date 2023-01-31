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

namespace ServiceInvgate
{
    public class Api
    {
        public dynamic Get(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Proxy = null;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(stream);

            string strData = HttpUtility.HtmlDecode(streamReader.ReadToEnd());

            dynamic data = JsonConvert.DeserializeObject(strData);

            return data;
        }

        public object GetIncidente(IncidentesGetRequest incidente)
        {
            object data;
            try
            {

                var properties = from p in incidente.GetType().GetProperties()
                                 where p.GetValue(incidente, null) != null
                                 select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(incidente).ToString());

                string parametros = String.Join("&", properties.ToArray());

                var client = new RestClient("https://servicio.grupopm.mx/api/v1/incident"+"?" + parametros);

                var request = new RestRequest("", Method.Get);
                client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator("remedy-imss", "TndbhizYaot15zCfNqbaoon9");
                //request.AddHeader("Authorization", "Basic cmVtZWR5LWltc3M6VG5kYmhpellhb3QxNXpDZk5xYmFvb245");
                //request.AddHeader("Cookie", "PHPSESSID=b3a2974d3b4cb6fcb4ea992dcb6997a8");
                //request.AddHeader("Content-Type", "application/json");

                var obj = JsonConvert.SerializeObject(incidente);

                //request.AddParameter("application/json", incidente, ParameterType.RequestBody);
                var response = client.Execute(request);

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

        public dynamic Post(string url, string json = null, string autorizacion = null)
        {
            dynamic data;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var cliente = new RestClient(url);
                var request = new RestRequest("",Method.Post);
                request.AddHeader("content-type", "application/json");
                if (!string.IsNullOrEmpty(json))
                    request.AddParameter("application/json", json, ParameterType.RequestBody);
                if (!string.IsNullOrEmpty(autorizacion))
                    request.AddHeader("Authorization", autorizacion);

                var response = cliente.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    data = JsonConvert.DeserializeObject(response.Content);
                }
                else
                {
                    data = new {Error = "Ocurrio un error: " + response.ErrorMessage};
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
