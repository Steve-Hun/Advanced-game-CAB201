namespace Advance {

    /// <summary>
    /// Represents the game board.
    /// </summary>
    public class Board {
        /// <summary>
        /// The size of the board.
        /// </summary>
        public const int Size = 9;

        private Square[] squares = new Square[Size * Size];

        /// <summary>
        /// Gets the collection of squares on the board.
        /// </summary>
        public IEnumerable<Square> Squares {
            get {
                return squares;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Board"/> class.
        /// </summary>
        public Board()
        {
            for(int row = 0; row < Size; row++) {
                for (int col = 0; col < Size; col++) {
                    Square newSquare = new Square(this, row, col);
                    Set(row, col, newSquare);
                }
            }
        }

        private void Set(int row, int col, Square newSquare) {
            if (row < 0 || row >= Size || col < 0 || col >= Size) throw new ArgumentException();
            squares[row * Size + col] = newSquare;
        }

        /// <summary>
        /// Gets the square at the specified row and column.
        /// </summary>
        /// <param name="row">The row index.</param>
        /// <param name="col">The column index.</param>
        /// <returns>The square at the specified position, or <c>null</c> if the position is invalid.</returns>
        public Square ? Get( int row, int col) {
            if (row < 0 || row >= Size || col < 0 || col >= Size) return null;
            return squares[row * Size + col];
        }

        /// <summary>
        /// Returns a string that represents the board.
        /// </summary>
        /// <returns>A string representation of the board.</returns>
        public override string ToString() {
            return $"Board:\n{string.Join<Square>("\n", squares)}";
        }
    }
}