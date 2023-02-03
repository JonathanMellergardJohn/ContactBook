using System.IO;
using System.Security.Cryptography.X509Certificates;
using ContactConsole.Services;
using ContactConsole.Models;
using System.Reflection;
using System.Text;

namespace ContactConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // $"{Directory.GetCurrentDirectory()}\\contacts.json"  <--- file path!
            Menu.MainMenu();
        }
    }
}
/*
TO-DO
    -re-write FileService.WriteToFile(). Should accept LIST as argument, not JSON/String!
    -re-write FileService.ReadFile(): Should return LIST, not JSON/String!
    -re-write FileService.ReadFile(): should sort List, alphabetically by last name, before returning List!
    -Remove/refactor methods:
        1. Remove AllContactsMenu(), replace with EditContact() and SelectContact()
        2. Remove WriteFile(). 
            Used in:
                -DeleteContact()
                -EditContact()
            Replace with: 
                -FileService.WriteToFile()
        3. Remove ReadFile()
            Used in: 
                DeleteContact(), 
                GetListFromFile()
        4. UpDateContactList() should be redundant??
            Used in:
                DeleteContact()
        COMPLETED:
            -Set up GitHub!
            -commit before starting re-factoring
            -Check contact book useability
            -FIX EditContact, input to Postal Code goes into City!
LIST OF ESSENTIAL METHODS
EditContact() 
    DisplayContactForEditing()
    DeleteContact()
    GetListFromFile()
    WriteFile()
SelectContact()
    DisplayList()
SearchForContacts()
ConvertJsonToList()
    SetLastIndex()
fs.ReadFromFile()
            
 */