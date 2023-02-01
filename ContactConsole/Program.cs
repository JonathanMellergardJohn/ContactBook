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
    -Set up GitHub!
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