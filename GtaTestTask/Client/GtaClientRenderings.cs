﻿using GtaTestTask.WebApi.Entities;
using GtaTestTask.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GtaTestTask.Client
{
    internal partial class GtaClient
    {
        public void ShowUi()
        {
            // Update Data
            PlayerDbData playerDbData = GtaWebApi.Endpoints.ReadUser(Username);
            if (!_isHost)
            {
                WorldState = _server.ReadCurrentWorldState();
            }
            // Show data
            Console.WriteLine($"Username: {Username}");
            Console.WriteLine($"Coins: {playerDbData.Coins}");
            Console.WriteLine($"HP: {WorldState.Players.Where(p => p.Username == Username).FirstOrDefault().Health}");
            RenderMap();
            Console.WriteLine();
            ShowClosestPlayers();
            // Command
            ReadCommand();
        }

        private void RenderMap()
        {
            Console.WriteLine("\nMap");
            for (int y = -1; y < WorldBorder.Y + 1; y++)
            {
                for (int x = -1; x < WorldBorder.X + 1; x++)
                {
                    if (x == -1 || x == WorldBorder.X || y == -1 || y == WorldBorder.Y)
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
                                Player = players.First();
                                Console.Write("X");
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
    }
}
