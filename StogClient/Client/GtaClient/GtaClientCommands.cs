using StogClient.WebApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using StogClient.Client.Commands;
using StogClient.Client.Commands.Command;

namespace StogClient.Client
{
    internal partial class GtaClient
    {
        private CommandsBuilder _availableCommandsBuilder;

        private void ResetAvailableCommandsBuilder()
        {
            _availableCommandsBuilder = new CommandsBuilder(this, GameServerData);
            if (WorldState.CurrentPlayerUsername == Username)
            {
                _availableCommandsBuilder.AddMove(new Vector2(-1, 0));
                _availableCommandsBuilder.AddMove(new Vector2(1, 0));
                _availableCommandsBuilder.AddMove(new Vector2(0, -1));
                _availableCommandsBuilder.AddMove(new Vector2(0, 1));
                _availableCommandsBuilder.AddHeal();
            }
        }

        private async Task ReadCommand()
        {
            Console.WriteLine();
            Console.WriteLine("enter command");
            string? commandString = Console.ReadLine();
            if (commandString != null) 
            {
                await ExecuteCommand(commandString);
            }
        }

        protected async Task ExecuteCommand(string commandString)
        {
            var command = _availableCommandsBuilder.Build().Where(c => c.CommandString == commandString).FirstOrDefault();
            if (command != null) 
            {
                await ExecuteCommand(command);
            }
            else 
            {
                Console.WriteLine("command not available");
            }
        }

        protected async Task ExecuteCommand(Command command)
        {
            var result = await command.Execute();
            ShowCommandExecutionResult(result);
        }
    }
}
