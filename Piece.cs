namespace Advance
{
    /// <summary>
    /// Represents a game piece, which can be moved and perform attacks.
    /// </summary>
    public abstract class Piece
    {
        /// <summary>
        /// Gets the Player that owns this piece.
        /// </summary>
        public Player? Player { get; private set; }

        /// <summary>
        /// Gets the Square that this piece is currently on.
        /// </summary>
        public Square? Square { get; private set; }

        /// <summary>
        /// Indicates whether this piece requires an enemy to be present to perform an attack. The default is true.
        /// </summary>
        public virtual bool RequiresEnemyToAttack { get { return true; } }

        /// <summary>
        /// Initializes a new instance of the Piece class with the specified player and initial square.
        /// </summary>
        /// <param name="player">The player that owns this piece.</param>
        /// <param name="initialSquare">The square that this piece starts on.</param>
        public Piece(Player? player, Square initialSquare)
        {
            Player = player;
            EnterBoard(initialSquare);
        }

        /// <summary>
        /// Returns a string that represents the current piece.
        /// </summary>
        /// <returns>A string that represents the current piece.</returns>
        public override string ToString()
        {
            return $"{Player.Colour} {GetType().Name} at {Square.Row}, {Square.Col}";
        }

        /// <summary>
        /// Places this piece on the board at the specified square.
        /// </summary>
        /// <param name="square">The square to place this piece on.</param>
        public virtual void EnterBoard(Square square)
        {
            if (OnBoard) throw new Exception("Piece is already on the board");
            square.Place(this);
            Square = square;
        }

        /// <summary>
        /// Gets a value indicating whether this piece is currently on the board.
        /// </summary>
        public bool OnBoard
        {
            get
            {
                return Square != null;
            }
        }

        /// <summary>
        /// Removes this piece from the board.
        /// </summary>
        public virtual void LeaveBoard()
        {
            if (Square == null) throw new ArgumentNullException("Piece cannot be removed if it is not on the board");
            Square.Remove();
            Square = null;
        }

        /// <summary>
        /// Moves this piece to the specified square.
        /// </summary>
        /// <param name="newSquare">The square to move this piece to.</param>
        /// <returns>True if the piece was moved, otherwise false.</returns>
        public virtual bool MoveTo(Square newSquare)
        {
            if (!CanMoveTo(newSquare)) return false;
            if (newSquare.IsOccupied) return false;

            LeaveBoard();
            EnterBoard(newSquare);
            return true;
        }

        /// <summary>
        /// Determines whether this piece can move to the specified square.
        /// </summary>
        /// <param name="newSquare">The square to move this piece to.</param>
        /// <returns>True if the piece can move to the specified square, otherwise false.</returns>
        public abstract bool CanMoveTo(Square newSquare);

        /// <summary>
        /// Attacks the piece on the specified square.
        /// </summary>
        /// <param name="targetSquare">The square to attack.</param>
        /// <returns>True if the attack was successful, otherwise false.</returns>
        public virtual bool Attack(Square targetSquare)
        {
            if (!CanAttack(targetSquare)) return false;
            if (targetSquare.IsFree) return false;
            if (targetSquare == Square) return false;

            targetSquare.Occupant?.LeaveBoard();
            LeaveBoard();
            EnterBoard(targetSquare);
            return true;
        }

        /// <summary>
        /// Determines whether this piece can attack the specified square.
        /// </summary>
        /// <param name="newSquare">The square to attack.</param>
        /// <returns>True if the piece can attack the specified square, otherwise false.</returns>
        public abstract bool CanAttack(Square newSquare);

        /// <summary>
        /// Changes the side of this piece, moving it to the opponent's army and adopting the opponent as the Player.
        /// </summary>
        /// <remarks>
        /// This is its own inverse, calling it twice should put the piece back in the original army.
        /// </remarks>
        internal void Defect()
        {

            // p.Defect() should cause p to change sides (moving to the opponent's army
            // and adopting the opponent as the Player).
            if (Player == null) throw new Exception("Piece cannot be defected from unknown player.");

            Player? opponent = Player.Opponent;

            if (opponent == null) throw new Exception("Player has no opponent.");

            // Remove piece from original player
            Player.Army.RemovePiece(this);

            // Change player of current piece
            Player = opponent;

            // Add to opponent's army
            opponent.Army.AddPiece(this);

        }

        /// <summary>
        /// Gets the icon for this piece.
        /// </summary>
        /// <value>
        /// The icon is the first letter of the type name. It's uppercase for white pieces and lowercase for black pieces.
        /// </value>
        public virtual char Icon
        {
            get
            {
                var res = GetType().Name[0];

                return Player.Colour == Colour.Black ? char.ToLower(res) : char.ToUpper(res);
            }
        }

    }
}