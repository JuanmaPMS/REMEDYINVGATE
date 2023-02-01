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
    public class CategoriastInvgate
    {

        string user = ConfigurationManager.AppSettings["usernameInvgate"];
        string password = ConfigurationManager.AppSettings["passwordInvgate"];
        string UrlServicios = ConfigurationManager.AppSettings["UrlInvgate"];

        public object GetAllCategorias()
        {
            object data;
            try
            {
                var client = new RestClient(UrlServicios+ "/categories");

                var request = new RestRequest("", Method.Get);
                client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(user, password);

                var response = client.Execute(request);

                Console.WriteLine(response.Content);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    data = JsonConvert.DeserializeObject<List<CategoriasInvgateResponse>>(response.Content);
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

        public ResultCategorias GetNombresCategoriasByProductoId(int idProducto)
        {
            ResultCategorias data = new ResultCategorias();

            List<CategoriasInvgateResponse> listaCategoriasAll = new List<CategoriasInvgateResponse>();
            Dictionary<int, string> categorias_ = new Dictionary<int, string>();
            try
            {
                var client = new RestClient(UrlServicios + "/categories");

                var request = new RestRequest("", Method.Get);
                client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(user, password);

                var response = client.Execute(request);

                //Console.WriteLine(response.Content);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    listaCategoriasAll = JsonConvert.DeserializeObject<List<CategoriasInvgateResponse>>(response.Content);

                }
                else
                {
                    data.exito = false;
                    data.error = response.ErrorMessage;
                    data.categorias = null;
                    return data;
                }

                int idPadre = 0;

                //Se agrega el producto, seria el ultimo

                if (listaCategoriasAll.Where(x => x.id == idProducto).ToList().Count == 0)
                {
                    data.exito = false;
                    data.error = "No se encontró el producto";
                    data.categorias = categorias_;
                    return data;
                }

                idPadre = listaCategoriasAll.Where(x => x.id == idProducto).First().parent_category_id;
                categorias_.Add(7, listaCategoriasAll.Where(x => x.id == idProducto).First().name);

                for (int i = 6; i >= 0; i--)
                {
                    if (idPadre != 0)
                    {
                        categorias_.Add(i, listaCategoriasAll.Where(x => x.id == idPadre).First().name);
                        idPadre = listaCategoriasAll.Where(x => x.id == idPadre).First().parent_category_id;
                    }
                    else
                    {
                        categorias_.Add(i, null);
                    }
                }

                if (categorias_.Where(x => x.Value != null).ToList().Count != 8)
                {
                    data.exito = false;
                    data.error = "El id proporcionado no corresponde a un producto de la MESA DE SERVICIO IMSS, por lo cúal no se encontraron todas las categorias";
                    data.categorias = categorias_;
                    return data;
                }

                if (categorias_.Where(x => x.Key == 0).First().Value != "MESA DE SERVICIO IMSS")
                {
                    data.exito = false;
                    data.error = "Las categorias encontradas no corresponden a MESA DE SERVICIO IMSS";
                    data.categorias = categorias_;
                    return data;
                }


                categorias_ = categorias_.OrderBy(x => x.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
                data.exito = true;
                data.error = null;
                data.categorias = categorias_;

            }
            catch (Exception ex)
            {
                data.exito = false;
                data.error = ex.Message;
                data.categorias = null;

            }
            return data;
        }


        public ResultCategorias GetNombresCategoriasByProductoNombre(string nombreProducto)
        {
            ResultCategorias data = new ResultCategorias();

            List<CategoriasInvgateResponse> listaCategoriasAll = new List<CategoriasInvgateResponse>();
            Dictionary<int, string> categorias_ = new Dictionary<int, string>();
            try
            {
                var client = new RestClient(UrlServicios + "/categories");

                var request = new RestRequest("", Method.Get);
                client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(user, password);

                var response = client.Execute(request);

                //Console.WriteLine(response.Content);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    listaCategoriasAll = JsonConvert.DeserializeObject<List<CategoriasInvgateResponse>>(response.Content);

                }
                else
                {
                    data.exito = false;
                    data.error = response.ErrorMessage;
                    data.categorias = null;
                    return data;
                }

                int idPadre = 0;

                //Se agrega el producto, seria el ultimo

                if (listaCategoriasAll.Where(x => x.name == nombreProducto).ToList().Count == 0)
                {
                    data.exito = false;
                    data.error = "No se encontró el producto";
                    data.categorias = categorias_;
                    return data;
                }

                idPadre = listaCategoriasAll.Where(x => x.name == nombreProducto).First().parent_category_id;
                categorias_.Add(7, listaCategoriasAll.Where(x => x.name == nombreProducto).First().name);

                for (int i = 6; i >= 0; i--)
                {
                    if (idPadre != 0)
                    {
                        categorias_.Add(i, listaCategoriasAll.Where(x => x.id == idPadre).First().name);
                        idPadre = listaCategoriasAll.Where(x => x.id == idPadre).First().parent_category_id;
                    }
                    else
                    {
                        categorias_.Add(i, null);
                    }
                }

                if (categorias_.Where(x => x.Value != null).ToList().Count != 8)
                {
                    data.exito = false;
                    data.error = "El id proporcionado no corresponde a un producto de la MESA DE SERVICIO IMSS, por lo cúal no se encontraron todas las categorias";
                    data.categorias = categorias_;
                    return data;
                }

                if (categorias_.Where(x => x.Key == 0).First().Value != "MESA DE SERVICIO IMSS")
                {
                    data.exito = false;
                    data.error = "Las categorias encontradas no corresponden a MESA DE SERVICIO IMSS";
                    data.categorias = categorias_;
                    return data;
                }


                categorias_ = categorias_.OrderBy(x => x.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
                data.exito = true;
                data.error = null;
                data.categorias = categorias_;

            }
            catch (Exception ex)
            {
                data.exito = false;
                data.error = ex.Message;
                data.categorias = null;

            }
            return data;
        }



        public ResultCategorias GetIdsCategoriasByProductoId(int idProducto)
        {
            ResultCategorias data = new ResultCategorias();

            List<CategoriasInvgateResponse> listaCategoriasAll = new List<CategoriasInvgateResponse>();
            Dictionary<int, int?> categorias_ = new Dictionary<int, int?>();
            try
            {
                var client = new RestClient(UrlServicios + "/categories");

                var request = new RestRequest("", Method.Get);
                client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(user, password);

                var response = client.Execute(request);

                //Console.WriteLine(response.Content);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    listaCategoriasAll = JsonConvert.DeserializeObject<List<CategoriasInvgateResponse>>(response.Content);

                }
                else
                {
                    data.exito = false;
                    data.error = response.ErrorMessage;
                    data.categorias = null;
                    return data;
                }

                int idPadre = 0;

                //Se agrega el producto, seria el ultimo

                if (listaCategoriasAll.Where(x => x.id == idProducto).ToList().Count == 0)
                {
                    data.exito = false;
                    data.error = "No se encontró el producto";
                    data.categorias = categorias_;
                    return data;
                }

                idPadre = listaCategoriasAll.Where(x => x.id == idProducto).First().parent_category_id;
                categorias_.Add(7, listaCategoriasAll.Where(x => x.id == idProducto).First().id);

                for (int i = 6; i >= 0; i--)
                {
                    if (idPadre != 0)
                    {
                        categorias_.Add(i, listaCategoriasAll.Where(x => x.id == idPadre).First().id);
                        idPadre = listaCategoriasAll.Where(x => x.id == idPadre).First().parent_category_id;
                    }
                    else
                    {
                        categorias_.Add(i, null);
                    }
                }

                if (categorias_.Where(x => x.Value != null).ToList().Count != 8)
                {
                    data.exito = false;
                    data.error = "El id proporcionado no corresponde a un producto de la MESA DE SERVICIO IMSS, por lo cúal no se encontraron todas las categorias";
                    data.categorias = categorias_;
                    return data;
                }
                //1069==> "MESA DE SERVICIO IMSS"
                if (categorias_.Where(x => x.Key == 0).First().Value != 1069)
                {
                    data.exito = false;
                    data.error = "Las categorias encontradas no corresponden a MESA DE SERVICIO IMSS";
                    data.categorias = categorias_;
                    return data;
                }


                categorias_ = categorias_.OrderBy(x => x.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
                data.exito = true;
                data.error = null;
                data.categorias = categorias_;

            }
            catch (Exception ex)
            {
                data.exito = false;
                data.error = ex.Message;
                data.categorias = null;

            }
            return data;
        }


        public ResultCategorias GetIdsCategoriasByProductoNombre(string nombreProducto)
        {
            ResultCategorias data = new ResultCategorias();

            List<CategoriasInvgateResponse> listaCategoriasAll = new List<CategoriasInvgateResponse>();
            Dictionary<int, int?> categorias_ = new Dictionary<int, int?>();
            try
            {
                var client = new RestClient(UrlServicios + "/categories");

                var request = new RestRequest("", Method.Get);
                client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(user, password);

                var response = client.Execute(request);

                //Console.WriteLine(response.Content);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    listaCategoriasAll = JsonConvert.DeserializeObject<List<CategoriasInvgateResponse>>(response.Content);

                }
                else
                {
                    data.exito = false;
                    data.error = response.ErrorMessage;
                    data.categorias = null;
                    return data;
                }

                int idPadre = 0;

                //Se agrega el producto, seria el ultimo

                if (listaCategoriasAll.Where(x => x.name == nombreProducto).ToList().Count == 0)
                {
                    data.exito = false;
                    data.error = "No se encontró el producto";
                    data.categorias = categorias_;
                    return data;
                }

                idPadre = listaCategoriasAll.Where(x => x.name == nombreProducto).First().parent_category_id;
                categorias_.Add(7, listaCategoriasAll.Where(x => x.name == nombreProducto).First().id);

                for (int i = 6; i >= 0; i--)
                {
                    if (idPadre != 0)
                    {
                        categorias_.Add(i, listaCategoriasAll.Where(x => x.id == idPadre).First().id);
                        idPadre = listaCategoriasAll.Where(x => x.id == idPadre).First().parent_category_id;
                    }
                    else
                    {
                        categorias_.Add(i, null);
                    }
                }

                if (categorias_.Where(x => x.Value != null).ToList().Count != 8)
                {
                    data.exito = false;
                    data.error = "El id proporcionado no corresponde a un producto de la MESA DE SERVICIO IMSS, por lo cúal no se encontraron todas las categorias";
                    data.categorias = categorias_;
                    return data;
                }

                //1069==> "MESA DE SERVICIO IMSS"
                if (categorias_.Where(x => x.Key == 0).First().Value != 1069)
                {
                    data.exito = false;
                    data.error = "Las categorias encontradas no corresponden a MESA DE SERVICIO IMSS";
                    data.categorias = categorias_;
                    return data;
                }


                categorias_ = categorias_.OrderBy(x => x.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
                data.exito = true;
                data.error = null;
                data.categorias = categorias_;

            }
            catch (Exception ex)
            {
                data.exito = false;
                data.error = ex.Message;
                data.categorias = null;

            }
            return data;
        }

    }
}
