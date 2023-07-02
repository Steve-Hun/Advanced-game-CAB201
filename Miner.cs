using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    /// <summary>
    /// Represents the Miner piece in the game.
    /// </summary>
    internal class Miner : Piece
    {
        /// <summary>
        /// Creates a new instance of Miner.
        /// </summary>
        /// <param name="player">The player who owns the piece.</param>
        /// <param name="initialSquare">The initial square where the piece is placed.</param>
        public Miner(Player player, Square initialSquare) : base(player, initialSquare)
        {
        }

        /// <summary>
        /// Checks if the miner can attack a square.
        /// </summary>
        /// <param name="newSquare">The square to be attacked.</param>
        /// <returns>True if the miner can attack the square, false otherwise.</returns>
        public override bool CanAttack(Square newSquare)
        {
            return CanMoveTo(newSquare);
        }

        /// <summary>
        /// Checks if the miner can move to a square.
        /// </summary>
        /// <param name="newSquare">The square to be moved to.</param>
        /// <returns>True if the miner can move to the square, false otherwise.</returns>
        public override bool CanMoveTo(Square newSquare)
        {
            if (Square == null) throw new Exception("Cannot Move with piece that is off the board");

            if (Square.Row == newSquare.Row && Square.Col == newSquare.Col) return false;

            bool success = true;

            if (Square.Row == newSquare.Row)
            {
                if (HasPiecesBetweenRowDir(Square, newSquare))
                    success = false;
            }
            else if (Square.Col == newSquare.Col)
            {
                if (HasPiecesBetweenColumnDir(Square, newSquare))
                    success = false;
            }
            else
            {
                // if newSquare is neither in the same row or column
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Checks if there are any pieces between the source and destination squares in the same row.
        /// </summary>
        /// <param name="source">The source square.</param>
        /// <param name="des">The destination square.</param>
        /// <returns>True if there are pieces between the squares, false otherwise.</returns>
        private bool HasPiecesBetweenRowDir(Square source, Square des)
        {
            if (source == null || des == null) throw new ArgumentNullException("Source or Destination square cannot be null.");
            if (source.Row == des.Row)
            {
                int startCol = Math.Min(source.Col, des.Col);
                int endCol = Math.Max(source.Col, des.Col);

                // Check if each square between start and end row contains any pieces
                for (int col = startCol + 1; col < endCol; col++)
                {
                    Square squareBetween = source.Board.Get(source.Row, col);
                    if (squareBetween.IsOccupied)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if there are any pieces between the source and destination squares in the same column.
        /// </summary>
        /// <param name="source">The source square.</param>
        /// <param name="des">The destination square.</param>
        /// <returns>True if there are pieces between the squares, false otherwise.</returns>
        private bool HasPiecesBetweenColumnDir(Square source, Square des)
        {
            if (source == null || des == null) throw new ArgumentNullException("Source or Destination square cannot be null.");
            if (source.Col == des.Col)
            {
                int startRow = Math.Min(source.Row, des.Row);
                int endRow = Math.Max(source.Row, des.Row);

                // Check if each square between start and end column contains any pieces
                for (int row = startRow + 1; row < endRow; row++)
                {
                    Square squareBetween = source.Board.Get(row, source.Col);
                    if (squareBetween.IsOccupied)
                    {
                        return true;
                    }
                }
            }


            return false;
        }




    }
}


