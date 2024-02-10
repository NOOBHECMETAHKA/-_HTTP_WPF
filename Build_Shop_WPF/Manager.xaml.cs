using Build_Shop_WPF.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Win32;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using System.Xml.Linq;
using System.Diagnostics.Contracts;
using System.Net;

namespace Build_Shop_WPF
{
    /// <summary>
    /// Логика взаимодействия для Manager.xaml
    /// </summary>
    public partial class Manager : Window
    {
        RegistryClass pageChecker;
        ElementFiller filler;
        HttpManipulation manipulation;

        //Settings
        //api/objects/pages/{Number}/{Size}
        int CountElementPage = 2;

        public Manager()
        {
            filler = new ElementFiller();
            manipulation = new HttpManipulation();
            InitializeComponent();
            pageChecker = new RegistryClass();
            string action = pageChecker.readKeyValue(RegistryClass.nameProgram, RegistryClass.windowKey);
            string actionPage = pageChecker.readKeyValue(RegistryClass.nameProgram, RegistryClass.pageValueKey);
            if (actionPage != null && actionPage != string.Empty && action == "Manager")
            {
                TC_Manager.SelectedIndex = int.Parse(actionPage);
            }
        }


        private void datagrids_Loaded(object sender, RoutedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid.Name == dg_Client.Name)
            {
                filler.FillDataGrid<Client>(dataGrid, $"clients/pages/1/{CountElementPage}");
                lb_PageUsers.Text = "1";

            } else if (dataGrid.Name == dg_Address.Name)
            {
                filler.FillDataGrid<Build_Shop_WPF.Models.Address>(dataGrid, $"addresses/pages/1/{CountElementPage}");
                lb_Addresses.Text = "1";
            }
            else if (dataGrid.Name == dg_Order.Name)
            {
                filler.FillDataGrid<Order>(dataGrid, $"orders/pages/1/{CountElementPage}");
                lb_Orders.Text = "1";
            }
            else if (dataGrid.Name == dg_contractor.Name)
            {
                filler.FillDataGrid<Contractor>(dataGrid, $"contractors/pages/1/{CountElementPage}");
                lb_Contractors.Text = "1";
            }
            else if (dataGrid.Name == dg_consumption.Name)
            {
                filler.FillDataGrid<Consumption>(dataGrid, $"consumptions/pages/1/{CountElementPage}");
                lb_Consumptions.Text = "1";
            }
        }


        //Client row = dg_Client.SelectedItems[0] as Client;
        //MessageBox.Show(row.IdClient.ToString());
        private void bt_InsertUser_Click(object sender, RoutedEventArgs e) {
            if(tb_FirstName.Text.Length > 1 || tb_LastName.Text.Length > 1 || tb_NumberPhone.Text.Length > 8 || tb_Email.Text.Length > 3)
            {
                if (IsValidPhoneNumber(tb_NumberPhone.Text))
                {
                    if (IsValidEmail(tb_Email.Text))
                    {
                        Client client = new Client
                        {
                            IdClient = null,
                            FirstName = tb_FirstName.Text,
                            LastName = tb_LastName.Text,
                            PhoneNumber = tb_NumberPhone.Text,
                            Email = tb_Email.Text,
                            IsDeleted = null
                        };
                        string json = JsonConvert.SerializeObject(client);
                        manipulation.PostAsync("clients", json);
                    }
                    MessageBox.Show("Неверный формат электронной почта!");
                }
                MessageBox.Show("Неверный формат номера телефона!");
            }
            else
            {
                MessageBox.Show("Все поля надо заполнить!");
            }
        } 

