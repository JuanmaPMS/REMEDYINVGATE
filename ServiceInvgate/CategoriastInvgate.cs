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
using Entities.Invgate;
using System.Xml.Linq;

namespace ServiceInvgate
{
    public class CategoriastInvgate
    {

        string user = ConfigurationManager.AppSettings["usernameInvgate"];
        string password = ConfigurationManager.AppSettings["passwordInvgate"];
        string UrlServicios = ConfigurationManager.AppSettings["UrlInvgate"];

        //public object GetAllCategorias()
        //{
        //    object data;
        //    try
        //    {
        //        var client = new RestClient(UrlServicios+ "/categories");

        //        var request = new RestRequest("", Method.Get);
        //        client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(user, password);

        //        var response = client.Execute(request);

        //        Console.WriteLine(response.Content);

        //        if (response.StatusCode == HttpStatusCode.OK)
        //        {
        //            data = JsonConvert.DeserializeObject<List<CategoriasInvgateResponse>>(response.Content);
        //        }
        //        else
        //        {
        //            data = new { Error = "Ocurrio un error: " + response.ErrorMessage };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        data = new { Error = "Ocurrio un error: " + ex.Message };
        //    }
        //    return data;
        //}

        //public ResultCategorias GetNombresCategoriasByProductoId(int idProducto)
        //{
        //    ResultCategorias data = new ResultCategorias();

        //    List<CategoriasInvgateResponse> listaCategoriasAll = new List<CategoriasInvgateResponse>();
        //    Dictionary<int, string> categorias_ = new Dictionary<int, string>();
        //    try
        //    {
        //        var client = new RestClient(UrlServicios + "/categories");

        //        var request = new RestRequest("", Method.Get);
        //        client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(user, password);

        //        var response = client.Execute(request);

        //        //Console.WriteLine(response.Content);

        //        if (response.StatusCode == HttpStatusCode.OK)
        //        {
        //            listaCategoriasAll = JsonConvert.DeserializeObject<List<CategoriasInvgateResponse>>(response.Content);

        //        }
        //        else
        //        {
        //            data.exito = false;
        //            data.error = response.ErrorMessage;
        //            data.categorias = null;
        //            return data;
        //        }

        //        int idPadre = 0;

        //        //Se agrega el producto, seria el ultimo

        //        if (listaCategoriasAll.Where(x => x.id == idProducto).ToList().Count == 0)
        //        {
        //            data.exito = false;
        //            data.error = "No se encontró el producto";
        //            data.categorias = categorias_;
        //            return data;
        //        }

        //        idPadre = listaCategoriasAll.Where(x => x.id == idProducto).First().parent_category_id;
        //        categorias_.Add(7, listaCategoriasAll.Where(x => x.id == idProducto).First().name);

        //        for (int i = 6; i >= 0; i--)
        //        {
        //            if (idPadre != 0)
        //            {
        //                categorias_.Add(i, listaCategoriasAll.Where(x => x.id == idPadre).First().name);
        //                idPadre = listaCategoriasAll.Where(x => x.id == idPadre).First().parent_category_id;
        //            }
        //            else
        //            {
        //                categorias_.Add(i, null);
        //            }
        //        }

        //        if (categorias_.Where(x => x.Value != null).ToList().Count != 8)
        //        {
        //            data.exito = false;
        //            data.error = "El id proporcionado no corresponde a un producto de la MESA DE SERVICIO IMSS, por lo cúal no se encontraron todas las categorias";
        //            data.categorias = categorias_;
        //            return data;
        //        }

        //        if (categorias_.Where(x => x.Key == 0).First().Value != "MESA DE SERVICIO IMSS")
        //        {
        //            data.exito = false;
        //            data.error = "Las categorias encontradas no corresponden a MESA DE SERVICIO IMSS";
        //            data.categorias = categorias_;
        //            return data;
        //        }


        //        categorias_ = categorias_.OrderBy(x => x.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
        //        data.exito = true;
        //        data.error = null;
        //        data.categorias = categorias_;

        //    }
        //    catch (Exception ex)
        //    {
        //        data.exito = false;
        //        data.error = ex.Message;
        //        data.categorias = null;

