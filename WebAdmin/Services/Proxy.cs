using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebAdmin.Models;

namespace WebAdmin.Services
{
    public class Proxy
    {
        string BaseAddress = "http://localhost:56134/";

        #region Peticiones POST AND GET

        public async Task<T> SendPost<T, PostData>(string requestURI, PostData data)
        {
            T Result = default(T);
            using (var Client = new HttpClient())
            {
                try
                {

                    requestURI = BaseAddress + requestURI;
                    Client.DefaultRequestHeaders.Accept.Clear();
                    Client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    var JsonData = JsonConvert.SerializeObject(data);
                    HttpResponseMessage Response =
                        await Client.PostAsync(requestURI,
                        new StringContent(JsonData.ToString(),
                        Encoding.UTF8, "application/json"));
                    var ResultWebAPI = await Response.Content.ReadAsStringAsync();
                    Result = JsonConvert.DeserializeObject<T>(ResultWebAPI);

                }
                catch (Exception)
                {

                    throw;
                }
            }
            return Result;
        }

        // Peticiones GET
        public async Task<T> SendGet<T>(string requesURI)
        {
            T Result = default(T);
            using (var Client = new HttpClient())
            {
                try
                {
                    requesURI = BaseAddress + requesURI;

                    Client.DefaultRequestHeaders.Accept.Clear();
                    Client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    var ResultJSON = await Client.GetStringAsync(requesURI);
                    Result = JsonConvert.DeserializeObject<T>(ResultJSON);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return Result;
        }
        #endregion

        public async Task<List<Cases>> GetAllAsync()
        {
            return await SendGet<List<Cases>>(
                "/Cases/ClientApi");
        }
        public List<Cases> GetAll()
        {
            List<Cases> Result = null;
            Task.Run(async () => Result = await GetAllAsync()).Wait();
            return Result;
        }

        public async Task<List<Cases>> GetByidAsync(int id)
        {
            return await SendGet<List<Cases>>($"/Cases/GetById/{id}");
        }

        public List<Cases> GetByid(int id)
        {
            List<Cases> Result = null;
            Task.Run(async () => Result = await GetByidAsync(id)).Wait();
            return Result;
        }
    }
}
