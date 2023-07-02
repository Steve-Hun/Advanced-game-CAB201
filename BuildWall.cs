using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    /// <summary>
    /// Represents an action to build a wall on a square.
    /// </summary>
    internal class BuildWall : Action
    {
        private Wall? builtWall;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildWall"/> class.
        /// </summary>
        /// <param name="actor">The piece performing the action.</param>
        /// <param name="target">The target square to build the wall on.</param>
        public BuildWall(Piece actor, Square target) : base(actor, target)
        {
        }

        /// <summary>
        /// Performs the build wall action.
        /// </summary>
        /// <returns><c>true</c> if the action was successful; otherwise, <c>false</c>.</returns>
        public override bool DoAction()
        {
            if (Target.IsOccupied) throw new Exception("Cannot build a wall on occupied square.");
            builtWall = new Wall(null, Target);

            return true;
        }

        /// <summary>
        /// Undoes the build wall action.
        /// </summary>
        public override void UndoAction()
        {
            if (builtWall != null)
            {
                builtWall.LeaveBoard();
            }
        }
    }
}
