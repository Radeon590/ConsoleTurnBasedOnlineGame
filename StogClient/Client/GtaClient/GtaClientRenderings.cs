using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Newtonsoft.Json;
using StogShared;
using StogShared.Entities.GameWorld;

namespace StogClient.Client
{
    internal partial class GtaClient
    {
        private async Task ShowUi()
        {
            Console.Clear();
            ResetAvailableCommandsBuilder();
            // Show data
            Console.WriteLine($"Username: {Username}");
            Console.WriteLine($"Coins: {Player.Coins}");
            Console.WriteLine($"HP: {WorldState.Players.Where(p => p.Username == Username).FirstOrDefault().Health}");
            RenderMap();
            Console.WriteLine();
            if (Player.Health > 0)
            {
                ShowLastAction();
                ShowClosestPlayers();
                ShowAvailableCommands();
                // Command
                await ReadCommand();
            }
        }

        private void RenderMap()
        {
            Console.WriteLine("\nMap");
            for (int y = -1; y < WorldConstants.MapBorderY + 1; y++)
            {
                for (int x = -1; x < WorldConstants.MapBorderX + 1; x++)
                {
                    if (x == -1 || x == WorldConstants.MapBorderX || y == -1 || y == WorldConstants.MapBorderY)
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        IEnumerable<Player> players = WorldState.Players.Where(p => p.Position == new Vector2(x, y));
                        if (players.Any())
                        {
                            if (players.Where(p => p.Username == Username).Any())
                            {
                                if (Player.Health <= 0)
                                {
                                    Console.Write("=");
                                }
                                else
                                {
                                    Console.Write("X");
                                }
                            }
                            else
                            {
                                if (players.Count() == 1 && players.First().Health == 0)
                                {
                                    Console.Write("_");
                                }
                                else
                                {
                                    Console.Write("O");
                                }
                            }
                        }
                        else
                        {
                            Console.Write(" ");
                        }
                    }
                }
                Console.WriteLine();
            }
        }

        private void ShowClosestPlayers()
        {
            Console.Write("player nearby: ");
            bool isFound = false;
            foreach (Player p in WorldState.Players)
            {
                if (p.Username != Username)
                {
                    if (Vector2.Distance(p.Position, Player.Position) <= 1)
                    {
                        isFound = true;
                        Console.Write($"{p.Username} ");

                        if (p.Health == 0)
                        {
                            Console.Write("(dead) ");
                        }
                        else
                        {
                            _availableCommandsBuilder.AddAttack(p.Username, 1);
                        }
                    }
                }
            }
            if (!isFound)
            {
                Console.WriteLine("Nobody");
            }
            else
            {
                Console.WriteLine();
            }
        }

        private void ShowLastAction()
        {
            if (Player.LastActions.Any())
            {
                return;
            }
            Console.WriteLine();
            Console.WriteLine("Last actions:");
            foreach (var lastAction in Player.LastActions)
            {
                Console.WriteLine($"\t{lastAction}");
            }
        }

        private void ShowAvailableCommands()
        {
            Console.WriteLine();
            Console.WriteLine("available commands:");
            var commands = _availableCommandsBuilder.Build();
            foreach (var command in commands) 
            {
                Console.WriteLine(command.CommandString);
            }
        }

        private void ShowCommandExecutionResult(string result)
        {
            Console.WriteLine();
            Console.WriteLine($"Command execution result: {result}");
        }
    }
}
