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
