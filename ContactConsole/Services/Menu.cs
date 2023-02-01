using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ContactConsole.Models;
using System.Windows;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Xml.Linq;
using System.Reflection.Metadata;

namespace ContactConsole.Services
{
    internal class Menu
    {
        // DISPLAY MAIN MENU SERVICE

        public static void MainMenu()
        {
            FileService fs = new FileService();
            // bool for entering/exiting program loop; strings that hold display info
            bool programRunning = true;

            // NOTE: ideally, these strings might be changed to parameters for the DisplayMenu()-method,
            // while the actual strings are stored in a model/class of some sort. Maybe as fields in the
            // Menu class?
            string menu = " ~~~~~~ Main Menu ~~~~~~~ \n \n1 - view all contacts \n2 - search contacts \n3 - add a contacts \n4 - exit address book app  \n";
            string message = "Welcome to the address book console app! What would you like to do? \nMake your selection from the menu displayed above:";
            string defaultMessage = "Make your selection from the menu displayed above:";

            // main menu loop
            while (programRunning)
            {
                Console.Clear();
                Console.WriteLine($"{menu}\n{message}");
                string menuSelection = Console.ReadLine();

                switch (menuSelection)
                {
                    // "view all contacts"
                    case "1":
                        Console.Clear();
                        AllContactsMenu(ConvertJsonToList(fs.ReadFromFile()));
                        break;
                    // "search contacts"
                    case "2":
                        EditContact(SelectContact(SearchForContacts(ConvertJsonToList(fs.ReadFromFile()))));
                        break;
                    // "add a contact"
                    case "3":
                        AddContact(fs);
                        break;
                    // "exit address book app"
                    case "4":
                        Console.WriteLine("Thanks for using the contact book! Press 'enter' key to quit program...");
                        Console.ReadLine();
                        programRunning = false;
                        break;
                    default:
                        Console.WriteLine("Your selection is unavailable. Press the 'enter' key for new selection.");
                        Console.ReadLine();
                        break;

                }
            }
        }

        // AUXILARY MENU SERVICES

        // NOTE: ResetConsole() was mostly a test to see if i understood the 'ref' keyword.
        // Should probably not be used, though it saves a couple of lines of code

            // File Services
        public static void WriteFile(List<Contact> list)
        {   
            SetLastIndex(list);
            string json = JsonSerializer.Serialize(list);
            string filePath = $"{Directory.GetCurrentDirectory()}\\contacts.json";

            StreamWriter writer = new StreamWriter(filePath);
            writer.Write(json);
            writer.Close();
        }
        public static string ConvertListToJson(List<Contact> list)
        {
            SetLastIndex(list);
            string json = JsonSerializer.Serialize(list);

            return json;
        }

        public static string ReadFile(string filePath)
        {
            // NOTE: This method can be improved by using an overload taking two arguments,
            // where the second argument is the method to employ ('createFile()') if file does not exist. If ONE argument
            // is used, an error message would be logged ("file does not exist"), if TWO arguments
            // are used, the file to be read is created.
           
            bool fileExists = File.Exists(filePath);
            if (!fileExists)
            {
                // NOTE: check convention for using StreamWriter/StreamReader. 
                // Can brackets '()' and 'using' be employed and .Close() skipped??
                StreamWriter writer = new StreamWriter(filePath);
                writer.Write("[]");
                writer.Close();
            }

            StreamReader reader = new StreamReader(filePath);
            string json = reader.ReadToEnd();
            reader.Close();

            return json;

        }

            // Contact Services

