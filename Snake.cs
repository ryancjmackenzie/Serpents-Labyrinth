
using System;

namespace SerpentsLabyrinth
{
    public class Snake : Piece
    {
        public Snake(string id, int head, int tail)
            : base(id, head, tail)
        {
            if (tail >= head)
            {
                throw new ArgumentException(
                    "A snake's tail must be below its head."
                );
            }
        }

        public override int Apply()
        {
            return EndPosition;
        }
    }
}