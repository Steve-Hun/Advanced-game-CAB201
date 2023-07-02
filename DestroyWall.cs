namespace Advance
{
    /// <summary>
    /// Represents an action to destroy a wall in the game. Inherits from the Action class.
    /// </summary>
    internal class DestroyWall : Action
    {
        private Wall? builtWall;

        private Square? actorInitialSquare;

        /// <summary>
        /// Initializes a new instance of the DestroyWall class.
        /// </summary>
        /// <param name="actor">The piece that is performing the action.</param>
        /// <param name="target">The target square of the action.</param>
        public DestroyWall(Piece actor, Square target) : base(actor, target)
        {
            if (!(actor is Miner)) throw new Exception("Only Miner can destroy a wall.");
            actorInitialSquare = actor.Square;
        }

        /// <summary>
        /// Executes the action to destroy a wall.
        /// </summary>
        /// <returns>True if the action was successful; otherwise, false.</returns>
        public override bool DoAction()
        {
            if (Target.IsFree) throw new Exception("Cannot destroy a wall on an empty square.");
            if (!Target.ContainsWall) return false;
            builtWall = Target.Occupant as Wall;
            builtWall.LeaveBoard();

            // Move actor to current square
            Actor.MoveTo(Target);
            return true;
        }

        /// <summary>
        /// Reverts the action of destroying a wall.
        /// </summary>
        public override void UndoAction()
        {
            Actor.MoveTo(actorInitialSquare);
            if (Target.IsFree && builtWall != null)
            {
                builtWall.EnterBoard(Target);
            }
        }
    }
}