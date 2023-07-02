using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    /// <summary>
    /// Represents a Sentinel piece in the game.
    /// Inherits from the Piece class.
    /// </summary>
    internal class Sentinel : Piece
    {

        /// <summary>
        /// Constructor for the Sentinel class.
        /// </summary>
        /// <param name="player">The player who controls this Sentinel piece.</param>
        /// <param name="initialSquare">The initial square where this Sentinel piece is placed.</param>
        public Sentinel(Player player, Square initialSquare) : base(player, initialSquare)
        {
        }

        /// <summary>
        /// Determines if this Sentinel piece can move to a square. 
        /// </summary>
        /// <param name="newSquare">The square to move to.</param>
        /// <returns>True if the Sentinel piece can move to the new square, false otherwise.</returns>
        public override bool CanMoveTo(Square newSquare)
        {
            if (newSquare == null)
                return false;
            if (newSquare == Square) return false;

            int dy = newSquare.Row - Square.Row;
            int dx = newSquare.Col - Square.Col;

            if (dx < 0) dx = -dx;
            if (dy < 0) dy = -dy;

            return dx != 0 && dy != 0 && dx + dy == 3;
        }

        // <summary>
        /// Determines if this Sentinel piece can attack a target square. 
        /// </summary>
        /// <param name="targetSquare">The square to attack.</param>
        /// <returns>True if the Sentinel piece can attack the target square, false otherwise.</returns>
        public override bool CanAttack(Square targetSquare)
        {
            return CanMoveTo(targetSquare);
        }

        /// <summary>
        /// Enumerates the squares that are protected by this Sentinel piece.
        /// </summary>
        private IEnumerable<Square> ProtectedSquares
        {
            get
            {
                return Square.AdjacentSquares;
            }
        }

        /// <summary>
        /// Determines if a target square is protected by this Sentinel piece.
        /// </summary>
        /// <param name="targetSquare">The target square to check.</param>
        /// <returns>True if the target square is protected, false otherwise.</returns>
        public bool IsProtected(Square targetSquare)
        {
            if (targetSquare == null) throw new ArgumentNullException("Target square cannot be null");
            return ProtectedSquares.Any(square => square.Equals(targetSquare));
        }
    }
}
