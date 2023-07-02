using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    /// <summary>
    /// Represents an AI player that makes intelligent moves in the game.
    /// </summary>
    public class AI
    {
        public const int MaxDepth = 3;
        static Random rand = new Random();
        private Game? Game;
        private List<Action>? Actions = new List<Action>();
        Player CurrentPlayer;


        /// <summary>
        /// Initializes a new instance of the <see cref="AI"/> class.
        /// </summary>
        /// <param name="game">The game object.</param>
        /// <param name="currentPlayer">The current player.</param>
        public AI(Game game, Player currentPlayer)
        {
            if (game == null) throw new Exception("No Game can be found.");
            Game = game;
            CurrentPlayer = currentPlayer;
        }

        /// <summary>
        /// Chooses the best action for the current player.
        /// </summary>
        /// <returns>The best action to take.</returns>
        public Action? ChooseBestAction()
        {
            // Level 4 functionality
            CurrentPlayer.FindPossibleActions(Actions);
            if (Actions.Count == 1)
            {
                return Actions[0];
            }
            if (Actions.Count == 0) throw new Exception("Current player is in stalemate position, there is no possible move.");

            // Level 5 functionality
            Action checkMateAction = GetCheckmateAction();
            if (checkMateAction != null)
            {
                return checkMateAction;
            }

            // Level 6 functionality
            List<Action>? advantageMoves = getMaterialAdvantageMove(CurrentPlayer);
            if (advantageMoves == null || advantageMoves.Count == 0)
            {
                return null;
            }
            else if (advantageMoves.Count == 1)
            {
                return advantageMoves[0];
            }


            // Level 7 functionality
            // Otherwise, we get more than one material-advantage-neutral actions
            List<Action> bestAction = new List<Action>();
            bool isMaximizingPlayer = CurrentPlayer.Colour == Colour.White;
            int bestScore = CurrentPlayer.Colour == Colour.White ? int.MinValue : int.MaxValue;

            foreach (var action in advantageMoves)
            {
                int score = Minimax(action, 0, int.MinValue, int.MaxValue, isMaximizingPlayer);
                // If the move results in checkmate for the white player, then skip it
                if (score == int.MinValue && CurrentPlayer.Colour == Colour.White)
                {
                    continue; // If the move results in checkmate for the white player, then skip it
                }
                else if (score == int.MaxValue && CurrentPlayer.Colour == Colour.Black)
                {
                    continue; // If the move results in checkmate for the black player, then skip it
                }

                //  Decide whether to add to best actions
                if ((CurrentPlayer.Colour == Colour.White
                && score > bestScore)
                ||
                (CurrentPlayer.Colour == Colour.Black
                && score < bestScore))
                {
                    bestScore = score;
                    bestAction.Clear();
                    bestAction.Add(action);
                }
                if (score == bestScore)
                {
                    bestAction.Add(action);
                }

                // Black is being checkmated by white
                if (score == int.MaxValue && CurrentPlayer.Colour == Colour.White)
                {
                    // If the move results in a checkmate for the opponent, prioritize it
                    bestAction.Clear();
                    bestAction.Add(action);
                    break; // Exit the loop, as checkmate is the best possible outcome
                }
                // White is being checkmated by black
                else if (score == int.MinValue && CurrentPlayer.Colour == Colour.Black)
                {
                    bestAction.Clear();
                    bestAction.Add(action);
                    break; 
                }

            }

            if (bestAction.Count() == 0)
                return null;
            else
                return bestAction[rand.Next(bestAction.Count())];

        }

        /// <summary>
        /// Performs the Minimax algorithm for AI decision-making in the game.
        /// </summary>
        /// <param name="action">The action to be evaluated.</param>
        /// <param name="depth">The depth of the game tree to be searched.</param>
        /// <param name="alpha">The best value that the maximizer currently can guarantee at this level or above.</param>
        /// <param name="beta">The best value that the minimizer currently can guarantee at this level or above.</param>
        /// <param name="isMaximizingPlayer">A boolean to check whether the current player is the maximizing player.</param>
        /// <returns>Returns the evaluation score of the action after performing the Minimax algorithm.</returns>
        public int Minimax(Action action, int depth, int alpha, int beta, bool isMaximizingPlayer)
        {
            Player CurrentPlayer = isMaximizingPlayer ? Game.White : Game.Black;
            if (depth == MaxDepth)
            {
                //List<Action> actions = new List<Action>();
                //CurrentPlayer.Opponent.FindAttacksToSaveGeneral(actions);
                //CurrentPlayer.Opponent.FindMovesToSaveGeneral(actions);
                //if (actions.Count == 0)
                //{ 
                //    return isMaximizingPlayer ? int.MaxValue : int.MinValue; // Assign very high or low score based on maximizing player
                //}

                return isMaximizingPlayer ? Game.White.EvaluateGame() : Game.Black.EvaluateGame(); // Otherwise, evaluate as usual

            }


            if (isMaximizingPlayer)
            {
                int maxEval = int.MinValue;
                List<Action> possibleActions = new List<Action>();
                possibleActions = getMaterialAdvantageMove(CurrentPlayer);
                if (possibleActions.Count == 0 || possibleActions == null)
                {
                    return int.MinValue;
                }

                foreach (var nextAction in possibleActions)
                {
                    nextAction.DoAction();
                    int eval = Minimax(nextAction, depth + 1, alpha, beta, false);
                    nextAction.UndoAction();
                    maxEval = Math.Max(maxEval, eval);
                    alpha = Math.Max(alpha, eval);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return maxEval;
            }
            else
            {
                int minEval = int.MaxValue;
                List<Action> possibleActions = new List<Action>();
                possibleActions = getMaterialAdvantageMove(CurrentPlayer);
                if (possibleActions.Count == 0 || possibleActions == null)
                {
                    return int.MaxValue;
                }
                foreach (var nextAction in possibleActions)
                {
                    nextAction.DoAction();
                    int eval = Minimax(nextAction, depth + 1, alpha, beta, true);
                    nextAction.UndoAction();
                    minEval = Math.Min(minEval, eval);
                    beta = Math.Min(beta, eval);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return minEval;
            }
        }

        /// <summary>
        /// Checks and returns an action that would lead to a checkmate situation.
        /// </summary>
        /// <returns>An Action that leads to a checkmate if one exists, null otherwise.</returns>
        public Action? GetCheckmateAction()
        {
            Player opponent = CurrentPlayer.Opponent;

            foreach (var proposedAction in Actions)
            {
                // Apply the proposed action on the cloned game
                proposedAction.DoAction();
                if (!CurrentPlayer.IsGeneralSafe())
                {
                    proposedAction.UndoAction();
                    continue; // skip this action as it puts player's general in danger
                }

                var opponentResponses = new List<Action>();
                opponent.FindPossibleActions(opponentResponses);

                bool isCheckMate = true;

                foreach (Action response in opponentResponses)
                {
                    response.DoAction();
                    if (opponent.IsGeneralSafe())
                    {
                        isCheckMate = false;
                        response.UndoAction();
                        break;
                    }
                    response.UndoAction();

                }

                proposedAction.UndoAction();

                if (isCheckMate)
                {
                    return proposedAction;
                }


            }
            return null;

        }

        /// <summary>
        /// Determines and returns the action(s) that would provide the current player a material advantage.
        /// </summary>
        /// <returns>A List of Actions that gives the current player a material advantage.</returns>
        private List<Action> getMaterialAdvantageMove()
        {
            int bestScore = int.MinValue;
            List<Action> actions = new List<Action>();
            List<Action> bestActions = new List<Action>();
            CurrentPlayer.FindPossibleMoves(actions);
            Dictionary<string, int> pieceValues = new Dictionary<string, int>
        {
            {"Wall", 0},
            { "Zombie", 1 },
            { "Builder", 2 },
            { "Jester", 3 },
            { "Miner", 4 },
            { "Sentinel", 5 },
            { "Catapult", 6 },
            { "Dragon", 7 },
            { "General", 1000 }
        };

            foreach (var action in Actions)
            {
                int oppPieceValue = 0;
                string pieceType = string.Empty;

                if (action is Attack)
                {
                    Piece p = action.Target.Occupant;
                    pieceType = p.GetType().Name;
                    oppPieceValue = pieceValues[pieceType];
                }

                if (action.DoAction())
                {

                    int score = CurrentPlayer.EvaluateGame();

                    if (action is Attack && action.Actor is Jester)
                    {
                        score += oppPieceValue * 10;
                    }
                    else
                    {
                        score += oppPieceValue;
                    }

                    Console.WriteLine(score);
                    action.UndoAction();

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestActions.Clear();
                    }
                    if (score == bestScore)
                    {
                        bestActions.Add(action);
                    }
                }
            }

            // Return a random action
            if (bestActions.Count() == 0)
                return null;
            else
                return bestActions;
        }

        /// <summary>
        /// Determines and returns the action(s) that would provide the specified player a material advantage.
        /// </summary>
        /// <param name="player">The player for whom to find the material advantage moves.</param>
        /// <returns>A List of Actions that gives the specified player a material advantage.</returns>
        private List<Action> getMaterialAdvantageMove(Player player)
        {
            int bestScore = int.MinValue;
            List<Action> actions = new List<Action>();
            List<Action> bestActions = new List<Action>();
            player.FindPossibleActions(actions);
            Dictionary<string, int> pieceValues = new Dictionary<string, int>
        {
            {"Wall", 0},
            { "Zombie", 1 },
            { "Builder", 2 },
            { "Jester", 3 },
            { "Miner", 4 },
            { "Sentinel", 5 },
            { "Catapult", 6 },
            { "Dragon", 7 },
            { "General", 1000 }
        };

            foreach (var action in actions)
            {
                int oppPieceValue = 0;
                string pieceType = string.Empty;

                if (action is Attack)
                {
                    Piece p = action.Target.Occupant;
                    pieceType = p.GetType().Name;
                    oppPieceValue = pieceValues[pieceType];
                }

                if (action.DoAction())
                {

                    int score = player.EvaluateGame();

                    if (action is Attack && action.Actor is Jester)
                    {
                        score += oppPieceValue * 10;
                    }
                    else
                    {
                        score += oppPieceValue;
                    }

                    Console.WriteLine(score);
                    action.UndoAction();

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestActions.Clear();
                    }
                    if (score == bestScore)
                    {
                        bestActions.Add(action);
                    }
                }
            }

            // Return a random action
            if (bestActions.Count() == 0)
                return null;
            else
                return bestActions;
        }


    }

}
