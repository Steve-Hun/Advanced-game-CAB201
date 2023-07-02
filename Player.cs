using System;
using System.Data.SqlTypes;
using System.Drawing;

namespace Advance
{
    /// <summary>
    /// Represents a player in the game, with their own unique colour and army.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Gets the colour of the player's pieces.
        /// </summary>
        public Colour Colour { get; }

        /// <summary>
        /// Gets the player's army.
        /// </summary>
        public Army Army { get; }

        /// <summary>
        /// Gets the game in which the player is participating.
        /// </summary>
        public Game Game { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="colour">The colour of the player's pieces.</param>
        /// <param name="game">The game in which the player is participating.</param>
        public Player(Colour colour, Game game)
        {
            Colour = colour;
            Game = game;
            Army = new Army(this, Game.Board);
        }

        /// <summary>
        /// Gets the opponent of the player.
        /// </summary>
        public Player Opponent
        {
            get
            {
                if (Game.White.Colour == this.Colour)
                    return Game.Black;
                else
                    return Game.White;
            }
        }

        /// <summary>
        /// Gets the direction of the player's movement, determined by the player's colour.
        /// </summary>
        public int Direction
        {
            get
            {
                return Colour == Colour.Black ? +1 : -1;
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Player {Colour}";
        }

        /// <summary>
        /// Chooses a move for the player. If there are possible actions, it picks a random one. If there are no possible actions, it returns null.
        /// </summary>
        /// <param name="actions">A list of possible actions.</param>
        /// <returns>An Action object representing the chosen move, or null if no moves are possible.</returns>
        public Action ChooseMove(List<Action>? actions = null)
        {
            if (actions == null) actions = new List<Action>();

            FindPossibleActions(actions);
            // Return a random action
            if (actions.Count() == 0)
                return null;
            else
                return actions[rand.Next(actions.Count())];
        }

        static Random rand = new Random();

        /// <summary>
        /// Finds the possible actions that a player can take on their turn.
        /// </summary>
        /// <param name="actions">A list of Action objects representing the possible actions.</param>
        public void FindPossibleActions(List<Action> actions)
        {
            if (IsGeneralSafe())
            {
                if (actions.Count >= 1) return;
                FindPossibleMoves(actions);
                FindPossibleAttacks(actions);
            }
            else
            {
                FindAttacksToSaveGeneral(actions);
                FindMovesToSaveGeneral(actions);
            }
        }

        /// <summary>
        /// Finds actions that lead to a win. Loop through all action that opponent can counter, if after all the move
        /// their General is still threatened. It results in a checkmate.
        /// </summary>
        /// <param name="actions">A list of Action objects representing the possible actions.</param>
        public void FindWinningActions(ref List<Action> actions)
        {
            List<Action> winningActions = new List<Action>();

            if (actions.Count == 0) return;

            foreach (var move in actions)
            {
                move.DoAction();

                // Check if this player checkmates the opponent
                if (IsCheckMateOpponent())
                {
                    winningActions.Add(move);
                }

                // Move the piece back to its original square
                move.UndoAction();
            }
            if (winningActions.Count > 0)
            {
                actions = winningActions;
            }
        }

        /// <summary>
        /// Finds all possible moves for the player's pieces.
        /// </summary>
        /// <param name="actions">A list of Action objects representing the possible actions.</param>
        public void FindPossibleMoves(List<Action> actions)
        {
            foreach (var piece in Army.Pieces)
            {
                if (!piece.OnBoard) continue;
                foreach (var square in Game.Board.Squares)
                {
                    if (square.IsFree && piece.CanMoveTo(square))
                    {
                        actions.Add(new Move(piece, square));
                    }
                    // Jester's swap move
                    else if (square.IsOccupied && piece.CanMoveTo(square) && piece is Jester)
                    {
                        actions.Add(new Move(piece, square));
                    }
                }
            }
        }

        /// <summary>
        /// Finds all possible attacks for the player's pieces.
        /// </summary>
        /// <param name="actions">A list of Action objects representing the possible actions.</param>
        public void FindPossibleAttacks(List<Action> actions)
        {
            foreach (var piece in Army.Pieces)
            {
                if (!piece.OnBoard) continue;
                foreach (var square in Game.Board.Squares)
                {
                    // For regular piece
                    if (square.IsOccupied
                        && !IsProtectedByEnemySentinel(piece, square)
                        && piece.RequiresEnemyToAttack
                        && square.Occupant.Player != this
                        && square.Occupant.Player != null
                        && piece.CanAttack(square)
                    )
                    {
                        actions.Add(new Attack(piece, square));
                    }
                    // If piece is a Builder
                    else if (!piece.RequiresEnemyToAttack && piece.CanAttack(square))
                    {
                        // Builder Attacking regular piece
                        if (square.IsOccupied
                            && !IsProtectedByEnemySentinel(piece, square)
                            && !square.ContainsWall
                            && square.Occupant.Player != this)
                        {
                            actions.Add(new Attack(piece, square));
                        }
                        // Builder building a wall
                        else if (square.IsFree)
                        {
                            actions.Add(new BuildWall(piece, square));
                        }
                    }

                    // Miner destroying wall
                    else if (square.IsOccupied
                        && piece is Miner
                        && square.ContainsWall
                        && piece.CanAttack(square))
                    {
                        actions.Add(new DestroyWall(piece, square));
                    }
                }
            }
        }

        // <summary>
        /// Finds moves that can save the player's General.
        /// </summary>
        /// <param name="actions">A list of Action objects representing the possible actions.</param>
        public void FindMovesToSaveGeneral(List<Action> actions)
        {
            List<Action> possibleMoves = new List<Action>();
            FindPossibleMoves(possibleMoves);

            // No possible attacks could save the general
            if (possibleMoves.Count == 0) return;

            foreach (var move in possibleMoves)
            {
                move.DoAction();

                // Check if the General would be safe after the move
                if (IsGeneralSafe())
                {
                    // If the move makes the General safe, add it to the possible actions
                    actions.Add(move);
                }

                // Move the piece back to its original square
                move.UndoAction();
            }
        }

        /// <summary>
        /// Finds attacks that can save the player's General.
        /// </summary>
        /// <param name="actions">A list of Action objects representing the possible actions.</param>
        public void FindAttacksToSaveGeneral(List<Action> actions)
        {
            List<Action> possibleAttacks = new List<Action>();
            FindPossibleAttacks(possibleAttacks);

            // No possible attacks could save the general
            if (possibleAttacks.Count == 0) return;

            foreach (var attack in possibleAttacks)
            {
                attack.DoAction();

                // Check if the General would be safe after the move
                if (IsGeneralSafe())
                {

                    // If the move makes the General safe, add it to the possible actions
                    actions.Add(attack);
                }

                // Move the piece back to its original square
                attack.UndoAction();
            }
        }

        /// <summary>
        /// Determines whether a square is protected by an enemy Sentinel.
        /// </summary>
        /// <param name="actor">The Piece attempting to move or attack.</param>
        /// <param name="targetSquare">The Square being targeted.</param>
        /// <returns>True if the square is protected by an enemy Sentinel, otherwise false.</returns>
        public bool IsProtectedByEnemySentinel(Piece actor, Square targetSquare)
        {
            if (targetSquare.Occupant is Sentinel || (actor is Jester)) return false;

            foreach (var piece in Opponent.Army.Pieces)
            {
                if (piece is Sentinel && piece.OnBoard)
                {
                    var sentinel = piece as Sentinel;
                    if (sentinel.IsProtected(targetSquare))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if the player's General is safe.
        /// </summary>
        /// <returns>True if the General is safe, otherwise false.</returns>
        public bool IsGeneralSafe()
        {
            General general = Army.Pieces.OfType<General>().FirstOrDefault();
            if (general == null) throw new Exception($"{this} does not have general in their army");

            var sentinels = Army.Pieces.OfType<Sentinel>().Where(piece => piece.OnBoard).ToList();
            bool isProtected = sentinels.Any(sentinel => sentinel.IsProtected(general.Square));

            foreach (var piece in Opponent.Army.Pieces)
            {
                // If an opponent's piece can attack the General and the General is not protected, it's not safe
                if (piece.Square != null && piece.CanAttack(general.Square) && !isProtected)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines if the opponent is in a checkmate situation.
        /// </summary>
        /// <returns>True if the opponent is in checkmate, otherwise false.</returns>
        public bool IsCheckMateOpponent()
        {
            bool success = true;
            if (!Opponent.IsGeneralSafe())
            {
                List<Action> rescueActions = new List<Action>();
                Opponent.FindPossibleActions(rescueActions);
                if (rescueActions.Count > 0)
                {
                    success = false;
                }

            }
            return success;
        }

        /// <summary>
        /// Evaluates the game state from the player's perspective.
        /// </summary>
        /// <returns>An integer representing the game state. Higher values are better for the player.</returns>
        public int EvaluateGame()
        {
            Dictionary<string, int> pieceValues = new Dictionary<string, int>
        {
            { "Zombie", 1 },
            { "Builder", 2 },
            { "Jester", 3 },
            { "Miner", 4 },
            { "Sentinel", 5 },
            { "Catapult", 6 },
            { "Dragon", 7 },
            { "General", 1000 } 
        };

            int material = 0;


            foreach (var piece in Army.Pieces)
            {
                if (!piece.OnBoard) continue;

                string pieceType = piece.GetType().Name;

                if (pieceValues.ContainsKey(pieceType))
                {
                    material += pieceValues[pieceType];
                }
            }

            // If the player for which we're evaluating the game state is the black player, we return 
            // the negative of the material.
            if (Colour == Colour.Black)
            {
                return -material;
            }

            return material;

        }

    }
}