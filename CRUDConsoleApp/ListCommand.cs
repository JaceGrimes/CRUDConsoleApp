using System;
using Microsoft.Extensions.Logging;

namespace CRUDConsoleApp
{
    public class ListCommand : AppCommand
    {
        private readonly DatabaseCommand _databaseCommand;
        private readonly ILogger<ListCommand> _logger;

        public ListCommand(DatabaseCommand databaseCommand, ILogger<ListCommand> logger)


        {
            _databaseCommand = databaseCommand;
            _logger = logger;
        }
        public override string Name => "LIST";
        public override string Usage => "LIST [type] [page] [count] [orderby] [direction]";
        public override string Description => "Lists records from the database.";

        public override void Execute()
        {
            string typeName = _arguments.Length > 0 ? _arguments[0] : "ALL";
            int page = _arguments.Length > 1 ? int.Parse(_arguments[1]) : 1;
            int count = _arguments.Length > 2 ? int.Parse(_arguments[2]) : 10;
            string orderBy = _arguments.Length > 3 ? _arguments[3] : null;
            string direction = _arguments.Length > 4 ? _arguments[4] : null;

            if (page < 1)
            {
                Console.WriteLine("Page number must be greater than or equal to 1.");
                return;
            }

            if (count < 1)
            {
                Console.WriteLine("Record count must be greater than or equal to 1.");
                return;
            }

            if (!string.IsNullOrEmpty(orderBy) && !orderBy.Equals("FirstName", StringComparison.OrdinalIgnoreCase) &&
                !orderBy.Equals("LastName", StringComparison.OrdinalIgnoreCase) &&
                !orderBy.Equals("DateOfBirth", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Invalid order by field '{orderBy}'.");
                return;
            }

            if (!string.IsNullOrEmpty(direction) && !direction.Equals("asc", StringComparison.OrdinalIgnoreCase) &&
                !direction.Equals("desc", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Invalid direction '{direction}'.");
                return;
            }

            try
            {
                var results = typeName.Equals("ALL", StringComparison.OrdinalIgnoreCase)
                    ? _databaseCommand.ListAll(page, count, orderBy, direction)
                    : _databaseCommand.ListByType(typeName, page, count, orderBy, direction);

                foreach (var result in results)
                {
                    Console.WriteLine($"{result.Id}\t{result.FirstName}\t{result.LastName}\t{result.DateOfBirth:d}\t{result.PersonType.Name}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while listing records from the database.");
            }
        }
    }
}
