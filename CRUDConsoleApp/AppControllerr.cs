using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace CRUDConsoleApp
{
    public class AppController
    {
        private readonly IEnumerable<IAppCommand> _commands;
        private readonly ILogger<AppController> _logger;

        public AppController(IEnumerable<IAppCommand> commands, ILogger<AppController> logger)
        {
            _commands = commands;
            _logger = logger;
        }

        public void Run()
        {
            Console.WriteLine("Welcome to the CRUD console application!");

            while (true)
            {
                Console.Write("> ");
                string commandText = Console.ReadLine();
                if (string.IsNullOrEmpty(commandText))
                {
                    continue;
                }

                string[] parts = commandText.Split(" ");
                string commandName = parts[0];

                IAppCommand command = _commands.SingleOrDefault(c => c.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase));
                if (command == null)
                {
                    Console.WriteLine($"Invalid command '{commandName}'. Type HELP for a list of commands.");
                    continue;
                }

                string[] arguments = parts.Skip(1).ToArray();
                try
                {
                    command.Initialize(arguments);
                    command.Execute();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while executing the command.");
                }
            }
        }
    }
}
