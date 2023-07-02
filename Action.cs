namespace Advance
{
    /// <summary>
    /// Represents an action performed by a piece on the board.
    /// </summary>
    public abstract class Action
    {
        /// <summary>
        /// Gets the actor (piece) that performs the action.
        /// </summary>
        public Piece Actor { get; }

        /// <summary>
        /// Gets the target square of the action.
        /// </summary>
        public Square Target { get; }

        /// <summary>
        /// Gets the scores associated with the action for each player.
        /// </summary>
        public Dictionary<Colour, double> Scores { get; }

        /// <summary>
        /// Indicates whether the action was successful.
        /// </summary>
        protected bool success = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Action"/> class.
        /// </summary>
        /// <param name="actor">The actor (piece) that performs the action.</param>
        /// <param name="target">The target square of the action.</param>
        protected Action(Piece actor, Square target)
        {
            Actor = actor;
            Target = target;
            Scores = new Dictionary<Colour, double>();
            Scores[Colour.Black] = double.MinValue;
            Scores[Colour.White] = double.MinValue;
        }

        /// <summary>
        /// Performs the action.
        /// </summary>
        /// <returns><c>true</c> if the action is successfully performed; otherwise, <c>false</c>.</returns>
        public abstract bool DoAction();

        /// <summary>
        /// Undoes the action.
        /// </summary>
        public abstract void UndoAction();
    }

    /// <summary>
    /// Represents a move action performed by a piece on the board.
    /// </summary>
    public class Move : Action
    {
        private Square? previousSquare;
        private Piece? swappedPiece;
        private Square? swappedPieceOriginalSquare;

        /// <summary>
        /// Initializes a new instance of the <see cref="Move"/> class.
        /// </summary>
        /// <param name="actor">The actor (piece) that performs the move.</param>
        /// <param name="target">The target square of the move.</param>
        public Move(Piece actor, Square target) : base(actor, target)
        {
            previousSquare = actor.Square;

            // When current piece is a Jester and if the target is its friendly piece, swap position
            if (target.IsOccupied
                && target.Occupant?.Player == actor.Player 
                && !(target.Occupant is Jester))
            {
                swappedPiece = target.Occupant;
                swappedPieceOriginalSquare = swappedPiece.Square;
            }
        }

        /// <inheritdoc/>
        public override bool DoAction()
        {
            success = Actor.MoveTo(Target);
            return success;
        }

        /// <inheritdoc/>
        public override void UndoAction()
        {
            if (success && Actor != null && previousSquare != null)
            {
                Actor.LeaveBoard();

                if (swappedPiece != null && swappedPieceOriginalSquare != null)
                {
                    swappedPiece.LeaveBoard();
                    swappedPiece.EnterBoard(swappedPieceOriginalSquare);
                }
                Actor.EnterBoard(previousSquare);
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{previousSquare.Occupant} Moving to {Target.Row + 1}, {Target.Col + 1}";
        }
    }

    /// <summary>
    /// Represents an attack action performed by a piece on the board.
    /// </summary>
    public class Attack : Action
    {
        private Square? originalSquare;
        private Piece? opponentPiece;
        private Player? opponent;

        /// <summary>
        /// Initializes a new instance of the <see cref="Attack"/> class.
        /// </summary>
        /// <param name="actor">The actor (piece) that performs the attack.</param>
        /// <param name="target">The target square of the attack.</param>
        public Attack(Piece actor, Square target) : base(actor, target)
        {
            originalSquare = actor.Square;
            opponentPiece = target.Occupant;
            opponent = opponentPiece?.Player;
        }

        /// <inheritdoc/>
        public override bool DoAction()
        {
            return Actor.Attack(Target);
        }

        /// <inheritdoc/>
        public override void UndoAction()
        {
            Actor.LeaveBoard();

            if (opponentPiece != null)
            {
                if (opponentPiece.OnBoard)
                {
                    opponentPiece.LeaveBoard();
                }

                opponentPiece.EnterBoard(Target);

                if (opponentPiece.Player != opponent)
                {
                    opponentPiece.Defect();
                }
            }
            // otherwise, u must attacked the wall  

            Actor.EnterBoard(originalSquare);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{Actor.Icon} attacking {opponentPiece.Icon}";
        }
    }

}