        private void bt_UpdateUser_Click(object sender, RoutedEventArgs e)
        {
            List<Client> clients = dg_Client.SelectedItems.Cast<Client>().ToList();
            if (tb_FirstName.Text.Length > 1 || tb_LastName.Text.Length > 1 || tb_NumberPhone.Text.Length > 8 || tb_Email.Text.Length > 3)
            {
                if (IsValidPhoneNumber(tb_NumberPhone.Text))
                {
                    if (IsValidEmail(tb_Email.Text))
                    {
                        if (clients.Count < 0)
                        {
                            clients[0].FirstName = tb_FirstName.Text;
                            clients[0].LastName = tb_LastName.Text;
                            clients[0].PhoneNumber = tb_NumberPhone.Text;
                            clients[0].Email = tb_Email.Text;
                            string json = JsonConvert.SerializeObject(clients[0]);
                            manipulation.PutAsync("clients/" + clients[0].IdClient, json);
                        }
                        else
                        {
                            MessageBox.Show("Элемент не выбран!");
                        }
                    }
                    else MessageBox.Show("Неверный формат электронной почта!");
                }
                else MessageBox.Show("Неверный формат номера телефона!");
            }
            else
            {
                MessageBox.Show("Все поля надо заполнить!");
            }
        }

        private void tb_DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            List<Client> clients = dg_Client.SelectedItems.Cast<Client>().ToList();
            if (clients != null)
            {
                List<int> ints = new List<int>();
                for (int i = 0; i < clients.Count; i++)
                {
                    ints.Add((int)clients[i].IdClient);
                }
                string json = JsonConvert.SerializeObject(ints);
                manipulation.PutAsync("clients/deleteClients", json);
            }
        }

        public void PaginationDataGrid<T>(DataGrid dataGrid, Button button, TextBlock label, string forward, string back, string uriObject)
        {
            int page = int.Parse(label.Text);
            if ((button.Name == back) && (page != 1)) page--;
            else if ((button.Name == forward) && (dataGrid.Items.Count != 0)) page++;
            label.Text = page.ToString();
            filler.FillDataGrid<T>(dataGrid, $"{uriObject}/pages/{int.Parse(label.Text)}/{CountElementPage}");
        }

        private void bt_PaginationUser_Click(object sender, RoutedEventArgs e)
        {
            Button pressed_Button = sender as Button;
            int page = int.Parse(lb_PageUsers.Text);
            if (pressed_Button.Name == "bt_PaginationUser_Back" && page != 1) page--;
            else if(pressed_Button.Name == "bt_PagenationUser_Forward" && dg_Client.Items.Count != 0) page++;
            lb_PageUsers.Text = page.ToString();
            filler.FillDataGrid<Client>(dg_Client, $"clients/pages/{int.Parse(lb_PageUsers.Text)}/{CountElementPage}");
        }

        private void bt_PaginationAddress_Click(object sender, RoutedEventArgs e)
        {
            Button pressed_Button = sender as Button;
            int page = int.Parse(lb_Addresses.Text);
            if (pressed_Button.Name == "bt_PaginationAddressBack" && page != 1) page--;
            else if (pressed_Button.Name == "bt_PaginationAddressForwart" && dg_Address.Items.Count != 0) page++;
            lb_Addresses.Text = page.ToString();
            filler.FillDataGrid<Build_Shop_WPF.Models.Address>(dg_Address, $"addresses/pages/{int.Parse(lb_Addresses.Text)}/{CountElementPage}");
        }

        private void bt_PaginationOrder_Click(object sender, RoutedEventArgs e)
        {
            Button pressed_Button = sender as Button;
            PaginationDataGrid<Order>(dg_Order, pressed_Button, lb_Orders, "bt_PaginationOrderForward", "bt_PaginationOrderBack", "orders");
        }

        private void bt_PaginationConsumption_Click(object sender, RoutedEventArgs e)
        {
            Button pressed_Button = sender as Button;
            PaginationDataGrid<Consumption>(dg_consumption, pressed_Button, lb_Consumptions, "bt_PaginationConsumptionForward", "bt_PaginationConsumptionBack", "consumptions");
        }

        private void bt_PaginationContractor_Click(object sender, RoutedEventArgs e)
        {
            Button pressed_Button = sender as Button;
            PaginationDataGrid<Contractor>(dg_contractor, pressed_Button, lb_Contractors, "bt_PaginationContractorForward", "bt_PaginationContractorBack", "contractors");
        }


        private void bt_Insert_Address_Click(object sender, RoutedEventArgs e)
        {
            int id_client = (int)cb_Address_Client.SelectedValue;
            Build_Shop_WPF.Models.Address address = new Build_Shop_WPF.Models.Address(tb_Address_City.Text, tb_Address_Street.Text, tb_Address_Number_House.Text, tb_Address_Entrance.Text, tb_Address_NumberFlat.Text, tb_Address_Comment.Text, id_client);
            string json = JsonConvert.SerializeObject(address);
            manipulation.PostAsync("addresses", json);
        }

        private void bt_Update_Address_Click(object sender, RoutedEventArgs e)
        {
            List<Build_Shop_WPF.Models.Address> collection = dg_Address.SelectedItems.Cast<Build_Shop_WPF.Models.Address>().ToList();
            int id_client = (int)cb_Address_Client.SelectedValue;
            if (collection.Count > 0 && cb_Address_Client.SelectedIndex > -1)
            {
                Build_Shop_WPF.Models.Address element = collection[0];
                element.City = tb_Address_City.Text;
                element.Street = tb_Address_Street.Text;
                element.NumberBuild = tb_Address_Number_House.Text;
                element.Entrance = tb_Address_Entrance.Text;
                element.NumberFlat = tb_Address_NumberFlat.Text;
                element.Сomment = tb_Address_Comment.Text;
                element.СlientId = (int)cb_Address_Client.SelectedValue;
                string json = JsonConvert.SerializeObject(element);
                manipulation.PutAsync("addresses/" + element.IdAddress, json);
            }
        }

        private void bt_Delete_Address_Click(object sender, RoutedEventArgs e)
        {
            List<Build_Shop_WPF.Models.Address> collection = dg_Address.SelectedItems.Cast<Build_Shop_WPF.Models.Address>().ToList();
            if (collection != null)
            {
                List<int> ints = new List<int>();
                for (int i = 0; i < collection.Count; i++)
                {
                    ints.Add((int)collection[i].IdAddress);
                }
                string json = JsonConvert.SerializeObject(ints);
                manipulation.PutAsync("addresses/deleteAddresses", json);
            }
        }

        private void Insert_Order_Click(object sender, RoutedEventArgs e)
        {
            Order order = new Order(DateTime.Parse(dp_DateCreated.Text), TimeSpan.Parse(tb_TimeOrderCreated.Text), (int)cd_Order_Contractor.SelectedValue, (int)cd_Order_Project.SelectedValue);
            string json = JsonConvert.SerializeObject(order);
            manipulation.PostAsync("orders", json);
        }

        private void Update_Order_Click(object sender, RoutedEventArgs e)
        {
            List<Build_Shop_WPF.Models.Order> collection = dg_Address.SelectedItems.Cast<Build_Shop_WPF.Models.Order>().ToList();
            if(collection.Count > 0)
            {
                Order element = collection[0];
                element.DateCreate = DateTime.Parse(dp_DateCreated.Text);
                element.TimeCreate = TimeSpan.Parse(tb_TimeOrderCreated.Text);
                element.ContractorId = (int)cd_Order_Contractor.SelectedValue;
                element.ProjectId = (int)cd_Order_Project.SelectedValue;

                string json = JsonConvert.SerializeObject(element);
                manipulation.PutAsync("orders/" + element.IdOrder, json);
            }
        }

        private void Delete_Order_Click(object sender, RoutedEventArgs e)
        {
            List<Order> collection = dg_Order.SelectedItems.Cast<Order>().ToList();
            if (collection != null)
            {
                List<int> ints = new List<int>();
                for (int i = 0; i < collection.Count; i++)
                {
                    ints.Add((int)collection[i].IdOrder);
                }
                string json = JsonConvert.SerializeObject(ints);
                manipulation.PutAsync("orders/deleteOrders", json);
            }
        }

        private void Insert_Contractor_Click(object sender, RoutedEventArgs e)
        {
            Contractor contractor = new Contractor(tb_Contractor_TIN.Text, tb_Contractor_OGTN.Text, tb_FirstName.Text, tb_LastName.Text, tb_Contractor_NumberPhone.Text);
            string json = JsonConvert.SerializeObject(contractor);
            manipulation.PostAsync("contractors", json);
        }

        private void Update_Contractor_Click(object sender, RoutedEventArgs e)
        {
            List<Build_Shop_WPF.Models.Contractor> collection = dg_contractor.SelectedItems.Cast<Build_Shop_WPF.Models.Contractor>().ToList();
            if(collection.Count > 0)
            {
                Contractor element = collection[0];
                element.FirstName = tb_FirstName.Text;
                element.LastName = tb_LastName.Text;
                element.PhoneNumber = tb_NumberPhone.Text;
                element.Tin = tb_Contractor_TIN.Text;
                element.Ogrn = tb_Contractor_OGTN.Text;

                string json = JsonConvert.SerializeObject(element);
                manipulation.PutAsync("orders/" + element.IdContractor, json);
            }
        }

        private void Delete_Contractor_Click(object sender, RoutedEventArgs e)
        {
            List<Contractor> collection = dg_contractor.SelectedItems.Cast<Contractor>().ToList();
            if (collection != null)
            {
                List<int> ints = new List<int>();
                for (int i = 0; i < collection.Count; i++)
                {
                    ints.Add((int)collection[i].IdContractor);
                }
                string json = JsonConvert.SerializeObject(ints);
                manipulation.PutAsync("Contractors/deleteContractors", json);
            }
        }

        private void bt_Insert_Consumption_Click(object sender, RoutedEventArgs e)
        {
            Consumption consumption = new Consumption(tb_nameConsumption.Text, tb_descriptionConsumption.Text, decimal.Parse(tb_Amount.Text), DateTime.Parse(dp_ConsumptionDate.Text), DateTime.Parse(dp_ConsumptionDate.Text), (int)cb_Project.SelectedValue);
            string json = JsonConvert.SerializeObject(consumption);
            manipulation.PostAsync("contractors", json);
        }

        private void bt_Update_Consumption_Click(object sender, RoutedEventArgs e)
        {
            List<Consumption> collection = dg_consumption.SelectedItems.Cast<Consumption>().ToList();
            if(collection.Count > 0)
            {
                Consumption element = collection[0];
                element.Name = tb_nameConsumption.Text;
                element.Description = tb_descriptionConsumption.Text;
                element.Amount = decimal.Parse(tb_Amount.Text);
                element.DatePayment = DateTime.Parse(dp_ConsumptionDate.Text);
                element.TimePayment = DateTime.Parse(dp_ConsumptionDate.Text);
                element.ProjectId = (int)cb_Project.SelectedValue;

                string json = JsonConvert.SerializeObject(element);
                manipulation.PutAsync("Consumptions/" + element.IdConsumption, json);
            }
        }

        private void bt_Delete_Consumption_Click(object sender, RoutedEventArgs e)
        {
            List<Consumption> collection = dg_consumption.SelectedItems.Cast<Consumption>().ToList();
            if (collection != null)
            {
                List<int> ints = new List<int>();
                for (int i = 0; i < collection.Count; i++)
                {
                    ints.Add((int)collection[i].IdConsumption);
                }
                string json = JsonConvert.SerializeObject(ints);
                manipulation.PutAsync("Consumptions/deleteConsumptions", json);
            }
        }

        private void bt_MemoryAction_Click(object sender, RoutedEventArgs e)
        {
            Button pressedButton = sender as Button;
            if(pressedButton.Name == bt_RecoveryClient.Name)
            {
                manipulation.PutAsync("clients/restore", "");
            }
            else if(pressedButton.Name == bt_CleanClient.Name)
            {
                manipulation.DeleteAsync("clients/cleanMemory");
            }
            else if (pressedButton.Name == bt_RecoveryAddress.Name)
            {
                manipulation.PutAsync("addresses/restore", "");
            }
            else if (pressedButton.Name == bt_CleanAddress.Name)
            {
                manipulation.DeleteAsync("addresses/cleanMemory");
            }
            else if (pressedButton.Name == bt_CleanOrder.Name)
            {
                manipulation.DeleteAsync("orders/cleanMemory");
            }
            else if (pressedButton.Name == bt_RecoveryOrder.Name)
            {
                manipulation.PutAsync("orders/restore", "");
            }
            else if (pressedButton.Name == bt_CleanContractor.Name)
            {
                manipulation.DeleteAsync("contractors/cleanMemory");
            }
            else if (pressedButton.Name == bt_RecoveryContractor.Name)
            {
                manipulation.PutAsync("contractors/restore", "");
            }
            else if (pressedButton.Name == bt_go_To.Name)
            {
                new Dispatcher().Show();
                this.Close();
            }
        }

        private void cb_Address_Client_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox combo = sender as ComboBox;
            filler.FillComboBox<Client>(combo, "clients");
        }

        private void cb_ProjectLoaded(object sender, RoutedEventArgs e)
        {
            ComboBox combo = sender as ComboBox;
            filler.FillComboBox<Project>(combo, "projects");
        }

        private void cd_Order_Project_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox combo = sender as ComboBox;
            filler.FillComboBox<Project>(combo, "projects");
        }

        private void cd_Order_Contractor_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox combo = sender as ComboBox;
            filler.FillComboBox<Contractor>(combo, "contractors");
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            RegistryClass reg = new RegistryClass();
            reg.writeKeyValue(RegistryClass.nameProgram, RegistryClass.windowKey, "Manager");
            reg.writeKeyValue(RegistryClass.nameProgram, RegistryClass.pageValueKey, TC_Manager.SelectedIndex.ToString());
        }

        private void printWord_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Отсчёт распечатан!");
            new ElementFiller().GetFromApiToWord("projects");
        }

        private void printExcel_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Отсчёт распечатан!");
            new ElementFiller().GetFromApiToExcel("clients");
        }

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            try
            {
                // Используем регулярное выражение для проверки формата электронной почты
                // Данное выражение проверяет наличие '@' и '.', а также расширение домена
                // Подробнее про регулярные выражения смотрите здесь: https://docs.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
            }
            catch (RegexMatchTimeoutException)
            {
                // В случае ошибки таймаута при сопоставлении регулярного выражения
                return false;
            }
        }

        public bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return false;

            try
            {
                // Используем регулярное выражение для проверки формата номера телефона
                // Данное выражение проверяет, что строка состоит из 11 цифр, начиная с "8" или "+7"
                // Подробнее про регулярные выражения смотрите здесь: https://docs.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference
                string pattern = @"^\d{11}$";
                return Regex.IsMatch(phoneNumber, pattern);
            }
            catch (RegexMatchTimeoutException)
            {
                // В случае ошибки таймаута при сопоставлении регулярного выражения
                return false;
            }
        }

        private void exitAccount_Click(object sender, RoutedEventArgs e)
        {
            new RegistryClass().writeKeyValue(RegistryClass.nameProgram, RegistryClass.userKey, "");
            MainWindow goTo = new MainWindow();
            goTo.Show();
            this.Close();
        }

        private void dg_Client_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid element = sender as DataGrid; 
            Client data = element.SelectedItem as Client;
            if (data != null)
            {
                tb_FirstName.Text = data.FirstName;
            }
        }
    }
}
