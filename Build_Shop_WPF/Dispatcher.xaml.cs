using Build_Shop_WPF.Models;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Xml.Linq;

namespace Build_Shop_WPF
{
    /// <summary>
    /// Логика взаимодействия для Dispatcher.xaml
    /// </summary>
    public partial class Dispatcher : Window
    {
        RegistryClass pageChecker;
        ElementFiller filler;
        HttpManipulation manipulation;

        //Settings
        //api/objects/pages/{Number}/{Size}
        int CountElementPage = 2;

        public Dispatcher()
        {
            InitializeComponent();
            filler = new ElementFiller();
            pageChecker = new RegistryClass();
            string action = pageChecker.readKeyValue(RegistryClass.nameProgram, RegistryClass.windowKey);
            string actionPage = pageChecker.readKeyValue(RegistryClass.nameProgram, RegistryClass.pageValueKey);
            manipulation = new HttpManipulation();
            if (actionPage != null && actionPage != string.Empty && action == "Disp")
            {
                TC_Dispatcher.SelectedIndex = int.Parse(actionPage);
            }

        }

        private void datagrids_Loaded(object sender, RoutedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid.Name == dg_Tasks.Name)
            {
                filler.FillDataGrid<Build_Shop_WPF.Models.Task>(dataGrid, $"tasks/pages/1/{CountElementPage}");
                lb_Tasks.Text = "1";
            } else if (dataGrid.Name == dgStatusTask.Name)
            {
                filler.FillDataGrid<Status>(dataGrid, $"status/pages/1/{CountElementPage}");
                lb_StatusTask.Text = "1";
            } else if (dataGrid.Name == dg_Project.Name)
            {
                filler.FillDataGrid<Project>(dataGrid, $"projects/pages/1/{CountElementPage}");
                lb_Projects.Text = "1";
            }
            else if (dataGrid.Name == dg_CompositionEmployee.Name)
            {
                filler.FillDataGrid<CompositionEmployee>(dataGrid, $"CompositionEmployees/pages/1/{CountElementPage}");
                lb_CompositionEmployee.Text = "1";
            }
            else if (dataGrid.Name == dg_CompositionEquipment.Name)
            {
                filler.FillDataGrid<CompositionEquipment>(dataGrid, $"CompositionEquipments/pages/1/{CountElementPage}");
                lb_CompositionEquipments.Text = "1";
            }
            else if (dataGrid.Name == dg_Equipment.Name)
            {
                filler.FillDataGrid<Equipment>(dataGrid, $"Equipments/pages/1/{CountElementPage}");
                lb_Equipment.Text = "1";
            }
            else if (dataGrid.Name == dg_Warehouse.Name)
            {
                filler.FillDataGrid<Warehouse>(dataGrid, $"Warehouses/pages/1/{CountElementPage}");
                lb_Warehouse.Text = "1";
            } else if (dataGrid.Name == dg_Material.Name)
            {
                filler.FillDataGrid<Material>(dataGrid, $"Materials/pages/1/{CountElementPage}");
                lb_Materials.Text = "1";
            }
            else if (dataGrid.Name == dg_CompositionMaterial.Name)
            {
                filler.FillDataGrid<CompositionMaterial>(dataGrid, $"CompositionMaterials/pages/1/{CountElementPage}");
                lb_Materials.Text = "1";
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

        private void bt_PaginationTask_Click(object sender, RoutedEventArgs e)
        {
            Button pressed_Button = sender as Button;
            PaginationDataGrid<Build_Shop_WPF.Models.Task>(dg_Tasks, pressed_Button, lb_Tasks, "bt_PaginationTaskForward", "bt_PaginationTaskBack", "tasks");
        }

        private void cd_TaskProject_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            filler.FillComboBox<Project>(comboBox, "projects");
        }

        private void cd_TaskStatus_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            filler.FillComboBox<Status>(comboBox, "status");
        }

