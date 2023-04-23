
using CRUDConsoleApp;
using System;
using Microsoft.Extensions.Logging;

namespace CRUDConsoleApp
{
    public class UpdateCommand : AppCommand
    {
        private readonly DatabaseCommand _databaseCommand;
        private readonly ILogger<UpdateCommand> _logger;

        public UpdateCommand(DatabaseCommand databaseCommand, ILogger<UpdateCommand> logger)
        {
            _databaseCommand = databaseCommand;
            _logger = logger;
        }

        public override string Name => "UPDATE";
        public override string Usage => "UPDATE [PersonId] [FirstName] [LastName] [DateOfBirth] [PersonType]";
        public override string Description => "Updates a record in the database.";

        public override void Execute()
        {
            if (_arguments.Length != 5)
            {
                Console.WriteLine($"Invalid number of arguments. Usage: {Usage}");
                return;
            }

            int personId;
            if (!int.TryParse(_arguments[0], out personId))
            {
                Console.WriteLine($"Invalid person ID '{_arguments[0]}'.");
                return;
            }

            string firstName = _arguments[1];
            string lastName = _arguments[2];
            DateTime dateOfBirth;
            if (!DateTime.TryParse(_arguments[3], out dateOfBirth))
            {
                Console.WriteLine($"Invalid date of birth '{_arguments[3]}'.");
                return;
            }

            string personTypeName = _arguments[4];

            try
            {
                _databaseCommand.UpdatePerson(personId, firstName, lastName, dateOfBirth, personTypeName);
                Console.WriteLine("Person updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a person in the database.");
            }
        }
    }
}
