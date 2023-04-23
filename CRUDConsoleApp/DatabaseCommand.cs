using System;
using System.Linq;
using CRUDConsoleApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using LinqKit;

namespace CRUDConsoleApp
{
    public class DatabaseCommand
    {
        private readonly MyDbContext _dbContext;
        private readonly ILogger<DatabaseCommand> _logger;

        public DatabaseCommand(IConfiguration configuration, ILogger<DatabaseCommand> logger)
        {
            _logger = logger;

            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            _dbContext = new MyDbContext(optionsBuilder.Options);
        }

        public IQueryable<Person> ListAll(int page, int count, string orderBy, string direction)
        {
            var query = _dbContext.People.Include(p => p.PersonType);

            if (!string.IsNullOrEmpty(orderBy))
            {
                query = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Person, PersonType>)(direction.Equals("asc", StringComparison.OrdinalIgnoreCase)
                    ? query.OrderByDynamic(orderBy, LinqExtensions.Order.Ascending)
                    : query.OrderByDynamic(orderBy, LinqExtensions.Order.Descending));
            }

            return query.Skip((page - 1) * count).Take(count);
        }

        public IQueryable<Person> ListByType(string typeName, int page, int count, string orderBy, string direction)
        {
            var query = _dbContext.People.Include(p => p.PersonType).Where(p => p.PersonType.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(orderBy))
            {
                query = direction.Equals("asc", StringComparison.OrdinalIgnoreCase)
                    ? query.OrderByDynamic(orderBy, LinqExtensions.Order.Ascending)
                    : query.OrderByDynamic(orderBy, LinqExtensions.Order.Descending);
            }

            return query.Skip((page - 1) * count).Take(count);
        }

        public void AddPerson(string firstName, string lastName, DateTime dateOfBirth, string personTypeName)
        {
            var personType = _dbContext.PersonTypes.FirstOrDefault(p => p.Name.Equals(personTypeName, StringComparison.OrdinalIgnoreCase));
            if (personType == null)
            {
                _logger.LogError($"Invalid person type '{personTypeName}'.");
                throw new ArgumentException("Invalid person type.");
            }

            var person = new Person
            {
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                PersonType = personType
            };

            _dbContext.People.Add(person);
            _dbContext.SaveChanges();
        }

        public void UpdatePerson(int personId, string firstName, string lastName, DateTime dateOfBirth, string personTypeName)
        {
            var person = _dbContext.People.Include(p => p.PersonType).FirstOrDefault(p => p.Id == personId);
            if (person == null)
            {
                _logger.LogError($"Person with ID '{personId}' not found.");
                throw new ArgumentException("Person not found.");
            }

            var personType = _dbContext.PersonTypes.FirstOrDefault(p => p.Name.Equals(personTypeName, StringComparison.OrdinalIgnoreCase));
            if (personType == null)
            {
                _logger.LogError($"Invalid person type '{personTypeName}'.");
                throw new ArgumentException("Invalid person type.");
            }

            person.FirstName = firstName;
            person.LastName = lastName;
            person.DateOfBirth = dateOfBirth;
            person.PersonType = personType;

            _dbContext.SaveChanges();
        }

        public void DeletePerson(int personId)
        {
            var person = _dbContext.People.FirstOrDefault(p => p.Id == personId);
            if (person == null)
            {
                _logger.LogError($"Person with ID '{personId}' not found.");
                throw new ArgumentException("Person not found.");
            }

            _dbContext.People.Remove(person);
            _dbContext.SaveChanges();
        }

        public void DeleteAll()
        {
            _dbContext.People.RemoveRange(_dbContext.People);
            _dbContext.SaveChanges();
        }

        internal void InitializeDatabase()
        {
            throw new NotImplementedException();
        }
    }
}