        private void Insert_Task_Click(object sender, RoutedEventArgs e)
        {
            Build_Shop_WPF.Models.Task element = new Build_Shop_WPF.Models.Task();
            element.Name = tb_TaskName.Text;
            element.Description = tb_TaskDescription.Text;
            element.DateCreate = DateTime.Parse(dp_DateCreatedProject.Text);
            element.TimeCreate = TimeSpan.Parse(tb_StartTask.Text);
            element.DateChange = DateTime.Parse(dp_DateChangeTask.Text);
            element.TimeChange = TimeSpan.Parse(tb_ChangeTask.Text);
            element.DateEnd = DateTime.Parse(dp_DateEndTask.Text);
            element.TimeEnd = TimeSpan.Parse(tb_EndTask.Text);
            element.ProjectId = (int)cd_TaskProject.SelectedValue;
            element.StatusId = (int)cd_TaskStatus.SelectedValue;

            string json = JsonConvert.SerializeObject(element);
            manipulation.PostAsync("tasks", json);
        }

        private void Update_Task_Click(object sender, RoutedEventArgs e)
        {
            List<Build_Shop_WPF.Models.Task> collection = dg_Tasks.SelectedItems.Cast<Models.Task>().ToList();
            if(collection.Count > 0)
            {
                Models.Task element = collection[0];

                element.Name = tb_TaskName.Text;
                element.Description = tb_TaskDescription.Text;
                element.DateCreate = DateTime.Parse(dp_DateCreatedProject.Text);
                element.TimeCreate = TimeSpan.Parse(tb_StartTask.Text);
                element.DateChange = DateTime.Parse(dp_DateChangeTask.Text);
                element.TimeChange = TimeSpan.Parse(tb_ChangeTask.Text);
                element.DateEnd = DateTime.Parse(dp_DateEndTask.Text);
                element.TimeEnd = TimeSpan.Parse(tb_EndTask.Text);
                element.ProjectId = (int)cd_TaskProject.SelectedValue;
                element.StatusId = (int)cd_TaskStatus.SelectedValue;

                string json = JsonConvert.SerializeObject(element);
                manipulation.PutAsync("tasks/"+element.IdTask, json);
            }
        }

        private void Delete_Task_Click(object sender, RoutedEventArgs e)
        {
            List<Build_Shop_WPF.Models.Task> collection = dg_Tasks.SelectedItems.Cast<Build_Shop_WPF.Models.Task>().ToList();
            if (collection != null)
            {
                List<int> ints = new List<int>();
                for (int i = 0; i < collection.Count; i++)
                {
                    ints.Add((int)collection[i].IdTask);
                }
                string json = JsonConvert.SerializeObject(ints);
                manipulation.PutAsync("tasks/deleteTasks", json);
            }
        }

        private void bt_PaginationStatusTask_Click(object sender, RoutedEventArgs e)
        {
            Button pressed_Button = sender as Button;
            PaginationDataGrid<Status>(dgStatusTask, pressed_Button, lb_StatusTask, "bt_PaginationStatusTaskForward", "bt_PaginationStatusTaskBack", "status");
        }

        private void Update_StatusChange_Click(object sender, RoutedEventArgs e)
        {
            List<Status> collection = dgStatusTask.SelectedItems.Cast<Status>().ToList();
            if(collection.Count > 0)
            {
                Status element = collection[0];
                element.LevelStatus = int.Parse(tb_LevelImportantStatus.Text);
                element.NameStatus = tb_NameStatus.Text;

                string json = JsonConvert.SerializeObject(element);
                manipulation.PutAsync("Status/"+element.IdStatus, json);
            }
        }

        private void Insert_StatusChange_Click(object sender, RoutedEventArgs e)
        {
            Status element = new Status(int.Parse(tb_LevelImportantStatus.Text), tb_NameStatus.Text);
            string json = JsonConvert.SerializeObject(element);
            manipulation.PostAsync("Status", json);
        }

