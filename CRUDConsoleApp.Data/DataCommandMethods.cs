using System.Collections.Generic;
using System.Linq;

namespace CRUDConsoleApp.Data
{
    public class DataCommandMethods
    {
        private readonly MyDbContext _context;

        public DataCommandMethods(MyDbContext context)
        {
            _context = context;
        }

        public List<Person> GetAllPeople()
        {
            return _context.People.ToList();
        }

        public List<Person> GetPeopleByType(int typeId)
        {
            return _context.People.Where(p => p.PersonTypeId == typeId).ToList();
        }

        public void AddPerson(Person person)
        {
            _context.People.Add(person);
            _context.SaveChanges();
        }

        public void UpdatePerson(Person person)
        {
            _context.People.Update(person);
            _context.SaveChanges();
        }

        public void DeletePerson(Person person)
        {
            _context.People.Remove(person);
            _context.SaveChanges();
        }

        public void DeleteAllPeople()
        {
            _context.People.RemoveRange(_context.People);
            _context.SaveChanges();
        }
    }
}
