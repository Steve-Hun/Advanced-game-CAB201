
namespace Advance;

public class Program
{
    private static void Main(string[] args)
    {

        Game game = new Game();

        if (ParseArguments(args, out string infile,
                out string outfile, out Colour colour
        ))
        {
            Load(game, infile);
            PlayOneTurn(game, colour);
            Save(game, outfile);
        }
        else
        {

            Console.WriteLine("Error processing command line arguments. Expected: ");
            Console.WriteLine("args[0] == colour (white or black)");
            Console.WriteLine("args[1] == input file path");
            Console.WriteLine("args[2] == output file path");
            Console.WriteLine("Your assignment may have different requirements.");
        }
    }


    /// <summary>
    /// Parses the command-line arguments to extract the input file, output file, and color.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    /// <param name="infile">Output variable to store the input file path.</param>
    /// <param name="outfile">Output variable to store the output file path.</param>
    /// <param name="colour">Output variable to store the color (white or black).</param>
    /// <returns>True if the arguments were successfully parsed; otherwise, false.</returns>
    private static bool ParseArguments(
        string[] args,
        out string infile,
        out string outfile,
        out Colour colour
    )
    {
        infile = string.Empty;
        outfile = string.Empty;
        colour = Colour.White;

        if (args.Length >= 3)
        {
            infile = args[1];
            outfile = args[2];
            return Enum.TryParse<Colour>(args[0], ignoreCase: true, out colour);
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Loads the game from the specified input file.
    /// </summary>
    /// <param name="game">The game object.</param>
    /// <param name="infile">The input file path.</param>
    private static void Load(Game game, string infile)
    {
        try
        {
            using StreamReader reader = new StreamReader(infile);
            game.Read(reader);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error loading game from '{0}': {1}", infile, ex.Message);
        }
    }

    /// <summary>
    /// Plays one turn of the game for the specified color.
    /// </summary>
    /// <param name="game">The game object.</param>
    /// <param name="colour">The color to play the turn (white or black).</param>
    private static void PlayOneTurn(Game game, Colour colour)
    {

        Player currentPlayer = colour == Colour.White ? game.White : game.Black;
        AI gameBot = new AI(game, currentPlayer);

        Action nextMove = gameBot.ChooseBestAction();
        if (nextMove != null)
        {
            nextMove.DoAction();
        }
        
        
    }

    /// <summary>
    /// Saves the game to the specified output file.
    /// </summary>
    /// <param name="game">The game object.</param>
    /// <param name="outfile">The output file path.</param>
    private static void Save(Game game, string outfile)
    {
        try
        {
            using StreamWriter writer = new StreamWriter(outfile);
            game.Write(writer);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error saving game to '{0}': {1}", outfile, ex.Message);
        }
    }
}