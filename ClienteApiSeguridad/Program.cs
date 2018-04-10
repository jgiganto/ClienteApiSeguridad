using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace ClienteApiSeguridad
{
    class Program
    {
        static String UrlApi = "http://localhost:55971/";
        static MediaTypeWithQualityHeaderValue media =
            new MediaTypeWithQualityHeaderValue("application/json");
        static   void Main(string[] args)
        {
              AccesoApi();
            Console.ReadLine();

        }

        public static async Task AccesoApi()
        {
            String token = await GetToken();
            using (HttpClient cliente = new HttpClient())
            {
                String peticion = "api/Empleados";
                cliente.BaseAddress = new Uri(UrlApi);
                cliente.DefaultRequestHeaders.Accept.Clear();
                cliente.DefaultRequestHeaders.Accept.Add(media);
                cliente.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                HttpResponseMessage respuesta =
                    await cliente.GetAsync(peticion);
                String datos =
                    await respuesta.Content.ReadAsStringAsync();
                Console.Write(datos);
                Console.ReadLine();
            }
        }

        public static async Task<String> GetToken()
        {
            using (HttpClient cliente = new HttpClient())
            {
                String peticion = "tokenempleado";
                cliente.BaseAddress = new Uri(UrlApi);
                cliente.DefaultRequestHeaders.Accept.Clear();
                cliente.DefaultRequestHeaders.Accept.Add(media);
                KeyValuePair<String, string> grant =
                    new KeyValuePair<string, string>("grant_type", "password");
                KeyValuePair<string, string> username =
                    new KeyValuePair<string, string>("username", "NEGRO");
                KeyValuePair<string, string> password =
                    new KeyValuePair<string, string>("password", "7698");
                //las credenciales son transportadas en un formulario encoded
                FormUrlEncodedContent formulario = new FormUrlEncodedContent(new[] { grant, username, password });

                HttpResponseMessage respuesta =
                    await cliente.PostAsync(peticion, formulario);
                //vamos a leer directamente todo el json
                String json = await  respuesta.Content.ReadAsStringAsync();
                //convertimos el String json a objeto
                JObject objeto = JObject.Parse(json);
                //recuperamos la clave del token que esta en access_token
                String token = objeto.GetValue("access_token").ToString();
                return token;

            }



        }
    }
}