        //Below method is redundant and should be deleted!
        public static List<Contact> UpdateContactList(string json)
        {
            List<Contact> contactList = new List<Contact>();

            if (json != "[]")
            {
                contactList = JsonSerializer.Deserialize<List<Contact>>(json);
                SetLastIndex(contactList);
            }

            return contactList;
        }
        public static List<Contact> ConvertJsonToList(string json) 
        {
            List<Contact> contactList = new List<Contact>();

            if (json != "[]")
            {
                contactList = JsonSerializer.Deserialize<List<Contact>>(json);
                // This resets the LastIndex property of all contacts in the list.
                // The property is needed to remove a contact from the contact book.
                SetLastIndex(contactList);
            }

            return contactList;
        }
        public static List<Contact> DisplayList(List<Contact> contactList)
        {
            int i = 1;

            foreach (Contact contact in contactList)
            {

                // NOTE: if possible, ListNr property should be removed as property of class Contact.
                // It is currently used to select a contact from a displayed list, but it should be 
                // possible to select by bracket [] notation so long as the list is available in the scope.
                //contact.ListNr = i;
                //Console.WriteLine($"{contact.ListNr} - Name: {contact.FirstName} {contact.LastName}\n");
                Console.WriteLine($" ~~~~~~ (( {i} )) ~~~~~~ ");
                DisplayContactForList(contact);
                Console.WriteLine("\n");
                i++;
            }

            return contactList;
        }
        public static void SetLastIndex(List<Contact> list)
        {
            int i = 0;
            foreach (Contact contact in list)
            {
                contact.LastIndex = i;
                i++;
            }
        }
        public static Contact SelectContact(List<Contact> list)
        {
            Contact selectedContact = new Contact();

            if (list != null)
            {
                DisplayList(list);
                bool programRunning = true;
                string instructions = $"Please input a number between 1 and {list.Count} to select a contact from the list displayed above.";

                while (programRunning)
                {
                    Console.WriteLine(instructions);
                    string userInput = Console.ReadLine();

                    int num = 0;
                    bool success = Int32.TryParse(userInput, out num);

                    if (success)
                    {
                        try
                        {
                            selectedContact = list[num - 1];
                            programRunning = false;
                        }
                        catch
                        {
                            instructions = $"Selection unavailable. Please input a number between 1 and {list.Count}";
                        }
                    }
                    else
                    {
                        instructions = $"Selection unavailable. Please input a number between 1 and {list.Count}";
                    }
                }
            } else
            {
                selectedContact = null;
                Console.WriteLine("Exiting to main menu!");
                Console.ReadLine();
            }
     
            return selectedContact;
        }
        public static Contact GetContactSelection(List<Contact> list)
        {
            
            bool stillSelecting = true;
            string instructions = "";
            Contact selectedContact = null;

            while (stillSelecting)
            {
                Console.WriteLine(instructions);
                string userInput = Console.ReadLine();

                // Parses user input to use as interger to get contact by bracket notation on list
                int num = 0;
                bool success = Int32.TryParse(userInput, out num);

                if (success)
                {
                    try
                    {
                        selectedContact = list[num - 1];
                        stillSelecting = false;
                    }
                    catch
                    {
                        if (list.Count > 1 )
                        {
                            instructions =  $"NUM RANGE: Selection out of range. Please type a number between 1 and {list.Count} and press 'enter' key to select a contacto. " +
                                            $"To return to the main menu, leave line blank and press 'enter' key.";
                        }
                        else
                        {
                            instructions =  $"NUM RANGE: Selection unavailable. Type 1 and press 'enter' key to select contact '{list[0].FirstName} {list[0].LastName}'." +
                                            $"To return to the main menu, leave line blank and press 'enter' key.";
                        }
                        
                    }
                }
                else if (userInput == "")
                {
                    stillSelecting = false;
                } 
                else
                {
                    instructions = $"NOT NUM: Selection unavailable. Please type a number between 1 and {list.Count} press 'enter' key to select a contact." +
                        $"To return to the main menu, leave terminal field blank and press 'enter' key.";
                }
            }

            return selectedContact;
        }
        public static void DeleteContact(Contact contactToDelete)
        {
            int index = contactToDelete.LastIndex;

            List<Contact> contactList = UpdateContactList(ReadFile($"{Directory.GetCurrentDirectory()}\\contacts.json"));
            contactList.RemoveAt(index);
            SetLastIndex(contactList);
            WriteFile(contactList);
        }
        public static void EditContact(Contact contactToEdit)
        {
            Console.Clear();
            bool stillEditing = true;

            if (contactToEdit == null)
            {
                stillEditing = false;
            }

            Contact contact = contactToEdit;          

            while (stillEditing) 
            {
                DisplayContactForEditing(contactToEdit);

                /*
                Console.WriteLine(" ~~~~~~~~ CONTACT ~~~~~~~~ \n");
                Console.WriteLine($"1 - First name: {contact.FirstName}");
                Console.WriteLine($"2 - Last name: {contact.LastName}\n");
                */

                Console.WriteLine(  "Select which field you wish to edit according to the enumeration shown above." +
                                    "\nIf you wish to return to the main menu, press the enter key. If you wish to delete " +
                                    "\n the contact, type DELETE CONTACT in capital letters.");

                string selection = Console.ReadLine();

                switch (selection)
                {
                    case "DELETE CONTACT":
                        DeleteContact(contact);
                        Console.WriteLine($"The contact '{contact.FirstName} {contact.LastName}' was removed from the contact book. Press enter key to return to main menu.");
                        Console.ReadLine();
                        stillEditing = false;
                        break;

                    case "1":
                        Console.WriteLine("First Name: ");
                        contact.FirstName = Console.ReadLine();
                        Console.Clear();
                        break; 
                    
                    case "2":
                        Console.WriteLine("Last Name: ");
                        contact.LastName = Console.ReadLine();
                        Console.Clear();
                        break;

                    case "3":
                        Console.WriteLine("Email: ");
                        contact.Email = Console.ReadLine();
                        Console.Clear();
                        break;

                    case "4":
                        Console.WriteLine("Phone Number: ");
                        contact.PhoneNumber = Console.ReadLine();
                        Console.Clear();
                        break;

                    case "5":
                        Console.WriteLine("Street Name: ");
                        contact.StreetName = Console.ReadLine();
                        Console.Clear();
                        break;

                    case "6":
                        Console.WriteLine("Street Number: ");
                        contact.StreetNumber = Console.ReadLine();
                        Console.Clear();
                        break;

                    case "7":
                        Console.WriteLine("City: ");
                        contact.City = Console.ReadLine();
                        Console.Clear();
                        break;

                    case "8":
                        Console.WriteLine("Postal Code: ");
                        contact.City = Console.ReadLine();
                        Console.Clear();
                        break;

                    case "": 
                        // get an updated list from file
                        List<Contact> list = GetListFromFile($"{Directory.GetCurrentDirectory()}\\contacts.json");
                        // get index of the contact currently editing
                        int index = contact.LastIndex;
                        // update list by removing at index
                        list.RemoveAt(index);
                        // update list by adding contact
                        list.Add(contact);
                        // write list to file
                        WriteFile(list);
                        stillEditing = false;
                        break;
                    default: 
                        Console.WriteLine();
                        break;
                }
            }
        }
        public static void AllContactsMenu(List<Contact> list) 
        {
            // HEADER
            Console.WriteLine("  ~~~~~~ CONTACTS ~~~~~~  \n");
            
            // INSTRUCTIONS
            if (list.Count > 1) 
            {
                
                DisplayList(list);
                Console.WriteLine(  $"Select a contact by its number and press 'enter' key. For instance: to select '{list[1].FirstName}', type 2 in " +
                                    $"terminal, etc. To return to the main menu, leave the line blank and press 'enter' key.");
                EditContact(GetContactSelection(list));
            } else if (list.Count == 1)
            {
                DisplayList(list);
                Console.WriteLine(  "To edit or delete the contact, type the number 1 on the terminal line and press the 'enter' key. To return " +
                                    "to the main menu, leave the terminal line blank and press the 'enter' key.");
                EditContact(GetContactSelection(list));
            } 
            else
            {
                Console.WriteLine("Your contact book is currently empty! Press 'enter' key to return to main menu.");
                Console.ReadLine();    
            }
        }
        public static List<Contact> GetListFromFile(string path)
        {
            return ConvertJsonToList(ReadFile(path));
        }
        public static void DisplayContactForList(Contact contact)
        {
            Contact newContact = contact;
            List<string> properties = GetPropertiesAsList();
            int i = 0;
            foreach (string property in properties)
            {
                // I got this from https://stackoverflow.com/questions/19276475/how-can-i-convert-string-value-to-object-property-name
                // and I don't understand exactly how it works under the hood!
                PropertyInfo pinfo = typeof(Contact).GetProperty(property);
                object propertyValue = pinfo.GetValue(newContact, null);

                if (propertyValue != "") 
                {
                    Console.WriteLine($"{AddSpacesToSentence(property)}: {propertyValue}");
                } 

                i++;
            }
        }
        public static void DisplayContactForEditing(Contact contact)
        {
            Contact newContact = contact;
            List<string> properties = GetPropertiesAsList();
            int i = 0;
            foreach (string property in properties)
            {
                // I got this from https://stackoverflow.com/questions/19276475/how-can-i-convert-string-value-to-object-property-name
                // and I don't understand exactly how it works under the hood!
                PropertyInfo pinfo = typeof(Contact).GetProperty(property);
                object propertyValue = pinfo.GetValue(newContact, null);

                    Console.WriteLine($" {i + 1} - {AddSpacesToSentence(property)}: {propertyValue}");

                i++;
            }
            Console.WriteLine("\n");
        }
        public static string AddSpacesToSentence(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }
        public static List<string> GetPropertiesAsList()
        {
            Type type = typeof(Contact);
            PropertyInfo[] list = type.GetProperties();
            List<string> listProperties = new List<string>();

            foreach (PropertyInfo property in list)
            {
                listProperties.Add(property.Name);
            }
            // removes ListNr and LastIndex from properties list
            listProperties.RemoveRange(0, 2);

            return listProperties;
        }
        public static List<Contact> SearchForContacts(List<Contact> contactList)
        {
            // variables
            List<Contact> searchMatches = new List<Contact>();
            bool stillSearching = true;


            while (stillSearching) 
            {
                Console.WriteLine("Input a search phrase. You may search by last name or first name. To return to main menu, type MAIN MENU and press enter.");

                string searchInput = Console.ReadLine();

                if (searchInput == "MAIN MENU") 
                {
                    stillSearching = false;
                    searchMatches = null;
                    if (searchMatches is null)
                    {
                        Console.WriteLine("Variable IS SET TO NULL!");
                    }
                    Console.ReadLine();
                    continue;
                } else if (searchInput == "")
                {
                    continue;
                }

                foreach (Contact contact in contactList)
                {
                    if (contact.FirstName.Contains(searchInput) || contact.LastName.Contains(searchInput))
                    {
                        searchMatches.Add(contact);
                    }
                }

                if (searchMatches.Count == 0) 
                {
                    Console.WriteLine("No matches found for search. Press 'enter' to initiate a new search.\n");
                    Console.ReadLine();
                } else
                {
                    stillSearching = false;
                }

            }
            
            // if searchMatches is null, return to main menu!
            return searchMatches;

        }
        public static void AddContact(FileService service)
        {
            Console.Clear();
            bool stillEditing = true;

            Contact newContact = new Contact();

            while (stillEditing)
            {
                DisplayContactForEditing(newContact);

                /*
                Console.WriteLine(" ~~~~~~~~ CONTACT ~~~~~~~~ \n");
                Console.WriteLine($"1 - First name: {contact.FirstName}");
                Console.WriteLine($"2 - Last name: {contact.LastName}\n");
                */

                Console.WriteLine(  "Select which field you wish to edit according to the enumeration shown above." +
                                    "\nType SAVE and press 'enter' key if you wish to save the added contact." +
                                    "\n Type CANCEL if you wish to return to the main menu without saving.");

                string selection = Console.ReadLine();

                switch (selection)
                {
                    case "CANCEL":
                        newContact = null;
                        stillEditing = false;
                        break;

                    case "1":
                        Console.WriteLine("First Name: ");
                        newContact.FirstName = Console.ReadLine();
                        Console.Clear();
                        break;

                    case "2":
                        Console.WriteLine("Last Name: ");
                        newContact.LastName = Console.ReadLine();
                        Console.Clear();
                        break;

                    case "3":
                        Console.WriteLine("Email: ");
                        newContact.Email = Console.ReadLine();
                        Console.Clear();
                        break;

                    case "4":
                        Console.WriteLine("Phone Number: ");
                        newContact.PhoneNumber = Console.ReadLine();
                        Console.Clear();
                        break;

                    case "5":
                        Console.WriteLine("Street Name: ");
                        newContact.StreetName = Console.ReadLine();
                        Console.Clear();
                        break;

                    case "6":
                        Console.WriteLine("Street Number: ");
                        newContact.StreetNumber = Console.ReadLine();
                        Console.Clear();
                        break;

                    case "7":
                        Console.WriteLine("City: ");
                        newContact.City = Console.ReadLine();
                        Console.Clear();
                        break;

                    case "8":
                        Console.WriteLine("Postal Code: ");
                        newContact.City = Console.ReadLine();
                        Console.Clear();
                        break;

                    case "SAVE":
                        // get an updated list from file
                        List<Contact> list = ConvertJsonToList(service.ReadFromFile()); 
                        // get index of the contact currently editing
                        list.Add(newContact);
                        // write list to file
                        service.WriteToFile(ConvertListToJson(list));
                        stillEditing = false;
                        break;
                    default:
                        Console.WriteLine();
                        break;
                }
            }
        }

    }


}
