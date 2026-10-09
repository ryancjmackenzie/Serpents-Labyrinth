
using System;

namespace SerpentsLabyrinth
{
    public class Dice
    {
        private readonly Random random = new Random();

        public int Roll()
        {
            return random.Next(1, 7);
        }
    }
}