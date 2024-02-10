using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Build_Shop_WPF
{
    public class RegistryClass
    {

        public static readonly string registryPath = @"Software\Microsoft\Windows\CurrentVersion\Run";

        public static readonly string  nameProgram = "BuildShop";
        public static readonly string windowKey = "BuildShopWindow";
        public static readonly string pageValueKey = "BuildShopWindowPage";
        public static readonly string userKey = "BuildShopUserJWT";

        public void checkSystemFiles()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath))
            { 
                if (key != null)
                {
                    // Проверяем наличие значения с именем каталога
                    if (key.GetValue(nameProgram) != null)
                    {
                        Console.WriteLine($"Каталог {nameProgram} найден в реестре");
                    }
                    else
                    {
                        Console.WriteLine($"Каталог {nameProgram} не найден в реестре");
                    }
                }
                else
                {
                    Console.WriteLine($"Ключ {registryPath} не найден в реестре");
                }
            }
        }

        public void writeKeyValue(string nameProgram, string keyWindow, string value)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey($"Software\\{nameProgram}");
            key.SetValue(keyWindow, value, RegistryValueKind.String);
            key.Close();
        }

        public string readKeyValue(string nameProgram, string keyWindow)
        {
            try
            {
                RegistryKey readKey = Registry.CurrentUser.OpenSubKey($"Software\\{nameProgram}");
                string myString = readKey.GetValue(keyWindow, "") as string;
                readKey.Close();
                return myString;
            }
            catch
            {
                return null;
            }
        }
    }
}
