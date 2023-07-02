using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    /// <summary>
    /// Represents the Jester piece in the game.
    /// </summary>
    internal class Jester : Piece
    {
        /// <summary>
        /// Creates a new instance of Jester.
        /// </summary>
        /// <param name="player">The player who owns the piece.</param>
        /// <param name="initialSquare">The initial square where the piece is placed.</param>
        public Jester(Player player, Square initialSquare) : base(player, initialSquare)
        {
        }

        /// <summary>
        /// Attacks a target square, causing the target piece to defect.
        /// </summary>
        /// <param name="targetSquare">The square to be attacked.</param>
        /// <returns>True if the attack is successful, false otherwise.</returns>
        public override bool Attack(Square targetSquare)
        {
            if (!CanAttack(targetSquare)) return false;

            Piece? targetPiece = targetSquare.Occupant;

            // Defect the enemy piece
            targetPiece?.Defect();
            return true;
        }

        /// <summary>
        /// Checks if the jester can attack a square.
        /// </summary>
        /// <param name="newSquare">The square to be attacked.</param>
        /// <returns>True if the jester can attack the square, false otherwise.</returns>
        public override bool CanAttack(Square newSquare)
        {
            if (Square == null) throw new Exception("Cannot Attack with piece that is off the board");

            int dx = newSquare.Row - Square.Row;
            int dy = newSquare.Col - Square.Col;

            if (dx < 0) dx = -dx;
            if (dy < 0) dy = -dy;

            if (dx == 0 && dy == 0) return false;

            if (dx <= 1 && dy <= 1
                && !(newSquare.Occupant is General))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the jester can move to a square.
        /// </summary>
        /// <param name="newSquare">The square to be moved to.</param>
        /// <returns>True if the jester can move to the square, false otherwise.</returns>
        public override bool CanMoveTo(Square newSquare)
        {
            if (Square == null) throw new Exception("Cannot Move with piece that is off the board");

            int dx = newSquare.Row - Square.Row;
            int dy = newSquare.Col - Square.Col;

            if (dx < 0) dx = -dx;
            if (dy < 0) dy = -dy;

            if (dx == 0 && dy == 0) return false;

            if (dx <= 1 && dy <= 1 && newSquare.IsFree) return true;

            // Check if the square is occupied by a friendly piece (but not another Jester)
            if (dx <= 1 && dy <= 1 && newSquare.IsOccupied
                && newSquare.Occupant.Player == Player
                && !(newSquare.Occupant is Jester))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Moves the jester to a new square, swapping places with a friendly piece if one is present.
        /// </summary>
        /// <param name="newSquare">The square to be moved to.</param>
        /// <returns>True if the move is successful, false otherwise.</returns>
        public override bool MoveTo(Square newSquare)
        {
            if (!CanMoveTo(newSquare)) return false;

            Square currentSquare = Square;
            Piece? friendlyPiece = newSquare.Occupant;
            if (newSquare.IsOccupied
                && newSquare.Occupant.Player == Player
                && !(newSquare.Occupant is Jester))
            {
                friendlyPiece.LeaveBoard();
                LeaveBoard(); // Jester leaves board
                friendlyPiece.EnterBoard(currentSquare);
                EnterBoard(newSquare);
            }
            else // Move to empty square
            {
                LeaveBoard();
                EnterBoard(newSquare);

            }
            return true;
        }
    }
}
