
using System;

namespace SerpentsLabyrinth
{
    public class Ladder : Piece
    {
        public Ladder(string id, int bottom, int top)
            : base(id, bottom, top)
        {
            if (top <= bottom)
            {
                throw new ArgumentException(
                    "A ladder's top must be above its bottom."
                );
            }
        }

        public override int Apply()
        {
            return EndPosition;
        }
    }
}