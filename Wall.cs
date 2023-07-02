using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    /// <summary>
    /// Represents a Wall piece in the game.
    /// Inherits from the Piece class.
    /// </summary>
    /// 
    public class Wall : Piece
    {


        /// <summary>
        /// Constructor for the Wall class.
        /// </summary>
        /// <param name="player">The player who controls this Wall piece.</param>
        /// <param name="initialSquare">The initial square where this Wall piece is placed.</param>
        public Wall(Player? player, Square initialSquare) : base(player, initialSquare)
        {

        }

        /// <summary>
        /// Determines if this Wall piece can attack a square. 
        /// Wall cannot attack so it always returns false.
        /// </summary>
        /// <param name="newSquare">The square to attack.</param>
        /// <returns>False because a Wall cannot attack.</returns>
        public override bool CanAttack(Square newSquare)
        {
            return false;
        }

        /// <summary>
        /// Determines if this Wall piece can move to a square. 
        /// Wall cannot move so it always returns false.
        /// </summary>
        /// <param name="newSquare">The square to move to.</param>
        /// <returns>False because a Wall cannot move.</returns>
        public override bool CanMoveTo(Square newSquare)
        {
            return false;
        }

        /// <summary>
        /// Overrides the ToString method to provide details about this Wall piece.
        /// </summary>
        /// <returns>A string that represents the Wall piece.</returns>
        public override string ToString()
        {
            return $"{GetType().Name} at {Square.Row}, {Square.Col}";
        }

        /// <summary>
        /// The icon representing the Wall piece.
        /// </summary>
        public override char Icon
        {
            get { return '#'; }
        }
    }
}
