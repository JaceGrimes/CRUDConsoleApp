
using CRUDConsoleApp;
using System;
using Microsoft.Extensions.Logging;

namespace CRUDConsoleApp
{
    public class AddCommand : AppCommand
    {
        private readonly DatabaseCommand _databaseCommand;
        private readonly ILogger<AddCommand> _logger;

        public AddCommand(DatabaseCommand databaseCommand, ILogger<AddCommand> logger)
        {
            _databaseCommand = databaseCommand;
            _logger = logger;
        }

        public override string Name => "ADD";
        public override string Usage => "ADD [FirstName] [LastName] [DateOfBirth] [PersonType]";
        public override string Description => "Adds a record to the database.";

        public override void Execute()
        {
            if (_arguments.Length != 4)
            {
                Console.WriteLine($"Invalid number of arguments. Usage: {Usage}");
                return;
            }

            string firstName = _arguments[0];
            string lastName = _arguments[1];
            DateTime dateOfBirth;
            if (!DateTime.TryParse(_arguments[2], out dateOfBirth))
            {
                Console.WriteLine($"Invalid date of birth '{_arguments[2]}'.");
                return;
            }

            string personTypeName = _arguments[3];

            try
            {
                _databaseCommand.AddPerson(firstName, lastName, dateOfBirth, personTypeName);
                Console.WriteLine("Person added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a person to the database.");
            }
        }
    }
}