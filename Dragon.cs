using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    /// <summary>
    /// Represents a Dragon piece in the game. Inherits from the Piece class.
    /// </summary>
    internal class Dragon : Piece
    {
        /// <summary>
        /// Initializes a new instance of the Dragon class.
        /// </summary>
        /// <param name="player">The player that the piece belongs to.</param>
        /// <param name="initialSquare">The initial position of the piece on the board.</param>
        public Dragon(Player player, Square initialSquare) : base(player, initialSquare)
        {

        }

        /// <summary>
        /// Determines whether the piece can attack a specified square.
        /// </summary>
        /// <param name="newSquare">The square to potentially attack.</param>
        /// <returns>True if the piece can attack the specified square; otherwise, false.</returns>
        public override bool CanAttack(Square newSquare)
        {
            if (!IsNeighbour(newSquare) && CanMoveTo(newSquare))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether the piece can move to a specified square.
        /// </summary>
        /// <param name="newSquare">The square to potentially move to.</param>
        /// <returns>True if the piece can move to the specified square; otherwise, false.</returns>
        public override bool CanMoveTo(Square newSquare)
        {
            if (Square == null) throw new Exception("Cannot move piece that is off the board");


            int dx = Square.Row - newSquare.Row;
            int dy = Square.Col - newSquare.Col;

            // Calculate the absolute difference
            if (dx < 0) dx = -dx;
            if (dy < 0) dy = -dy;

            // Cannot move to current square
            if (dx == 0 && dy == 0) return false;

            // Moving horizontally
            if (Square.Row == newSquare.Row && !HasPiecesBetweenRowDir(Square, newSquare))
                return true;
            // Moving vertically
            else if (Square.Col == newSquare.Col && !HasPiecesBetweenColumnDir(Square, newSquare))
                return true;
            //// Moving diagonally
            else if (dx == dy && !HasPiecesBetweenDiagonalDir(Square, newSquare))
                return true;
            return false;

        }

        /// <summary>
        /// Checks if there are any pieces between the current square and the destination square in a diagonal direction.
        /// </summary>
        /// <param name="source">The current square.</param>
        /// <param name="des">The destination square.</param>
        /// <returns>True if there are pieces in between; otherwise, false.</returns>
        private bool HasPiecesBetweenDiagonalDir(Square source, Square des)
        {
            if (source.Row == des.Row && source.Col == des.Col) throw new ArgumentException("Cannot find pieces in between the same square");

            // Find which direction to go
            int rowStep = source.Row < des.Row ? 1 : -1;
            int colStep = source.Col < des.Col ? 1 : -1;

            // Skip the starting square
            int startRow = source.Row + rowStep;
            int startCol = source.Col + colStep;

            while (startRow != des.Row && startCol != des.Col)
            {
                if (source.Board.Get(startRow, startCol).IsOccupied)
                {
                    return true;
                }
                startRow += rowStep;
                startCol += colStep;
            }

            return false;
        }

        /// <summary>
        /// Checks if the target square is a neighbour of the current square.
        /// </summary>
        /// <param name="targetSquare">The target square to check.</param>
        /// <returns>True if the target square is a neighbour; otherwise, false.</returns>
        public bool IsNeighbour(Square targetSquare)
        {
            return Square.NeighbourSquares.Any(square => square.Equals(targetSquare));
        }

        /// <summary>
        /// Checks if there are any pieces between the current square and the destination square in the row direction.
        /// </summary>
        /// <param name="source">The current square.</param>
        /// <param name="des">The destination square.</param>
        /// <returns>True if there are pieces in between; otherwise, false.</returns>
        private bool HasPiecesBetweenRowDir(Square source, Square des)
        {
            if (source == null || des == null) throw new ArgumentNullException("Source or Destination square cannot be null.");
            if (source.Row == des.Row)
            {
                int startCol = Math.Min(source.Col, des.Col);
                int endCol = Math.Max(source.Col, des.Col);

                // Check if each square between start and end column contains any pieces
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
        /// Checks if there are any pieces between the current square and the destination square in the column direction.
        /// </summary>
        /// <param name="source">The current square.</param>
        /// <param name="des">The destination square.</param>
        /// <returns>True if there are pieces in between; otherwise, false.</returns>
        private bool HasPiecesBetweenColumnDir(Square source, Square des)
        {
            if (source == null || des == null) throw new ArgumentNullException("Source or Destination square cannot be null.");
            if (source.Col == des.Col)
            {
                int startRow = Math.Min(source.Row, des.Row);
                int endRow = Math.Max(source.Row, des.Row);

                // Check if each square between start and end row contains any pieces
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
