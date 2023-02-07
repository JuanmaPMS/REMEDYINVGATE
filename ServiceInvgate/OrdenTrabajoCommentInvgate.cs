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
    public class OrdenTrabajoCommentInvgate
    {

        string user = ConfigurationManager.AppSettings["usernameInvgate"];
        string password = ConfigurationManager.AppSettings["passwordInvgate"];
        string UrlServicios = ConfigurationManager.AppSettings["UrlInvgate"];

        public object GetOrdenTrabajoComment(OrdenTrabajoCommentGetRequest comentario)
        {
            var a = JsonConvert.SerializeObject(comentario);
            object data;
            try
            {

                var properties = from p in comentario.GetType().GetProperties()
                                 where p.GetValue(comentario, null) != null
                                 select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(comentario).ToString());

                string parametros = String.Join("&", properties.ToArray());

                var client = new RestClient(UrlServicios+ "/incident.comment" + "?" + parametros);

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

        public Entities.Intermedio.Result PostOrdenTrabajoComment(IncidentesCommentPostRequest comentario)
        {

            IncidentesCommentPostResponse postResponse = new IncidentesCommentPostResponse();
            Entities.Intermedio.Result Resultado = new Entities.Intermedio.Result();
            try
            {
                var client = new RestClient(UrlServicios + "/incident.comment");
                var request = new RestRequest("", Method.Post);
                client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(user, password);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", comentario, ParameterType.RequestBody);
                var response = client.Execute(request);

                Console.WriteLine(response.Content);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    postResponse = JsonConvert.DeserializeObject<IncidentesCommentPostResponse>(response.Content);
                    Resultado.Ticket = comentario.request_id.ToString();
                    Resultado.Resultado = "Transacción exitosa, registro agregado a Invgate People Media";
                    Resultado.Estado = "Exito";
                }
                else
                {
                    //data = new { Error = "Ocurrio un error: " + response.ErrorMessage };
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



    }
}