        private void Delete_StatusChange_Click(object sender, RoutedEventArgs e)
        {
            List<Build_Shop_WPF.Models.Status> collection = dgStatusTask.SelectedItems.Cast<Build_Shop_WPF.Models.Status>().ToList();
            if (collection != null)
            {
                List<int> ints = new List<int>();
                for (int i = 0; i < collection.Count; i++)
                {
                    ints.Add((int)collection[i].IdStatus);
                }
                string json = JsonConvert.SerializeObject(ints);
                manipulation.PutAsync("status/deleteStatus", json);
            }
        }

        private void bt_PaginationProject_Click(object sender, RoutedEventArgs e)
        {
            Button pressed_Button = sender as Button;
            PaginationDataGrid<Project>(dg_Project, pressed_Button, lb_Projects, "bt_PaginationProjectForward", "bt_PaginationProjectBack", "projects");
        }

        private void Insert_Project_Click(object sender, RoutedEventArgs e)
        {
            Client client = cd_ClientProject.SelectedItem as Client;
            Project element = new Project(tb_NameProject.Text, DateTime.Parse(dp_DateCreatedProject.Text), TimeSpan.Parse(tb_TimeCreatedProject.Text), DateTime.Parse(dp_DateEndProject.Text), TimeSpan.Parse(tb_TimeEndProject.Text), (int)client.IdClient);
            string json = JsonConvert.SerializeObject(element);
            manipulation.PostAsync("projects", json);
        }

        private void Update_Project_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Project_Click(object sender, RoutedEventArgs e)
        {
            List<Build_Shop_WPF.Models.Project> collection = dg_Project.SelectedItems.Cast<Build_Shop_WPF.Models.Project>().ToList();
            if (collection != null)
            {
                List<int> ints = new List<int>();
                for (int i = 0; i < collection.Count; i++)
                {
                    ints.Add((int)collection[i].IdProject);
                }
                string json = JsonConvert.SerializeObject(ints);
                manipulation.PutAsync("Projects/deleteProjects", json);
            }
        }

        private void bt_PaginationCompositionEmployee_Click(object sender, RoutedEventArgs e)
        {
            Button pressed_Button = sender as Button;
            PaginationDataGrid<CompositionEmployee>(dg_CompositionEmployee, pressed_Button, lb_CompositionEmployee, "bt_PaginationCompositionEmployeeForward", "bt_PaginationCompositionEmployeeBack", "CompositionEmployees");
        }

