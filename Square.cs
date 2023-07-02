using System.Data.Common;

namespace Advance
{
    /// <summary>
    /// Represents a square on a game board.
    /// </summary>
    public class Square
    {
        /// <summary>
        /// Gets the board that this square belongs to.
        /// </summary>
        public Board Board { get; }

        /// <summary>
        /// Gets the row index of this square on the game board.
        /// </summary>
        public int Row { get; }

        /// <summary>
        /// Gets the column index of this square on the game board.
        /// </summary>
        public int Col { get; }

        /// <summary>
        /// Initializes a new instance of the Square class.
        /// </summary>
        /// <param name="board">The board that this square belongs to.</param>
        /// <param name="row">The row index of this square on the game board.</param>
        /// <param name="col">The column index of this square on the game board.</param>

        private Piece? occupant;

        public Square(Board board, int row, int col)
        {
            Board = board;
            Row = row;
            Col = col;
        }


        /// <summary>
        /// Gets or sets the piece currently occupying this square.
        /// </summary>
        public Piece? Occupant
        {
            get { return occupant; }
            private set
            {
                if (value == null) throw new ArgumentNullException();
                occupant = value;
            }
        }

        /// <summary>
        /// Determines whether the square is unoccupied.
        /// </summary>
        public bool IsFree
        {
            get
            {
                return Occupant == null;
            }
        }

        /// <summary>
        /// Determines whether the square is occupied by a piece.
        /// </summary>
        public bool IsOccupied
        {
            get
            {
                return !IsFree;
            }
        }

        /// <summary>
        /// Determines whether the square contains a wall.
        /// </summary>
        public bool ContainsWall
        {
            get { return Occupant.Player == null; }
        }

        /// <summary>
        /// Places a piece on the square.
        /// </summary>
        public void Place(Piece piece)
        {
            if (IsOccupied) throw new ArgumentException("Piece cannot be placed in occupied square.");
            Occupant = piece;
        }

        /// <summary>
        /// Removes the piece from the square.
        /// </summary>
        public void Remove()
        {
            occupant = null;
        }

        /// <summary>
        /// Gets the squares adjacent to this square in the four cardinal directions.
        /// </summary>
        public IEnumerable<Square> AdjacentSquares
        {
            get
            {
                List<Square> adjacentSquares = new List<Square>();

                // Square above
                if (Row - 1 >= 0)
                {
                    Square squareAbove = Board.Get(Row - 1, Col);
                    adjacentSquares.Add(squareAbove);
                }

                // Square below
                if (Row + 1 < Board.Size)
                {
                    Square squareBelow = Board.Get(Row + 1, Col);
                    adjacentSquares.Add(squareBelow);
                }

                // Left square
                if (Col - 1 >= 0)
                {
                    Square squareLeft = Board.Get(Row, Col - 1);
                    adjacentSquares.Add(squareLeft);
                }

                // Right square
                if (Col + 1 < Board.Size)
                {
                    Square squareRight = Board.Get(Row, Col + 1);
                    adjacentSquares.Add(squareRight);
                }

                return adjacentSquares;
            }
        }

        /// <summary>
        /// Gets all squares neighboring this square, including diagonals.
        /// </summary>
        public IEnumerable<Square> NeighbourSquares
        {
            get
            {
                List<Square> neighbourSquares = new List<Square>();

                // Square above
                if (Row - 1 >= 0)
                {
                    Square squareAbove = Board.Get(Row - 1, Col);
                    neighbourSquares.Add(squareAbove);
                }

                // Square below
                if (Row + 1 < Board.Size)
                {
                    Square squareBelow = Board.Get(Row + 1, Col);
                    neighbourSquares.Add(squareBelow);
                }

                // Left square
                if (Col - 1 >= 0)
                {
                    Square squareLeft = Board.Get(Row, Col - 1);
                    neighbourSquares.Add(squareLeft);
                }

                // Right square
                if (Col + 1 < Board.Size)
                {
                    Square squareRight = Board.Get(Row, Col + 1);
                    neighbourSquares.Add(squareRight);
                }

                // Top left square
                if (Row - 1 >= 0 && Col - 1 >= 0)
                {
                    Square squareTopLeft = Board.Get(Row - 1, Col - 1);
                    neighbourSquares.Add(squareTopLeft);
                }

                // Top right square
                if (Row - 1 >= 0 && Col + 1 < Board.Size)
                {
                    Square squareTopRight = Board.Get(Row - 1, Col + 1);
                    neighbourSquares.Add(squareTopRight);
                }

                // Bottom left square
                if (Row + 1 < Board.Size && Col - 1 >= 0)
                {
                    Square squareBottomLeft = Board.Get(Row + 1, Col - 1);
                    neighbourSquares.Add(squareBottomLeft);
                }

                // Bottom right square
                if (Row + 1 < Board.Size && Col + 1 < Board.Size)
                {
                    Square squareBottomRight = Board.Get(Row + 1, Col + 1);
                    neighbourSquares.Add(squareBottomRight);
                }

                return neighbourSquares;
            }
        }

        /// <summary>
        /// Returns the pieces that threaten this square for a specific player.
        /// </summary>
        public IEnumerable<Piece> ThreateningPieces(Player player)
        {
            List<Piece> threateningPieces = new List<Piece>();
            foreach (var piece in player.Army.Pieces)
            {
                if (!piece.OnBoard) continue;

                if (piece is General)
                {
                    // For General, check only the neighbour squares, not using IsThreatenedBy
                    if (piece.Square.NeighbourSquares.Any<Square>(square => square.Equals(this)))
                    {
                        threateningPieces.Add(piece);
                    }
                }
                else
                {
                    // If a piece can attack this square, it's threatened
                    if (piece.CanAttack(this))
                    {
                        threateningPieces.Add(piece);
                    }
                }

            }
            return threateningPieces;
        }

        /// <summary>
        /// Determines if the square is threatened by any of a player's pieces.
        /// </summary>
        public bool IsThreatenedBy(Player player)
        {
            return ThreateningPieces(player).Count() != 0;
        }

        /// <summary>
        /// Returns a string representation of the Square.
        /// </summary>
        public override string ToString()
        {
            if (IsFree)
                return $"Empty square at {Row}, {Col}";
            else
                return $"{Occupant}";
        }
    }
}