        //    }
        //    return data;
        //}


        //public ResultCategorias GetNombresCategoriasByProductoNombre(string nombreProducto)
        //{
        //    ResultCategorias data = new ResultCategorias();

        //    List<CategoriasInvgateResponse> listaCategoriasAll = new List<CategoriasInvgateResponse>();
        //    Dictionary<int, string> categorias_ = new Dictionary<int, string>();
        //    try
        //    {
        //        var client = new RestClient(UrlServicios + "/categories");

        //        var request = new RestRequest("", Method.Get);
        //        client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(user, password);

        //        var response = client.Execute(request);

        //        //Console.WriteLine(response.Content);

        //        if (response.StatusCode == HttpStatusCode.OK)
        //        {
        //            listaCategoriasAll = JsonConvert.DeserializeObject<List<CategoriasInvgateResponse>>(response.Content);

        //        }
        //        else
        //        {
        //            data.exito = false;
        //            data.error = response.ErrorMessage;
        //            data.categorias = null;
        //            return data;
        //        }

        //        int idPadre = 0;

        //        //Se agrega el producto, seria el ultimo
        //        var sss = listaCategoriasAll.Where(x => x.name == nombreProducto).ToList();
        //        if (listaCategoriasAll.Where(x => x.name == nombreProducto).ToList().Count == 0)
        //        {
        //            data.exito = false;
        //            data.error = "No se encontró el producto";
        //            data.categorias = categorias_;
        //            return data;
        //        }

        //        idPadre = listaCategoriasAll.Where(x => x.name == nombreProducto).First().parent_category_id;
        //        categorias_.Add(7, listaCategoriasAll.Where(x => x.name == nombreProducto).First().name);

        //        for (int i = 6; i >= 0; i--)
        //        {
        //            if (idPadre != 0)
        //            {
        //                categorias_.Add(i, listaCategoriasAll.Where(x => x.id == idPadre).First().name);
        //                idPadre = listaCategoriasAll.Where(x => x.id == idPadre).First().parent_category_id;
        //            }
        //            else
        //            {
        //                categorias_.Add(i, null);
        //            }
        //        }

        //        if (categorias_.Where(x => x.Value != null).ToList().Count != 8)
        //        {
        //            data.exito = false;
        //            data.error = "El id proporcionado no corresponde a un producto de la MESA DE SERVICIO IMSS, por lo cúal no se encontraron todas las categorias";
        //            data.categorias = categorias_;
        //            return data;
        //        }

        //        if (categorias_.Where(x => x.Key == 0).First().Value != "MESA DE SERVICIO IMSS")
        //        {
        //            data.exito = false;
        //            data.error = "Las categorias encontradas no corresponden a MESA DE SERVICIO IMSS";
        //            data.categorias = categorias_;
        //            return data;
        //        }


        //        categorias_ = categorias_.OrderBy(x => x.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
        //        data.exito = true;
        //        data.error = null;
        //        data.categorias = categorias_;

        //    }
        //    catch (Exception ex)
        //    {
        //        data.exito = false;
        //        data.error = ex.Message;
        //        data.categorias = null;

        //    }
        //    return data;
        //}



        //public ResultCategorias GetIdsCategoriasByProductoId(int idProducto)
        //{
        //    ResultCategorias data = new ResultCategorias();

        //    List<CategoriasInvgateResponse> listaCategoriasAll = new List<CategoriasInvgateResponse>();
        //    Dictionary<int, int?> categorias_ = new Dictionary<int, int?>();
        //    try
        //    {
        //        var client = new RestClient(UrlServicios + "/categories");

        //        var request = new RestRequest("", Method.Get);
        //        client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(user, password);

        //        var response = client.Execute(request);

        //        //Console.WriteLine(response.Content);

        //        if (response.StatusCode == HttpStatusCode.OK)
        //        {
        //            listaCategoriasAll = JsonConvert.DeserializeObject<List<CategoriasInvgateResponse>>(response.Content);

        //        }
        //        else
        //        {
        //            data.exito = false;
        //            data.error = response.ErrorMessage;
        //            data.categorias = null;
        //            return data;
        //        }

