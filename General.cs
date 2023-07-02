using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    /// <summary>
    /// Represents the General piece in the game.
    /// </summary>
    internal class General : Piece
    {
        /// <summary>
        /// Creates a new instance of General.
        /// </summary>
        /// <param name="player">The player who owns the piece.</param>
        /// <param name="initialSquare">The initial square where the piece is placed.</param>
        public General(Player player, Square initialSquare) : base(player, initialSquare)
        {
        }

        /// <summary>
        /// Checks if the general can attack a square.
        /// </summary>
        /// <param name="newSquare">The square to be attacked.</param>
        /// <returns>True if the general can attack the square, false otherwise.</returns>
        public override bool CanAttack(Square newSquare)
        {
            bool success = Square.NeighbourSquares.Any(square => square.Equals(newSquare))
                          && !newSquare.IsThreatenedBy(Player.Opponent);
            return success;
        }

        /// <summary>
        /// Checks if the general can move to a square.
        /// </summary>
        /// <param name="newSquare">The square to be moved to.</param>
        /// <returns>True if the general can move to the square, false otherwise.</returns>
        public override bool CanMoveTo(Square newSquare)
        {
            bool success = Square.NeighbourSquares.Any(square => square.Equals(newSquare))
                          && !newSquare.IsThreatenedBy(Player.Opponent);
            return success;
        }
    }
}
