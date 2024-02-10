using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Build_Shop_WPF.Models;
using System.Windows;
using System.Security.Policy;
using Microsoft.Win32;

namespace Build_Shop_WPF
{
    internal class HttpManipulation
    {
        public HttpClient _httpClient;

        public HttpManipulation()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(App.URL);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return await _httpClient.GetAsync(requestUri);
        }

        public async Task<string> GetStringAsync(string requestUri)
        {
            return await _httpClient.GetStringAsync(requestUri);
        }

        public async Task<List<T>> GetObjects<T>(string requestUri)
        {
            var responce = _httpClient.GetStringAsync(requestUri);
            return JsonConvert.DeserializeObject<List<T>>(responce.ToString());
        }

        public async void PostAsync(string requestUri, string data)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(App.URL + requestUri, content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Данные успешно отправлены!");
                }
                else
                {
                    MessageBox.Show("Возникли ошибки!");
                }

            }
        }

        public async void PutAsync(string requestUri, string data)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(App.URL + requestUri, content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Данные успешно отправлены!");
                }
                else
                {
                    MessageBox.Show("Возникли проблемы с сервером!");
                }

            }
        }

        public async void DeleteAsync(string requestUri)
        {
            var response = await _httpClient.DeleteAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Данные успешно отправлены!");
            }
            else
            {
                MessageBox.Show("Возникли проблемы с сервером!");
            }

        }
    }
}
