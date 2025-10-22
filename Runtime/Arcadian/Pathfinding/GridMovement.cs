using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Arcadian.Pathfinding
{
    /// <summary>
    /// A Unity component that moves an object smoothly along a series of <c>Node</c> positions, providing movement speed control, current velocity tracking, and an event callback when the final target node is reached. Ideal for pathfinding agents or grid-based movement systems.
    /// </summary>
    [ExecuteAlways, AddComponentMenu("Arcadian/Pathfinding/Grid Movement")]
    public class GridMovement : MonoBehaviour
    {
        /// <summary>
        /// Invoked when the target node has been reached.
        /// </summary>
        public event Action TargetReached;

        /// <summary>
        /// Speed to move the object (units/s).
        /// </summary>
        [Tooltip("Speed to move the object (units/s)."), BoxGroup("Settings")]
        public float speed = 2;
        
        /// <summary>
        /// Is the object currently moving?
        /// </summary>
        public bool IsMoving { private set; get; }

        /// <summary>
        /// What is the current velocity of the object?
        /// </summary>
        public Vector2 Velocity { private set; get; }

        /// <summary>
        /// What was the last velocity of the object?
        /// </summary>
        public Vector2 LastVelocity { private set; get; }

        /// <summary>
        /// Set and start the path traversal.
        /// </summary>
        /// <param name="path">Path of nodes to iterate over.</param>
        public void SetPath(IEnumerable<Node> path)
        {
            StartCoroutine(Move(path));
        }
        
        private IEnumerator Move(IEnumerable<Node> path)
        {
            IsMoving = true;
            
            foreach (var node in path)
            {
                yield return MoveToNode(node);
            }

            IsMoving = false;
            Velocity = Vector2.zero;
            
            TargetReached?.Invoke();
        }

        private IEnumerator MoveToNode(Node node)
        {
            Velocity = (node.WorldPosition - transform.position).normalized;
            LastVelocity = Velocity;

            while (Vector3.Magnitude(transform.position - node.WorldPosition) > 0.0125f)
            {
                transform.position += new Vector3(Velocity.x, Velocity.y, 0) * (speed * Time.deltaTime);

                yield return null;
            }

            transform.position = node.WorldPosition;
        }
    }
}