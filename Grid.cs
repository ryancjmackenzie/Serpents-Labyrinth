
using System;
using System.Collections.Generic;

namespace SerpentsLabyrinth
{
    public class Grid
    {
        private int gridSize;
        private List<Cell> cells = new List<Cell>();
        private List<Piece> pieces = new List<Piece>();
        private Player[] players = new Player[4];
        private Random random = new Random();

        public int GridSize => gridSize;
        public int FinishPosition => gridSize * gridSize;

        public Grid(int size)
        {
            if (size < 2)
                throw new ArgumentException("Grid size must be at least 2.");

            gridSize = size;

            CreateCells();
            GenerateObjects();
        }

        private void CreateCells()
        {
            cells.Clear();

            for (int i = 1; i <= FinishPosition; i++)
                cells.Add(new Cell(i));
        }

        private void GenerateObjects()
        {
            int targetCells = (int)(FinishPosition * 0.10);
            int targetPieces = Math.Max(2, targetCells / 3);

            int snakeCount = targetPieces / 2;
            int ladderCount = targetPieces - snakeCount;

            // Generate a planned mix of snakes and ladders.
            for (int i = 1; i <= snakeCount; i++)
                TryPlacePiece(true, $"S{i}");

            for (int i = 1; i <= ladderCount; i++)
                TryPlacePiece(false, $"L{i}");

            // Fill remaining object spaces with abilities.
            int occupiedCells = 0;

            foreach (Cell cell in cells)
            {
                if (cell.Piece != null || cell.Ability != null)
                    occupiedCells++;
            }

            while (occupiedCells < targetCells)
            {
                int position = random.Next(2, FinishPosition);
                Cell cell = cells[position - 1];

                if (cell.Piece != null || cell.Ability != null)
                    continue;

                AbilityType type = (AbilityType)random.Next(3);
                cell.Ability = new Ability(type);
                occupiedCells++;
            }
        }

        private bool TryPlacePiece(bool createSnake, string id)
        {
            int minimumPieceLength = gridSize;
            int minimumStartSpacing = gridSize;

            for (int attempt = 0; attempt < 500; attempt++)
            {
                int first = random.Next(2, FinishPosition);
                int second = random.Next(2, FinishPosition);

                if (first == second)
                    continue;

                // Ensure the snake or ladder is long enough.
                if (Math.Abs(first - second) < minimumPieceLength)
                    continue;

                Cell firstCell = cells[first - 1];
                Cell secondCell = cells[second - 1];

                if (firstCell.Piece != null || firstCell.Ability != null)
                    continue;

                if (secondCell.Piece != null || secondCell.Ability != null)
                    continue;

                // Snakes start at their heads; ladders at their bottoms.
                int pieceStart = createSnake
                    ? Math.Max(first, second)
                    : Math.Min(first, second);

                bool tooClose = false;

                foreach (Piece existingPiece in pieces)
                {
                    if (Math.Abs(
                        existingPiece.StartPosition - pieceStart
                    ) < minimumStartSpacing)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (tooClose)
                    continue;

                Piece piece;

                if (createSnake)
                {
                    int head = Math.Max(first, second);
                    int tail = Math.Min(first, second);

                    piece = new Snake(id, head, tail);

                    cells[head - 1].IsPieceStart = true;
                    cells[head - 1].Piece = piece;
                    cells[tail - 1].Piece = piece;
                }
                else
                {
                    int bottom = Math.Min(first, second);
                    int top = Math.Max(first, second);

                    piece = new Ladder(id, bottom, top);

                    cells[bottom - 1].IsPieceStart = true;
                    cells[bottom - 1].Piece = piece;
                    cells[top - 1].Piece = piece;
                }

                pieces.Add(piece);
                return true;
            }

            return false;
        }

        public void SetPlayers(Player[] newPlayers)
        {
            if (newPlayers.Length > players.Length)
                throw new ArgumentException(
                    "A maximum of four players is supported."
                );

            players = newPlayers;
        }

        public Cell? GetCell(int position)
        {
            if (position < 1 || position > FinishPosition)
                return null;

            return cells[position - 1];
        }

        public List<Piece> GetPieces()
        {
            return new List<Piece>(pieces);
        }

        
        public void Display()
        {
            Console.WriteLine();

            const int cellWidth = 5;

            for (int row = gridSize - 1; row >= 0; row--)
            {
                Console.Write("    +");

                for (int column = 0; column < gridSize; column++)
                    Console.Write(new string('-', cellWidth) + "+");

                Console.WriteLine();
                Console.Write($"{row + 1,3} |");

                for (int column = 0; column < gridSize; column++)
                {
                    int position;

                    // Alternate direction on each row.
                    if (row % 2 == 0)
                        position = row * gridSize + column + 1;
                    else
                        position = row * gridSize + gridSize - column;

                    DisplayCell(cells[position - 1]);
                }

                Console.WriteLine();
            }

            // Bottom border.
            Console.Write("    +");

            for (int column = 0; column < gridSize; column++)
                Console.Write(new string('-', cellWidth) + "+");

            Console.WriteLine();

            // X-axis labels: six characters per column,

            // X-axis labels centred underneath each cell.
            Console.Write("      ");

            for (int column = 1; column <= gridSize; column++)
            {
                string label = column.ToString();

                int leftPadding = (cellWidth - label.Length) / 2;
                int rightPadding = cellWidth - label.Length - leftPadding;

                Console.Write(new string(' ', leftPadding));
                Console.Write(label);
                Console.Write(new string(' ', rightPadding));
                Console.Write(' ');
            }

            Console.WriteLine();
            Console.WriteLine();

            Console.ResetColor();
        }

        private void DisplayCell(Cell cell)
        {
            string label = cell.Index.ToString();
            ConsoleColor colour = ConsoleColor.Gray;

            if (cell.Piece != null)
            {
                Piece piece = cell.Piece;

                if (piece is Snake)
                {
                    colour = ConsoleColor.Red;
                    label = cell.IsPieceStart
                        ? piece.Id
                        : piece.Id + "*";
                }
                else if (piece is Ladder)
                {
                    colour = ConsoleColor.Green;
                    label = cell.IsPieceStart
                        ? piece.Id
                        : piece.Id + "*";
                }
            }
            else if (cell.Ability != null)
            {
                label = cell.Ability.GetSymbol();

                colour = cell.Ability.Type switch
                {
                    AbilityType.MovePiece => ConsoleColor.Blue,
                    AbilityType.ShrinkBoard => ConsoleColor.Yellow,
                    AbilityType.GrowBoard => ConsoleColor.Magenta,
                    _ => ConsoleColor.Gray
                };
            }

            // Display a player token if a player occupies this cell.
            Player? player = null;

            foreach (Player currentPlayer in players)
            {
                if (currentPlayer != null &&
                    currentPlayer.Position == cell.Index)
                {
                    player = currentPlayer;
                    break;
                }
            }

            if (player != null)
            {
                label = player.GetToken();
                colour = player.Colour;
            }

            Console.ForegroundColor = colour;
            Console.Write($"{label,4} ");
            Console.ResetColor();
            Console.Write("|");
        }
    }
}