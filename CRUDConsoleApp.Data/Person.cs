namespace CRUDConsoleApp.Data
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int PersonTypeId { get; set; }
        public PersonType PersonType { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
