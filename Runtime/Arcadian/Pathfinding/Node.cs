using UnityEngine;

namespace Arcadian.Pathfinding
{
    /// <summary>
    /// A lightweight data container representing a single cell within the pathfinding grid, storing its world position, walkability, grid coordinates, and pathfinding costs used in A* calculations. Each node can also reference its parent node for path reconstruction.
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Is the node walkable?
        /// </summary>
        public readonly bool Walkable;

        /// <summary>
        /// Real-world location for this position.
        /// </summary>
        public readonly Vector3 WorldPosition;

        /// <summary>
        /// Horizontal coordinate within the path finder's grid.
        /// </summary>
        public readonly int GridX;

        /// <summary>
        /// Vertical coordinate within the path finder's grid.
        /// </summary>
        public readonly int GridY;

        /// <summary>
        /// Actual cost from start node, to this node.
        /// </summary>
        public int GCost;

        /// <summary>
        /// Heuristic estimate of the cheapest cost from this node, to the goal.
        /// </summary>
        public int HCost;

        /// <summary>
        /// The parent node of this, used in building paths.
        /// </summary>
        public Node Parent;

        /// <summary>
        /// Total cost of this node (sum of HCost and GCost).
        /// </summary>
        public int FCost => GCost + HCost;

        /// <summary>
        /// Setup the node with it's permanent values.
        /// </summary>
        /// <param name="walkable">Is the node walkable?</param>
        /// <param name="worldPos">Real-world location for this position.</param>
        /// <param name="gridX">Horizontal coordinate within the path finder's grid.</param>
        /// <param name="gridY">Vertical coordinate within the path finder's grid.</param>
        public Node(bool walkable, Vector3 worldPos, int gridX, int gridY)
        {
            Walkable = walkable;
            WorldPosition = worldPos;
            GridX = gridX;
            GridY = gridY;
        }
    }
}