using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    /// <summary>
    /// Represents a Catapult piece in the game.
    /// </summary>
    internal class Catapult : Piece
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Catapult"/> class.
        /// </summary>
        /// <param name="player">The player that owns the Catapult.</param>
        /// <param name="initialSquare">The initial square where the Catapult is placed.</param>
        public Catapult(Player player, Square initialSquare) : base(player, initialSquare)
        {

        }

        /// <summary>
        /// Determines whether the Catapult can attack a specific square.
        /// </summary>
        /// <param name="newSquare">The square to attack.</param>
        /// <returns><c>true</c> if the Catapult can attack the square; otherwise, <c>false</c>.</returns>
        public override bool CanAttack(Square newSquare)
        {
            if (Square == null) throw new Exception("Cannot attack with piece that is off the board");

            int dx = Square.Row - newSquare.Row;
            int dy = Square.Col - newSquare.Col;

            // Calculate the absolute difference
            if (dx < 0) dx = -dx;
            if (dy < 0) dy = -dy;

            return dx == 0 && dy == 3 || dx == 3 && dy == 0 || dx == 2 && dy == 2;
        }

        /// <summary>
        /// Attacks a target square with the Catapult.
        /// </summary>
        /// <param name="targetSquare">The square to attack.</param>
        /// <returns><c>true</c> if the attack was successful; otherwise, <c>false</c>.</returns>
        public override bool Attack(Square targetSquare)
        {
            if (!CanAttack(targetSquare)) return false;
            if (targetSquare.IsFree) return false;
            if (targetSquare == Square) return false;

            targetSquare.Occupant?.LeaveBoard();
            return true;
        }

        /// <summary>
        /// Determines whether the Catapult can move to a specific square.
        /// </summary>
        /// <param name="newSquare">The square to move to.</param>
        /// <returns><c>true</c> if the Catapult can move to the square; otherwise, <c>false</c>.</returns>
        public override bool CanMoveTo(Square newSquare)
        {
            if (Square == null) throw new Exception("Cannot move piece that is off the board");
            if (newSquare == null)
                return false;

            return Square.AdjacentSquares.Any(square => square.Equals(newSquare));

        }

    }
}