        private void cd_Employee_CompositionEmployee_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            filler.FillComboBox<User>(comboBox, "users/page");
        }

        private void cd_Project_CompositionEmployee_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            filler.FillComboBox<Project>(comboBox, "projects");
        }

        private void bt_PaginationCompositionEquipments_Click(object sender, RoutedEventArgs e)
        {
            Button pressed_Button = sender as Button;
            PaginationDataGrid<CompositionEquipment>(dg_CompositionEquipment, pressed_Button, lb_CompositionEquipments, "bt_PaginationCompositionEquipmentForward", "bt_PaginationCompositionEquipmentBack", "CompositionEquipments");
        }

        private void cd_EquipmentCompositionEquipment_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            filler.FillComboBox<Equipment>(comboBox, "Equipments");
        }

        private void Insert_CompositionEmployee_Click(object sender, RoutedEventArgs e)
        {
            CompositionEmployee element = new CompositionEmployee((int)cd_Employee_CompositionEmployee.SelectedValue, (int)cd_Project_CompositionEmployee.SelectedValue);
            string json = JsonConvert.SerializeObject(element);
            manipulation.PostAsync("CompositionEmployees", json);
        }

        private void Update_CompositionEmployee_Click(object sender, RoutedEventArgs e)
        {
            List<CompositionEmployee> collection = dg_CompositionEmployee.SelectedItems as List<CompositionEmployee>;
            if(collection.Count > 0)
            {
                CompositionEmployee element = collection[0];
                element.UserId = (int)cd_Employee_CompositionEmployee.SelectedValue;
                element.ProjectId = (int)cd_Project_CompositionEmployee.SelectedValue;
                string json = JsonConvert.SerializeObject(element);
                manipulation.PutAsync("CompositionEmployees/"+element.IdCompositionEmployees, json);
            }
        }

        private void Delete_CompositionEmployee_Click(object sender, RoutedEventArgs e)
        {
            List<Build_Shop_WPF.Models.CompositionEmployee> collection = dg_CompositionEmployee.SelectedItems.Cast<Build_Shop_WPF.Models.CompositionEmployee>().ToList();
            if (collection != null)
            {
                List<int> ints = new List<int>();
                for (int i = 0; i < collection.Count; i++)
                {
                    ints.Add((int)collection[i].IdCompositionEmployees);
                }
                string json = JsonConvert.SerializeObject(ints);
                manipulation.PutAsync("CompositionEmployees/deleteCompositionEmployees", json);
            }
        }

        private void cd_ProjectCompositionEquipment_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            filler.FillComboBox<Project>(comboBox, "projects");
        }

        private void Insert_CompositionEquipment_Click(object sender, RoutedEventArgs e)
        {
            CompositionEquipment element = new CompositionEquipment((int)cd_EquipmentCompositionEquipment.SelectedValue, (int)cd_ProjectCompositionEquipment.SelectedValue);
            string json = JsonConvert.SerializeObject(element);
            manipulation.PostAsync("ProjectCompositionEquipments", json);
        }

        private void Update_CompositionEquipment_Click(object sender, RoutedEventArgs e)
        {
            List<CompositionEquipment> collection = dg_CompositionEquipment.SelectedItems as List<CompositionEquipment>;
            if (collection.Count > 0)
            {
                CompositionEquipment element = collection[0];
                element.EquipmentId = (int)cd_EquipmentCompositionEquipment.SelectedValue;
                element.ProjectId = (int)cd_ProjectCompositionEquipment.SelectedValue;
                string json = JsonConvert.SerializeObject(element);
                manipulation.PutAsync("CompositionEmployees/" + element.IdCompositionEquipment, json);
            }
        }

        private void Delete_CompositionEquipment_Click(object sender, RoutedEventArgs e)
        {
            List<Build_Shop_WPF.Models.CompositionEquipment> collection = dg_CompositionEquipment.SelectedItems.Cast<Build_Shop_WPF.Models.CompositionEquipment>().ToList();
            if (collection != null)
            {
                List<int> ints = new List<int>();
                for (int i = 0; i < collection.Count; i++)
                {
                    ints.Add((int)collection[i].IdCompositionEquipment);
                }
                string json = JsonConvert.SerializeObject(ints);
                manipulation.PutAsync("CompositionEquipments/deleteCompositionEquipment", json);
            }
        } 

        private void bt_PaginationEquipment_Click(object sender, RoutedEventArgs e)
        {
            Button pressed_Button = sender as Button;
            PaginationDataGrid<Equipment>(dg_Equipment, pressed_Button, lb_Equipment, "bt_PaginationEquipmentForward", "bt_PaginationEquipmentBack", "Equipments");
        }

        private void сb_warehouse_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            filler.FillComboBox<Warehouse>(comboBox, "Warehouses");
        }

        private void Insert_Equipment_Click(object sender, RoutedEventArgs e)
        {
            Equipment element = new Equipment(tb_NameEquipment.Text, int.Parse(tb_AmountEquipment.Text), (int)сb_warehouse.SelectedValue);
            string json = JsonConvert.SerializeObject(element);
            manipulation.PostAsync("Equipments", json);
        }

        private void Update_Equipment_Click(object sender, RoutedEventArgs e)
        {
            List<Equipment> collection = dg_Equipment.SelectedItems.Cast<Equipment>().ToList();
            if(collection.Count > 0)
            {
                Equipment element = collection[0];
                element.Name = tb_NameEquipment.Text;
                element.Count = int.Parse(tb_AmountEquipment.Text);
                element.WarehouseId = (int)сb_warehouse.SelectedValue;
                string json = JsonConvert.SerializeObject(element);
                manipulation.PostAsync("Equipments/"+element.IdEquipment, json);
            }
        }

        private void Delete_Equipment_Click(object sender, RoutedEventArgs e)
        {
            List<Build_Shop_WPF.Models.Equipment> collection = dg_Equipment.SelectedItems.Cast<Build_Shop_WPF.Models.Equipment>().ToList();
            if (collection != null)
            {
                List<int> ints = new List<int>();
                for (int i = 0; i < collection.Count; i++)
                {
                    ints.Add((int)collection[i].IdEquipment);
                }
                string json = JsonConvert.SerializeObject(ints);
                manipulation.PutAsync("Equipments/deleteEquipments", json);
            }
        }
    

        private void bt_PaginationWarehouse_Click(object sender, RoutedEventArgs e)
        {
            Button pressed_Button = sender as Button;
            PaginationDataGrid<Warehouse>(dg_Warehouse, pressed_Button, lb_Warehouse, "bt_PaginationWarehouseForward", "bt_PaginationWarehouseBack", "Warehouses");
        }

        private void Insert_Warehouse_Click(object sender, RoutedEventArgs e)
        {
            Warehouse element = new Warehouse(tb_CityWarehouse.Text, tb_StreetWarehouse.Text, tb_NumberHouse.Text);
            string json = JsonConvert.SerializeObject(element);
            manipulation.PostAsync("Warehouses", json);
        }

        private void Update_Warehouse_Click(object sender, RoutedEventArgs e)
        {
            List<Warehouse> collection = dg_Warehouse.SelectedItems.Cast<Warehouse>().ToList();
            if(collection.Count > 0)
            {
                Warehouse element = collection[0];
                element.City = tb_CityWarehouse.Text;
                element.Street = tb_StreetWarehouse.Text;
                element.NumberBuild = tb_NumberHouse.Text;
                string json = JsonConvert.SerializeObject(element);
                manipulation.PutAsync("Warehouses/"+element.IdWarehouse, json);
            }
        }

        private void Delete_Warehouse_Click(object sender, RoutedEventArgs e)
        {
            List<Build_Shop_WPF.Models.Warehouse> collection = dg_Warehouse.SelectedItems.Cast<Build_Shop_WPF.Models.Warehouse>().ToList();
            if (collection != null)
            {
                List<int> ints = new List<int>();
                for (int i = 0; i < collection.Count; i++)
                {
                    ints.Add((int)collection[i].IdWarehouse);
                }
                string json = JsonConvert.SerializeObject(ints);
                manipulation.PutAsync("Warehouses/deleteWarehouses", json);
            }
        }

        private void bt_PaginationMaterial_Click(object sender, RoutedEventArgs e)
        {
            Button pressed_Button = sender as Button;
            PaginationDataGrid<Material>(dg_Material, pressed_Button, lb_Materials, "bt_PaginationMaterialForward", "bt_PaginationMaterialBack", "materials");
        }

        private void cb_WirehouseMaterial_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            filler.FillComboBox<Warehouse>(comboBox, "Warehouses");
        }

        private void Insert_Material_Click(object sender, RoutedEventArgs e)
        {
            Material element = new Material(tb_NameMaterial.Text, int.Parse(tb_CountMaterial.Text), decimal.Parse(tb_PriceMaterial.Text), (int)cb_WirehouseMaterial.SelectedValue);
            string json = JsonConvert.SerializeObject(element);
            manipulation.PostAsync("Materials", json);
        }

        private void Update_Material_Click(object sender, RoutedEventArgs e)
        {
            List<Material> collection = dg_Material.SelectedItems.Cast<Material>().ToList();
            if(collection.Count == 0)
            {
                Material element = collection[0];
                element.Name = tb_NameMaterial.Text;
                element.Amount = int.Parse(tb_CountMaterial.Text);
                element.Price = decimal.Parse(tb_PriceMaterial.Text);
                element.WarehouseId = (int)cb_WirehouseMaterial.SelectedValue;
                string json = JsonConvert.SerializeObject(element);
                manipulation.PutAsync("Materials/"+element.IdMaterial, json);
            }
        }

        private void Delete_Material_Click(object sender, RoutedEventArgs e)
        {
            List<Build_Shop_WPF.Models.Material> collection = dg_Material.SelectedItems.Cast<Build_Shop_WPF.Models.Material>().ToList();
            if (collection != null)
            {
                List<int> ints = new List<int>();
                for (int i = 0; i < collection.Count; i++)
                {
                    ints.Add((int)collection[i].IdMaterial);
                }
                string json = JsonConvert.SerializeObject(ints);
                manipulation.PutAsync("materials/deleteMaterials", json);
            }
        }

        private void bt_PaginationCompositionMaterial_Click(object sender, RoutedEventArgs e)
        {
            Button pressed_Button = sender as Button;
            PaginationDataGrid<CompositionMaterial>(dg_CompositionMaterial, pressed_Button, lb_CompositionMaterial, "bt_PaginationCompositionMaterialForward", "bt_PaginationCompositionMaterialBack", "CompositionMaterials");
        }

        private void Insert_CompositionMaterial_Click(object sender, RoutedEventArgs e)
        {
            CompositionMaterial element = new CompositionMaterial();
            element.MaterialId = (int)cd_MaterialCompositionMaterial.SelectedValue;
            element.ProjectId = (int)cb_ProjectCompositionMaterial.SelectedValue;
            string json = JsonConvert.SerializeObject(element);
            manipulation.PostAsync("CompositionMaterial", json);
        }

        private void Update_CompositionMaterial_Click(object sender, RoutedEventArgs e)
        {
            List<Build_Shop_WPF.Models.CompositionMaterial> collection = dg_CompositionMaterial.SelectedItems.Cast<Build_Shop_WPF.Models.CompositionMaterial>().ToList();
            if(collection.Count > 0)
            {
                CompositionMaterial element = collection[0];
                element.MaterialId = (int)cd_MaterialCompositionMaterial.SelectedValue;
                element.ProjectId = (int)cb_ProjectCompositionMaterial.SelectedValue;
                string json = JsonConvert.SerializeObject(element);
                manipulation.PutAsync("CompositionMaterial/"+element.IdCompositionMaterial, json);
            }
        }

        private void Delete_CompositionMaterial_Click(object sender, RoutedEventArgs e)
        {
            List<Build_Shop_WPF.Models.CompositionMaterial> collection = dg_CompositionMaterial.SelectedItems.Cast<Build_Shop_WPF.Models.CompositionMaterial>().ToList();
            if (collection != null)
            {
                List<int> ints = new List<int>();
                for (int i = 0; i < collection.Count; i++)
                {
                    ints.Add((int)collection[i].IdCompositionMaterial);
                }
                string json = JsonConvert.SerializeObject(ints);
                manipulation.PutAsync("CompositionMaterials/deleteCompositionMaterials", json);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            RegistryClass reg = new RegistryClass();
            reg.writeKeyValue(RegistryClass.nameProgram, RegistryClass.windowKey, "Disp");
            reg.writeKeyValue(RegistryClass.nameProgram, RegistryClass.pageValueKey, TC_Dispatcher.SelectedIndex.ToString());
        }

        private void bt_ManagerWindow_Click(object sender, RoutedEventArgs e)
        {
            new Manager().Show();
            this.Close();
        }

        private void cd_ClientProject_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            filler.FillComboBox<Client>(comboBox, "clients");
        }

        private void cd_MaterialCompositionMaterial_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            filler.FillComboBox<Material>(comboBox, "materials");
        }

        private void cb_ProjectCompositionMaterial_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            filler.FillComboBox<Project>(comboBox, "projects");
        }
    }
}