        //        int idPadre = 0;

        //        //Se agrega el producto, seria el ultimo

        //        if (listaCategoriasAll.Where(x => x.id == idProducto).ToList().Count == 0)
        //        {
        //            data.exito = false;
        //            data.error = "No se encontró el producto";
        //            data.categorias = categorias_;
        //            return data;
        //        }

        //        idPadre = listaCategoriasAll.Where(x => x.id == idProducto).First().parent_category_id;
        //        categorias_.Add(7, listaCategoriasAll.Where(x => x.id == idProducto).First().id);

        //        for (int i = 6; i >= 0; i--)
        //        {
        //            if (idPadre != 0)
        //            {
        //                categorias_.Add(i, listaCategoriasAll.Where(x => x.id == idPadre).First().id);
        //                idPadre = listaCategoriasAll.Where(x => x.id == idPadre).First().parent_category_id;
        //            }
        //            else
        //            {
        //                categorias_.Add(i, null);
        //            }
        //        }

        //        if (categorias_.Where(x => x.Value != null).ToList().Count != 8)
        //        {
        //            data.exito = false;
        //            data.error = "El id proporcionado no corresponde a un producto de la MESA DE SERVICIO IMSS, por lo cúal no se encontraron todas las categorias";
        //            data.categorias = categorias_;
        //            return data;
        //        }
        //        //1069==> "MESA DE SERVICIO IMSS"
        //        if (categorias_.Where(x => x.Key == 0).First().Value != 1069)
        //        {
        //            data.exito = false;
        //            data.error = "Las categorias encontradas no corresponden a MESA DE SERVICIO IMSS";
        //            data.categorias = categorias_;
        //            return data;
        //        }


        //        categorias_ = categorias_.OrderBy(x => x.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
        //        data.exito = true;
        //        data.error = null;
        //        data.categorias = categorias_;

        //    }
        //    catch (Exception ex)
        //    {
        //        data.exito = false;
        //        data.error = ex.Message;
        //        data.categorias = null;

        //    }
        //    return data;
        //}


        //public ResultCategorias GetIdsCategoriasByProductoNombre(string nombreProducto)
        //{
        //    ResultCategorias data = new ResultCategorias();

        //    List<CategoriasInvgateResponse> listaCategoriasAll = new List<CategoriasInvgateResponse>();
        //    Dictionary<int, int?> categorias_ = new Dictionary<int, int?>();
        //    try
        //    {
        //        var client = new RestClient(UrlServicios + "/categories");

        //        var request = new RestRequest("", Method.Get);
        //        client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(user, password);

        //        var response = client.Execute(request);

        //        //Console.WriteLine(response.Content);

        //        if (response.StatusCode == HttpStatusCode.OK)
        //        {
        //            listaCategoriasAll = JsonConvert.DeserializeObject<List<CategoriasInvgateResponse>>(response.Content);

        //        }
        //        else
        //        {
        //            data.exito = false;
        //            data.error = response.ErrorMessage;
        //            data.categorias = null;
        //            return data;
        //        }

        //        int idPadre = 0;

        //        //Se agrega el producto, seria el ultimo

        //        if (listaCategoriasAll.Where(x => x.name == nombreProducto).ToList().Count == 0)
        //        {
        //            data.exito = false;
        //            data.error = "No se encontró el producto";
        //            data.categorias = categorias_;
        //            return data;
        //        }

        //        idPadre = listaCategoriasAll.Where(x => x.name == nombreProducto).First().parent_category_id;
        //        categorias_.Add(7, listaCategoriasAll.Where(x => x.name == nombreProducto).First().id);

        //        for (int i = 6; i >= 0; i--)
        //        {
        //            if (idPadre != 0)
        //            {
        //                categorias_.Add(i, listaCategoriasAll.Where(x => x.id == idPadre).First().id);
        //                idPadre = listaCategoriasAll.Where(x => x.id == idPadre).First().parent_category_id;
        //            }
        //            else
        //            {
        //                categorias_.Add(i, null);
        //            }
        //        }

