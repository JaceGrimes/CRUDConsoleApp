using System;
using Microsoft.Extensions.Logging;

namespace CRUDConsoleApp
{
    public class DeleteCommand : AppCommand
    {
        private readonly DatabaseCommand _databaseCommand;
        private readonly ILogger<DeleteCommand> _logger;

        public DeleteCommand(DatabaseCommand databaseCommand, ILogger<DeleteCommand> logger)
        {
            _databaseCommand = databaseCommand;
            _logger = logger;
        }

        public override string Name => "DELETE";
        public override string Usage => "DELETE [PersonId] or *";
        public override string Description => "Deletes a record from the database.";

        public override void Execute()
        {
            if (_arguments.Length != 1)
            {
                Console.WriteLine($"Invalid number of arguments. Usage: {Usage}");
                return;
            }

            if (_arguments[0] == "*")
            {
                try
                {
                    _databaseCommand.DeleteAll();
                    Console.WriteLine("All records deleted successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while deleting all records from the database.");
                }
            }
            else
            {
                int personId;
                if (!int.TryParse(_arguments[0], out personId))
                {
                    Console.WriteLine($"Invalid person ID '{_arguments[0]}'.");
                    return;
                }

                try
                {
                    _databaseCommand.DeletePerson(personId);
                    Console.WriteLine("Person deleted successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while deleting a person from the database.");
                }
            }
        }
    }
}
