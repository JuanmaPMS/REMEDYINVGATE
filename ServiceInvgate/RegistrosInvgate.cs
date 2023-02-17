using Entities;
using Entities.Invgate;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ServiceInvgate
{
    public class RegistrosInvgate
    {
        string UrlServicios = ConfigurationManager.AppSettings["UrlServiceControl"];
        public List<RegistrosRequest> Get()
        {
            List<RegistrosRequest> registros = new List<RegistrosRequest>();
            try
            {
                var client = new RestClient(UrlServicios + "/getControles");

                var request = new RestRequest("", Method.Get);
                var response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    registros = JsonConvert.DeserializeObject<List<RegistrosRequest>>(response.Content);
                }
                else
                {
                    registros = new List<RegistrosRequest>();
                }
            }
            catch
            {
                registros = new List<RegistrosRequest>();
            }

            return registros;
        }

        public bool Procesados(List<int> idProcesados)
        {
            bool sucess = false;
            try
            {
                var client = new RestClient(UrlServicios + "/ActualizarLoteProcesados");
                var request = new RestRequest("", Method.Post);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", idProcesados, ParameterType.RequestBody);
                var response = client.Execute(request);

                Console.WriteLine(response.Content);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    sucess = true;
                }
                else
                {
                    sucess = false;
                }
            }
            catch
            {
                sucess = false;
            }

            return sucess;
        }
    }
}
