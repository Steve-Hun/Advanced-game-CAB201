using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    /// <summary>
    /// Represents a Zombie piece in the game.
    /// Inherits from the Piece class.
    /// </summary>
    internal class Zombie : Piece
    {
        /// <summary>
        /// Constructor for the Zombie class.
        /// </summary>
        /// <param name="player">The player who controls this Zombie piece.</param>
        /// <param name="initialSquare">The initial square where this Zombie piece is placed.</param>
        public Zombie(Player player, Square initialSquare) : base(player, initialSquare)
        {
        }

        /// <summary>
        /// Determines if this Zombie piece can attack a square.
        /// </summary>
        /// <param name="newSquare">The square to attack.</param>
        /// <returns>True if the Zombie piece can attack the square, false otherwise.</returns>
        public override bool CanAttack(Square newSquare)
        {
            if (Square == null) throw new Exception("Cannot attack with piece that is off the board");


            int rowDiff = newSquare.Row - Square.Row;
            int colDiff = newSquare.Col - Square.Col;

            // Check for regular move
            if (rowDiff == Player.Direction && (colDiff == 0 || colDiff == 1 || colDiff == -1))
                return true;

            // Check for Leaping attack
            if (rowDiff == 2 * Player.Direction && (colDiff == 0 || colDiff == -2 || colDiff == 2))
            {

                Square immediateSquare = Square.Board.Get(Square.Row + Player.Direction, Square.Col + colDiff / 2);
                if (!immediateSquare.IsOccupied)
                {
                    return true;
                }
            }
            return false;

        }

        /// <summary>
        /// Determines if this Zombie piece can move to a square.
        /// </summary>
        /// <param name="newSquare">The square to move to.</param>
        /// <returns>True if the Zombie piece can move to the square, false otherwise.</returns>
        public override bool CanMoveTo(Square newSquare)
        {
            if (Square == null) throw new Exception("Cannot move with piece that is off the board");

            int rowDiff = newSquare.Row - Square.Row;
            int colDiff = newSquare.Col - Square.Col;

            if (rowDiff != Player.Direction) return false;
            if (colDiff == 0 || colDiff == 1 || colDiff == -1) return true;

            return false;
        }
    }
}
