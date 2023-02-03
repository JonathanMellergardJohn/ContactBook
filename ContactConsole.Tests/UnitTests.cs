using ContactConsole.Models;
using ContactConsole.Services;


namespace ContactConsole.Tests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void Should_Add_Contact_To_File()
        {
            // arrange 

            List<Contact> oldList = FileService.ReadFromFile();
            int oldListCount = oldList.Count;
            Contact newContact = new Contact();
            newContact.FirstName = "Thenue";
            newContact.LastName = "kontaktoad";

            // act

            oldList.Add(newContact);
            FileService.WriteToFile(oldList);
            List<Contact> newList = FileService.ReadFromFile();

            // assert

            Assert.AreEqual(oldListCount + 1, newList.Count);
        }
    }
}