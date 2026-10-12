
using System;

namespace SerpentsLabyrinth
{
    public class Game
    {
        private Grid grid;
        private Dice dice;
        private Player[] players;
        private bool gameOver;

        public Game()
        {
            int size = ChooseBoardSize();

            grid = new Grid(size);
            dice = new Dice();
            players = CreatePlayers();

            grid.SetPlayers(players);
            gameOver = false;
        }

        public void Start()
        {
            Console.Clear();
            Console.WriteLine("=== SERPENT'S LABYRINTH ===");

            grid.Display();

            while (!gameOver)
            {
                foreach (Player player in players)
                {
                    Console.WriteLine();
                    Console.WriteLine(
                        $"{player.Name}'s turn (Position: {player.Position})");

                    UseAbility(player);

                    Console.WriteLine("Press Enter to roll the dice...");
                    Console.ReadLine();

                    int roll = dice.Roll();

                    Console.WriteLine($"{player.Name} rolled a {roll}.");

                    int newPosition = player.Position + roll;

                    if (newPosition > grid.FinishPosition)
                    {
                        Console.WriteLine(
                            "You need an exact roll to finish.");
                    }
                    else
                    {
                        player.SetPosition(newPosition);

                        Console.WriteLine(
                            $"{player.Name} moved to position {player.Position}.");

                        if (grid.ApplyPiece(
                            player.Position, out int destination))
                        {
                            Cell? cell = grid.GetCell(player.Position);

                            if (cell?.Piece is Snake)
                            {
                                Console.WriteLine(
                                    $"Oh no! A snake sent {player.Name} down!");
                            }
                            else if (cell?.Piece is Ladder)
                            {
                                Console.WriteLine(
                                    $"Great! {player.Name} climbed a ladder!");
                            }

                            player.SetPosition(destination);

                            Console.WriteLine(
                                $"{player.Name} moved to position {destination}.");
                        }

                        // Collect an ability at the final landing position.
                        Cell? landingCell = grid.GetCell(player.Position);

                        if (landingCell?.Ability != null)
                        {
                            Ability ability = landingCell.Ability;

                            player.AddAbility(ability);
                            landingCell.Ability = null;

                            Console.WriteLine(
                                $"{player.Name} collected {ability.Type}!");
                        }
                    }

                    grid.Display();

                    if (CheckForWinner(player))
                    {
                        gameOver = true;
                        break;
                    }

                }
            }

            Console.WriteLine();
            Console.WriteLine("Game over!");
        }

        private void UseAbility(Player player)
        {
            if (player.Abilities.Count == 0)
                return;

            Console.WriteLine();
            Console.WriteLine("Your abilities:");

            for (int i = 0; i < player.Abilities.Count; i++)
            {
                Console.WriteLine(
                    $"{i + 1}. {player.Abilities[i].Type}");
            }

            Console.WriteLine("0. Save abilities and continue");
            Console.Write("Choose an ability to use: ");

            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int choice) ||
                choice < 1 ||
                choice > player.Abilities.Count)
            {
                return;
            }

            Ability ability = player.Abilities[choice - 1];
            bool used = false;

            switch (ability.Type)
            {
                case AbilityType.MovePiece:
                    used = grid.MoveRandomPiece();

                    if (used)
                    {
                        Console.WriteLine(
                            $"{player.Name} moved a snake or ladder!");
                    }
                    else
                    {
                        Console.WriteLine(
                            "No piece could be moved. Ability retained.");
                    }

                    break;

                case AbilityType.ShrinkBoard:
                    if (grid.GridSize <= 7)
                    {
                        Console.WriteLine(
                            "The board is already at its minimum size. (7x7). ");
                        break;
                    }

                    int smallerSize = grid.GridSize - 1;
                    int smallerFinish = smallerSize * smallerSize;

                    bool playersFit = true;

                    foreach (Player other in players)
                    {
                        if (other.Position > smallerFinish)
                        {
                            playersFit = false;
                            break;
                        }
                    }

                    if (!playersFit)
                    {
                        Console.WriteLine(
                            "The board cannot shrink because a player " +
                            "would be beyond the new finish.");
                        break;
                    }

                    used = grid.ResizeBoard(smallerSize);

                    if (used)
                    {
                        Console.WriteLine(
                            $"The board shrank to {smallerSize} x {smallerSize}!");
                    }

                    break;

                case AbilityType.GrowBoard:
                    if (grid.GridSize >= 14)
                    {
                        Console.WriteLine(
                            "The board is already at its maximum size. (14x14).");
                        break;
                    }

                    int largerSize = grid.GridSize + 1;

                    used = grid.ResizeBoard(largerSize);

                    if (used)
                    {
                        Console.WriteLine(
                            $"The board grew to {largerSize} x {largerSize}!");
                    }

                    break;
            }

            if (used)
            {
                player.RemoveAbility(ability);
                grid.Display();
            }
        }


        private int ChooseBoardSize()
        {
            while (true)
            {
                Console.WriteLine("Choose your board size:");
                Console.WriteLine("1. 7 x 7");
                Console.WriteLine("2. 8 x 8");
                Console.WriteLine("3. 9 x 9");
                Console.WriteLine("4. 10 x 10");
                Console.WriteLine("5. 11 x 11");
                Console.WriteLine("6. 12 x 12");
                Console.WriteLine("7. 13 x 13");
                Console.WriteLine("8. 14 x 14");
                Console.Write("Selection: ");

                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1": return 7;
                    case "2": return 8;
                    case "3": return 9;
                    case "4": return 10;
                    case "5": return 11;
                    case "6": return 12;
                    case "7": return 13;
                    case "8": return 14;

                    default:
                        Console.WriteLine("Invalid selection. Try again.");
                        break;
                }
            }
        }


        private Player[] CreatePlayers()
        {
            int playerCount;

            while (true)
            {
                Console.Write("How many players (2-4)? ");
                string? input = Console.ReadLine();

                if (int.TryParse(input, out playerCount) &&
                    playerCount >= 2 &&
                    playerCount <= 4)
                {
                    break;
                }

                Console.WriteLine("Enter a number between 2 and 4.");
            }

            Console.WriteLine();

            Player[] newPlayers = new Player[playerCount];

            ConsoleColor[] colours =
            {
                ConsoleColor.Cyan,
                ConsoleColor.Yellow,
                ConsoleColor.Magenta,
                ConsoleColor.Green
            };

            for (int i = 0; i < playerCount; i++)
            {
                Console.Write($"Enter the name for Player {i + 1}: ");
                string? name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name))
                    name = $"Player {i + 1}";

                newPlayers[i] = new Player(name, colours[i]);
                newPlayers[i].SetPosition(1);
            }

            return newPlayers;
        }

        private bool CheckForWinner(Player player)
        {
            if (player.Position != grid.FinishPosition)
                return false;

            Console.WriteLine();
            Console.WriteLine("================================");
            Console.WriteLine($"  {player.Name} WINS!");
            Console.WriteLine("  Serpent's Labyrinth completed!");
            Console.WriteLine("================================");

            return true;
        }

        public static void Main()
        {
            Game game = new Game();
            game.Start();
        }
    }
}
