using ContactConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ContactConsole.Services
{
    public static class FileService
    {
        //PROPERTIES
        public static string FilePath { get; set; } = $"{Directory.GetCurrentDirectory()}\\contacts.json";

        //METHODS
        public static List<Contact> ReadFromFile()
        {
            bool fileExists = File.Exists(FilePath);
            if (!fileExists)
            {
                StreamWriter writer = new StreamWriter(FilePath);
                writer.Write("[]");
                writer.Close();

            }

            StreamReader reader = new StreamReader(FilePath);
            string json = reader.ReadToEnd();
            reader.Close();
            List<Contact> contactList = JsonSerializer.Deserialize<List<Contact>>(json);

            return contactList;
        }

        public static void WriteToFile(List<Contact> contactList)
        {
            //sets .LastIndex property for all Contacts. The .LastIndex property is used when editing and deleting file!
            int i = 0;
            foreach (Contact contact in contactList)
            {
                contact.LastIndex = i;
                i++;
            }
            string json = JsonSerializer.Serialize(contactList);
            StreamWriter writer = new StreamWriter(FilePath);
            writer.Write(json);
            writer.Close();
        }

        public static void SetFile(string directory, string fileName ) 
        {
            FilePath = $"{directory}\\{fileName}";
        }
    }
}
