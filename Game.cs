
using System;
using System.Collections.Generic;

namespace SerpentsLabyrinth
{
    public class Game
    {
        private Grid? grid;
        private readonly Dice dice = new Dice();
        private readonly List<Player> players = new List<Player>();

        public void Start()
        {
            AddPlayers();
            CreateGrid();
        }

        private void AddPlayers()
        {
            Console.WriteLine("--- Setup Players ---");

            int playerCount;

            do
            {
                Console.Write("Enter number of players (2-4): ");

                if (!int.TryParse(Console.ReadLine(), out playerCount))
                {
                    playerCount = 0;
                }
            }
            while (playerCount < 2 || playerCount > 4);

            ConsoleColor[] colours =
            {
                ConsoleColor.Cyan,
                ConsoleColor.Magenta,
                ConsoleColor.White,
                ConsoleColor.DarkYellow
            };

            for (int i = 0; i < playerCount; i++)
            {
                string name;

                do
                {
                    Console.Write($"Enter player {i + 1} name: ");
                    name = Console.ReadLine()?.Trim() ?? "";
                }
                while (string.IsNullOrWhiteSpace(name));

                players.Add(new Player(name, colours[i]));
            }
        }

        private void CreateGrid()
        {
            Console.WriteLine("\nWelcome to Serpent's Labyrinth!");

            Console.WriteLine("1. 9x9");
            Console.WriteLine("2. 10x10");
            Console.WriteLine("3. 11x11");
            Console.WriteLine("4. 12x12");

            int option;

            do
            {
                Console.Write("Choose a grid size: ");

                if (!int.TryParse(Console.ReadLine(), out option))
                {
                    option = 0;
                }
            }
            while (option < 1 || option > 4);

            int size = option + 8;

            grid = new Grid(size);
            grid.SetPlayers(players.ToArray());

            Console.WriteLine("\nGrid initialized successfully!");
            grid.Display();
        }

        public static void Main()
        {
            Game game = new Game();
            game.Start();
        }
    }
}