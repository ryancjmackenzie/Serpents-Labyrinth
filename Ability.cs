
namespace SerpentsLabyrinth
{
    public enum AbilityType
    {
        MovePiece,
        ShrinkBoard,
        GrowBoard
    }

    public class Ability
    {
        public AbilityType Type { get; }

        public Ability(AbilityType type)
        {
            Type = type;
        }

        public string GetName()
        {
            return Type switch
            {
                AbilityType.MovePiece => "Move Piece",
                AbilityType.ShrinkBoard => "Shrink Board",
                AbilityType.GrowBoard => "Grow Board",
                _ => "Unknown"
            };
        }

        public string GetSymbol()
        {
            return Type switch
            {
                AbilityType.MovePiece => "M",
                AbilityType.ShrinkBoard => "S",
                AbilityType.GrowBoard => "G",
                _ => "?"
            };
        }
    }
}