        //        if (categorias_.Where(x => x.Value != null).ToList().Count != 8)
        //        {
        //            data.exito = false;
        //            data.error = "El id proporcionado no corresponde a un producto de la MESA DE SERVICIO IMSS, por lo cúal no se encontraron todas las categorias";
        //            data.categorias = categorias_;
        //            return data;
        //        }

        //        //1069==> "MESA DE SERVICIO IMSS"
        //        if (categorias_.Where(x => x.Key == 0).First().Value != 1069)
        //        {
        //            data.exito = false;
        //            data.error = "Las categorias encontradas no corresponden a MESA DE SERVICIO IMSS";
        //            data.categorias = categorias_;
        //            return data;
        //        }


        //        categorias_ = categorias_.OrderBy(x => x.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
        //        data.exito = true;
        //        data.error = null;
        //        data.categorias = categorias_;

        //    }
        //    catch (Exception ex)
        //    {
        //        data.exito = false;
        //        data.error = ex.Message;
        //        data.categorias = null;

        //    }
        //    return data;
        //}


        public ResultCategorias GetNombresCategoriasByProductoNombreArreglo(string cadena)
        {
            ResultCategorias data = new ResultCategorias();

            List<CategoriasInvgateResponse> listaCategoriasAll = new List<CategoriasInvgateResponse>();
            List<CategoriasInvgateResponse> listaCategoriasEncontradas = new List<CategoriasInvgateResponse>();
            
            var arregloSplit = cadena.Split('|');
            
            int indexAuxiliar = 0;
            string producto = null;

            for (int i = (arregloSplit.Length - 1); i > 0; i--)
            {
                if (indexAuxiliar == 0) //NO SE HA DEFINIDO VALOR 
                {
                    if (arregloSplit[i] != "") {
                        //SE DEFINE CUAL ES EL ULTIMO NIVEL
                        indexAuxiliar = i;
                        producto = arregloSplit[i];
                    }
                }
            }

            List<CategoriaComplemento> diccionarioBase = new List<CategoriaComplemento>();

            ////SE INICIALIZA
            //for (int i = 0; i < arregloSplit.Length; i++)
            //{
            //    diccionarioBase.Add(new CategoriaComplemento { nivel = i, categoria = "", idCategoria = null });
            //}

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

                
                //Se agrega el producto, seria el ultimo
                listaCategoriasEncontradas = listaCategoriasAll.Where(x => x.name == producto).ToList();

                if (listaCategoriasEncontradas.Count == 0) { 
                    List<CategoriaComplemento> lista = new List<CategoriaComplemento>();
                    CategoriasInvgateResponse mesa = listaCategoriasAll.Where((x) => x.name == "MESA DE SERVICIO IMSS").First();
                    lista.Add(new CategoriaComplemento() { nivel = 0, idCategoria = mesa.id, categoria = mesa.name });
                    
                    data.exito = true;
                    data.error = null;

                    //var aaa = JsonConvert.SerializeObject(listaCategoriasEncontradas);
                    data.categorias = lista;
                    return data;
                }

                listaCategoriasEncontradas.ForEach((x)=>
                        {
                            //x.diccionario = diccionarioBase;

                            x.diccionario = new List<CategoriaComplemento>();
                            //SE INICIALIZA
                            for (int i = 0; i < arregloSplit.Length; i++)
                            {
                                x.diccionario.Add(new CategoriaComplemento { nivel = i, categoria = "", idCategoria = null });
                            }


                            int idPadre = 0;

                            x.diccionario[indexAuxiliar].idCategoria = x.id;
                            x.diccionario[indexAuxiliar].categoria = x.name;
                            idPadre = x.parent_category_id;

                            for (int i = (indexAuxiliar - 1); i >= 0; i--)
                            {
                                if (idPadre != 0)
                                {
                                    CategoriasInvgateResponse categoriaEncontrada = listaCategoriasAll.Where(y => y.id == idPadre).First();

                                    x.diccionario[i].idCategoria = categoriaEncontrada.id;
                                    x.diccionario[i].categoria = categoriaEncontrada.name;
                                    idPadre = categoriaEncontrada.parent_category_id;
                                }
                            }

                            for (int i = 0; i < x.diccionario.Count; i++)
                            {
                                if (i == 0)
                                {
                                    x.diccionarioCadena = x.diccionario[i].categoria;
                                }
                                else {
                                    x.diccionarioCadena += "|"+ x.diccionario[i].categoria;
                                }
                            }
                        });
                data.exito = true;
                data.error = null;

                //var aaa = JsonConvert.SerializeObject(listaCategoriasEncontradas);
                data.categorias = listaCategoriasEncontradas.Where(x=> x.diccionarioCadena == cadena).First().diccionario;


            }
            catch (Exception ex)
            {
                data.exito = false;
                data.error = ex.Message;
                data.categorias = null;

            }
            return data;

        }

