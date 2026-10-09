
namespace SerpentsLabyrinth
{
    public abstract class Piece
    {
        public string Id { get; }
        public int StartPosition { get; private set; }
        public int EndPosition { get; private set; }

        protected Piece(string id, int startPosition, int endPosition)
        {
            Id = id;
            StartPosition = startPosition;
            EndPosition = endPosition;
        }

        public void Move(int newStart, int newEnd)
        {
            StartPosition = newStart;
            EndPosition = newEnd;
        }

        public abstract int Apply();
    }
}