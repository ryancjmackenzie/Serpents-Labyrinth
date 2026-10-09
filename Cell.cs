
using System.Collections.Generic;

namespace SerpentsLabyrinth
{
    public class Cell
    {
        public int Index { get; }

        public Piece? Piece { get; set; }
        public bool IsPieceStart { get; set; }
        public Ability? Ability { get; set; }

        public List<Player> Players { get; } = new List<Player>();

        public Cell(int index)
        {
            Index = index;
        }
    }
}