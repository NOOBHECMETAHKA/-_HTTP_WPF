using Build_Shop_WPF.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows;

namespace Build_Shop_WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool CANINTER = false;

        HttpManipulation httpManipulation;

        public MainWindow()
        {
            InitializeComponent();
            httpManipulation = new HttpManipulation();
            tbLoginUser.Text = "NastyaLOH";
            tbPassword.Text = "password123";

            new RegistryClass().checkSystemFiles();

            Responce responce  = new Responce { access_token=valueSession(), username=""} ;
            if (responce != null && responce.access_token != null && responce.access_token != string.Empty)
            {
                SendJWT(App.URL + "users/refresh_token", JsonConvert.SerializeObject(responce));
            }
        }

        public string valueSession()
        {
            RegistryClass registry = new RegistryClass();
            string element = registry.readKeyValue(RegistryClass.nameProgram, RegistryClass.userKey);
            return element;
        }

        public void checkSetting(bool mode)
        {
            RegistryClass registry = new RegistryClass();
            if (mode)
            {
                string action = registry.readKeyValue(RegistryClass.nameProgram, RegistryClass.windowKey);
                if (action != null)
                {
                    if (action == "Disp")
                    {
                        new Dispatcher().Show();
                        this.Close();
                    }
                    else if (action == "Manager")
                    {
                        new Manager().Show();
                        this.Close();
                    }
                }

            }
        }

        async void SendPostRequest(string url, string data)
        {
            RegistryClass registry = new RegistryClass();
            using (var client = new HttpClient())
            {
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Responce responce = JsonConvert.DeserializeObject<Responce>(responseString);
                    if (responce != null)
                    {
                        registry.writeKeyValue(RegistryClass.nameProgram, RegistryClass.userKey, responce.access_token);
                        checkSetting(true);
                        Dispatcher goTo = new Dispatcher();
                        goTo.Show();
                        this.Close();
                    }
                }

            }
        }

        async void SendJWT(string url, string data)
        {
            RegistryClass registry = new RegistryClass();
            using (var client = new HttpClient())
            {
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Responce responce = JsonConvert.DeserializeObject<Responce>(responseString);
                    if (responce != null)
                    {
                        registry.writeKeyValue(RegistryClass.nameProgram, RegistryClass.userKey, responce.access_token);
                        checkSetting(true);
                    }
                }
            }
        }


        private void btLogIn_Click(object sender, RoutedEventArgs e)
        {
            User user = new User
            {
                IdUser = 0,
                FirstName = "",
                LastName = "",
                Email = "",
                Login = tbLoginUser.Text.Trim(),
                Password = tbPassword.Text.Trim(),
                PostId = 1,
                IsDeleted = false,
            };

            SendPostRequest(App.URL + "users/signin", JsonConvert.SerializeObject(user));
        }

        private void btSingUp_Click(object sender, RoutedEventArgs e)
        {
            new Registration().Show();
            this.Close();
        }
    }
}
