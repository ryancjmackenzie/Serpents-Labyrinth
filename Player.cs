
using System.Collections.Generic;

namespace SerpentsLabyrinth
{
    public class Player
    {
        public string Name { get; }
        public int Position { get; private set; }
        public System.ConsoleColor Colour { get; }

        private readonly List<Ability> abilities =
            new List<Ability>();

        public Player(string name, System.ConsoleColor colour)
        {
            Name = name;
            Colour = colour;
            Position = 0;
        }

        public void SetPosition(int position)
        {
            Position = position;
        }

        public void AddAbility(Ability ability)
        {
            abilities.Add(ability);
        }

        public bool RemoveAbility(Ability ability)
        {
            return abilities.Remove(ability);
        }

        public List<Ability> GetAbilities()
        {
            return new List<Ability>(abilities);
        }

        public string GetToken()
        {
            return Name.Length > 0
                ? Name.Substring(0, 1).ToUpper()
                : "?";
        }
    }
}