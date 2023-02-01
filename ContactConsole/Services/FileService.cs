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
    internal class FileService
    {
        //PROPERTIES
        public string FilePath { get; set; } = $"{Directory.GetCurrentDirectory()}\\contacts.json";

        //METHODS
        public string ReadFromFile()
        {
            // NOTE: This method can be improved by using an overload taking two arguments,
            // where the second argument is the method to employ ('createFile()') if file does not exist. If ONE argument
            // is used, an error message would be logged ("file does not exist"), if TWO arguments
            // are used, the file to be read is created.

            bool fileExists = File.Exists(FilePath);
            if (!fileExists)
            {
                // NOTE: check convention for using StreamWriter/StreamReader. 
                // Can brackets '()' and 'using' be employed and .Close() skipped??
                StreamWriter writer = new StreamWriter(FilePath);
                writer.Write("[]");
                writer.Close();
            }

            StreamReader reader = new StreamReader(FilePath);
            string json = reader.ReadToEnd();
            reader.Close();

            return json;

        }

        public void WriteToFile(string json)
        {
            StreamWriter writer = new StreamWriter(this.FilePath);
            writer.Write(json);
            writer.Close();
        }

        public void SetFile(string directory, string fileName ) 
        {
            this.FilePath = $"{directory}\\{fileName}";
        }
    }
}
