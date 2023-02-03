using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ContactConsole.Models
{
    public class Contact
    {
        // NOTE: how would one create an object in an object here?
        // for instance, if one wanted to group some of the properties
        // together (name, address)?
        public int ListNr { get; set; }
        public int LastIndex { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        // NOTE: I was considering storing PhoneNumber as int, but opted for string as no calculations  
        // can be done on them and there is no relationship between phonenumbers that are close.
        public string PhoneNumber { get; set; } = string.Empty;

        public string StreetName { get; set; } = string.Empty;
        // NOTE: There is a case to make StreetNumber type int, but I opted for string. 
        public string StreetNumber { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        //NOTE: // NOTE: There is a case to make PostalCode type int, but I opted for string. 
        public string PostalCode { get; set; } = string.Empty;
    }
}
