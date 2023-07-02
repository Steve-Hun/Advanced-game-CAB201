using System.Reflection.PortableExecutable;

namespace Advance;

/// <summary>
/// Represents the Game which includes two players, a board, and the wall pieces.
/// </summary>
public class Game
{
    /// <summary>
    /// Gets the white player in the game.
    /// </summary>
    public Player White { get; }

    /// <summary>
    /// Gets the black player in the game.
    /// </summary>
    public Player Black { get; }

    /// <summary>
    /// Gets the game board.
    /// </summary>
    public Board Board { get; }

    public List<Wall> walls = new List<Wall>();

    /// <summary>
    /// Initializes a new instance of the Game class.
    /// </summary>
    public Game()
    {
        Board = new Board();
        White = new Player(Colour.White, this);
        Black = new Player(Colour.Black, this);
    }

    /// <summary>
    /// Returns a string that represents the current game state.
    /// </summary>
    /// <returns>A string that represents the current game state.</returns>
    public override string ToString()
    {
        StringWriter writer = new StringWriter();
        Write(writer);
        return writer.ToString();
    }

    /// <summary>
    /// Clears the board of all pieces.
    /// </summary>
    public void Clear()
    {
        Black.Army.RemoveAllPieces();
        White.Army.RemoveAllPieces();
    }

    /// <summary>
    /// Reads the game state from a text reader.
    /// </summary>
    /// <param name="reader">The text reader to read the game state from.</param>
    public void Read(TextReader? reader)
    {
        Clear();

        for (int row = 0; row < Board.Size; row++)
        {
            string? currentRow = reader.ReadLine();

            if (currentRow == null)
                throw new Exception("Ran out of data before reading full board");
            if (currentRow.Length != Board.Size)
            {
                Console.WriteLine($"row length {currentRow.Length}");
                throw new Exception($"Row {row} is not the right length");
            }


            for (int col = 0; col < Board.Size; col++)
            {
                Square? currentSquare = Board.Get(row, col);
                char icon = currentRow[col];

                if (icon != '.')
                {
                    Player currentPlayer = Char.IsLower(icon) ? Black : White;
                    currentPlayer.Army.Recruit(icon, currentSquare);
                }
        }
        }
    }

    /// <summary>
    /// Writes the game state to a text writer.
    /// </summary>
    /// <param name="writer">The text writer to write the game state to.</param>
    public void Write(TextWriter writer)
    {
        for (int row = 0; row < Board.Size; row++)
        {
            for (int col = 0; col < Board.Size; col++)
            {
                Square currentSquare = Board.Get(row, col);
                Piece? currentPiece = currentSquare.Occupant;

                if (currentPiece == null)
                    writer.Write('.');
                else
                    writer.Write(currentPiece.Icon);
            }
            writer.WriteLine();
        }
    }

    

}