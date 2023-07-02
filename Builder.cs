using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    /// <summary>
    /// Represents a builder piece.
    /// </summary>
    internal class Builder : Piece
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Builder"/> class.
        /// </summary>
        /// <param name="player">The player who owns the builder.</param>
        /// <param name="initialSquare">The initial square where the builder is placed.</param>
        public Builder(Player player, Square initialSquare) : base(player, initialSquare)
        {

        }

        /// <summary>
        /// Gets a value indicating whether the builder requires an enemy to attack.
        /// Builders can attack walls directly without requiring an enemy piece.
        /// </summary>
        public override bool RequiresEnemyToAttack { get { return false; } }

        /// <summary>
        /// Attacks a square, removing any piece occupying it.
        /// </summary>
        /// <param name="targetSquare">The target square to attack.</param>
        /// <returns><c>true</c> if the attack was successful; otherwise, <c>false</c>.</returns>
        public override bool Attack(Square targetSquare)
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
        /// Determines whether the builder can attack a square.
        /// Builders can attack squares within a range of 1 in any direction.
        /// </summary>
        /// <param name="newSquare">The square to attack.</param>
        /// <returns><c>true</c> if the builder can attack the square; otherwise, <c>false</c>.</returns>
        public override bool CanAttack(Square newSquare)
        {
            if (Square == null) throw new Exception("Cannot attack with piece that is off the board");
            if (Square == newSquare) return false;

            int dx = newSquare.Row - Square.Row;
            int dy = newSquare.Col - Square.Col;

            if (dx < 0) dx = -dx;
            if (dy < 0) dy = -dy;

            if (dx == 0 && dy == 0) return false;

            return dx <= 1 && dy <= 1;
        }

        /// <summary>
        /// Determines whether the builder can move to a square.
        /// Builders can move to squares within a range of 1 in any direction.
        /// </summary>
        /// <param name="newSquare">The square to move to.</param>
        /// <returns><c>true</c> if the builder can move to the square; otherwise, <c>false</c>.</returns>
        public override bool CanMoveTo(Square newSquare)
        {
            if (Square == null) throw new Exception("Cannot Move with piece that is off the board");

            int dx = newSquare.Row - Square.Row;
            int dy = newSquare.Col - Square.Col;

            if (dx < 0) dx = -dx;
            if (dy < 0) dy = -dy;

            if (dx == 0 && dy == 0) return false;

            return dx <= 1 && dy <= 1;
        }
    }
}
