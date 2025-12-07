# Pathfinding

> Click [here](../../../README.md#features) to go back.

## GridMovement

A Unity component that moves an object smoothly along a series of `Node` positions, providing movement speed control, current velocity tracking, and an event callback when the final target node is reached. Ideal for pathfinding agents or grid-based movement systems.

Example Usage:
```c#
using LucasWarwick02.UnityAssets;
using UnityEngine;

public class MovementExample : MonoBehaviour
{
    public GridPathfinder pathfinder;
    public GridMovement mover;
    public Transform target;

    void Start()
    {
        var path = pathfinder.FindPath(transform.position, target.position);
        mover.TargetReached += () => Debug.Log("Destination reached!");
        mover.SetPath(path);
    }
}
```

## GridPathfinder

A Unity component that generates a 2D grid of nodes for pathfinding, providing A*-based shortest path calculations, randomized path variation, and utility methods to query the closest walkable node, while also visualising the grid in the editor for debugging.

Example Usage:
```c#
using LucasWarwick02.UnityAssets;
using UnityEngine;

public class PathfinderExample : MonoBehaviour
{
    public GridPathfinder pathfinder;
    public Transform target;

    void Start()
    {
        // Find a path from current position to target
        var path = pathfinder.FindPath(transform.position, target.position);
        if (path != null)
        {
            foreach (var node in path)
                Debug.Log($"Path Node: {node.WorldPosition}");
        }

        // Get the closest valid node to a point
        var closestNode = pathfinder.GetClosestValidNode(new Vector3(3, 3, 0));
        Debug.Log($"Closest Walkable Node: {closestNode.WorldPosition}");
    }
}
```

## Node

A lightweight data container representing a single cell within the pathfinding grid, storing its world position, walkability, grid coordinates, and pathfinding costs used in A* calculations. Each node can also reference its parent node for path reconstruction.

Example Usage:
```c#
using LucasWarwick02.UnityAssets;
using UnityEngine;

public class NodeExample : MonoBehaviour
{
    void Start()
    {
        // Create a simple walkable node at (0, 0)
        var node = new Node(true, new Vector3(0, 0, 0), 0, 0);

        // Assign pathfinding values
        node.GCost = 5;
        node.HCost = 10;

        Debug.Log($"Node at {node.WorldPosition} has FCost: {node.FCost}");
    }
}
```