

namespace Advance
{
    /// <summary>
    /// Represents an army of pieces in the game.
    /// </summary>
    public class Army
    {
        /// <summary>
        /// Gets the collection of pieces in the army.
        /// </summary>
        public IEnumerable<Piece> Pieces
        {
            get
            {
                return pieces;
            }
        }

        /// <summary>
        /// Adds a piece to the army.
        /// </summary>
        /// <param name="piece">The piece to add.</param>
        public void AddPiece(Piece defected)
        {
            pieces.Add(defected);
        }

        /// <summary>
        /// Removes a piece from the army.
        /// </summary>
        /// <param name="piece">The piece to remove.</param>
        public void RemovePiece(Piece defected)
        {
            pieces.Remove(defected);
        }


        private List<Piece> pieces = new List<Piece>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Army"/> class.
        /// </summary>
        /// <param name="player">The player that owns the army.</param>
        /// <param name="board">The game board.</param>
        public Army(Player player, Board board)
        {
            Player = player;
            Board = board;

            int baseRow = player.Colour == Colour.Black ? 0 : Board.Size - 1;
            int direction = player.Direction;

        }

        /// <summary>
        /// Gets the player that owns the army.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the game board.
        /// </summary>
        public Board Board { get; }

        /// <summary>
        /// Removes all pieces from the army and clears the board.
        /// </summary>
        public void RemoveAllPieces()
        {
            foreach (var currentPiece in pieces)
            {
                if (currentPiece.OnBoard) currentPiece.LeaveBoard();
            }

            pieces.Clear();
        }

        /// <summary>
        /// Recruits a new piece into the army and places it on the initial square.
        /// </summary>
        /// <param name="icon">The icon representing the piece.</param>
        /// <param name="initialSquare">The initial square where the piece will be placed.</param>
        public void Recruit(char icon, Square? initialSquare)
        {
            if (initialSquare == null)
                throw new ArgumentException("initialSquare must not be null");

            var symbol = Char.ToLower(icon);
            Piece? newPiece = null;

            switch (symbol)
            {
                case 'z':
                    newPiece = new Zombie(Player, initialSquare);
                    break;
                case 'b':
                    newPiece = new Builder(Player, initialSquare);
                    break;
                case 'm':
                    newPiece = new Miner(Player, initialSquare);
                    break;
                case 'j':
                    newPiece = new Jester(Player, initialSquare);
                    break;
                case 's':
                    newPiece = new Sentinel(Player, initialSquare);
                    break;
                case 'c':
                    newPiece = new Catapult(Player, initialSquare);
                    break;
                case 'd':
                    newPiece = new Dragon(Player, initialSquare);
                    break;
                case 'g':
                    newPiece = new General(Player, initialSquare);
                    break;
                case '#':
                    newPiece = new Wall(null, initialSquare);
                    break;
                default:
                    throw new ArgumentException("Unrecognised icon");
            }

            pieces.Add(newPiece);
        }
    }
}