        public int GetCategoria(string cadena)
        {
            List<CategoriasInvgateResponse> listaCategoriasAll = new List<CategoriasInvgateResponse>();
            List<CategoriasInvgateResponse> listaCategoriasEncontradas = new List<CategoriasInvgateResponse>();
            int IdCategoria = 0;
            var arregloSplit = cadena.Split('|');

            try
            {
                var client = new RestClient(UrlServicios + "/categories");

                var request = new RestRequest("", Method.Get);
                client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(user, password);

                var response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    listaCategoriasAll = JsonConvert.DeserializeObject<List<CategoriasInvgateResponse>>(response.Content);

                    int idParent = 0;
                    foreach (string elemento in arregloSplit)
                    {
                        listaCategoriasEncontradas = listaCategoriasAll.Where(x => x.parent_category_id == idParent && x.name == elemento).ToList();

                        if (listaCategoriasEncontradas.Count > 0)
                            idParent = listaCategoriasEncontradas[0].id;
                    }

                    if (idParent > 0)
                        IdCategoria = idParent;
                    else
                        IdCategoria = 1069;//Se asigna por defecto el Id de la Mesa IMSS
                }
                else 
                {
                    IdCategoria = 1069;//Se asigna por defecto el Id de la Mesa IMSS
                }
            }
            catch//(Exception ex)
            {
                IdCategoria = 1069;//Se asigna por defecto el Id de la Mesa IMSS
            }

            return IdCategoria;
        }


        public Categorizacion GetCategorizacion(int IdCategoria)
        {
            Categorizacion data = new Categorizacion();

            List<CategoriasInvgateResponse> listaCategoriasAll = new List<CategoriasInvgateResponse>();
            List<CategoriasInvgateResponse> listaCategoriasEncontradas = new List<CategoriasInvgateResponse>();

            try
            {
                var client = new RestClient(UrlServicios + "/categories");

                var request = new RestRequest("", Method.Get);
                client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(user, password);

                var response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    listaCategoriasAll = JsonConvert.DeserializeObject<List<CategoriasInvgateResponse>>(response.Content);

                    string[] arr = new string[7];

                    int idParent = IdCategoria;

                    for (int i = 6;i >= 0; i--)
                    {
                        listaCategoriasEncontradas = listaCategoriasAll.Where(x => x.id == idParent).ToList();

                        if (listaCategoriasEncontradas.Count > 0)
                        {
                            arr[i] = listaCategoriasEncontradas[0].name;
                            idParent = listaCategoriasEncontradas[0].parent_category_id;
                        }                        
                    }

                    int x_ = 0;
                    string[] arr2 = new string[7];
                    for (int i = 0; i < arr.Length; i++)
                    {  
                        if (arr[i] != null)
                        {
                            arr2[x_] = arr[i];
                            x_ ++;
                        }
                    }

                    data.CatOperacion01 = arr2[1] == null ? "" : arr2[1];
                    data.CatOperacion02 = arr2[2] == null ? "" : arr2[2];
                    data.CatOperacion03 = arr2[3] == null ? "" : arr2[3];
                    data.CatProducto01 = arr2[4] == null ? "" : arr2[4];
                    data.CatProducto02 = arr2[5] == null ? "" : arr2[5];
                    data.CatProducto03 = arr2[6] == null ? "" : arr2[6];

                }
            }
            catch//(Exception ex)
            {
            }

            return data;
        }

    }
}
