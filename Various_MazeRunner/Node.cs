//Used to make list.
using System.Collections.Generic;
using System.Drawing;

namespace Various_MazeRunner
{
    public class Node
    {
        //Construct node
        public Node(Coordinates coordinates, bool closed, NodeVisual nodeVisual)
        {
            //Coordinates in grid.
            Coordinates = coordinates;
            //Is node closed
            Closed = closed;
            //Visual class responsible for coloring node at coordinates.
            NodeVisual = nodeVisual;
        }

        //H and G cost of node. Used in A*.
        public int G;
        public int F;
        //If Closed is true the Node may not be traveled on, nor have neighbors.
        public bool Closed { get; }
        //Coordinates where node exist within a 2D array.
        public Coordinates Coordinates { get; }
        //Previous holds a reference to the node which has discovered this node.
        public Node Previous = null;
        //If InOpenNodes is true, this node has already been added to the OpenNodes list in Search.
        public bool InOpenNodes = false;
        //NodeVisual reference for this node.
        public NodeVisual NodeVisual { get; }
        //Neighbors holds references to all nodes touching this node, which are not closed.
        public List<Neighbor> Neighbors { get; private set; } = new List<Neighbor>();
        public void AddNeighbor(Neighbor neighbor)
        {
            Neighbors.Add(neighbor);
        }

        /* Resets the node to an unaltered state, but
         * retains whether or not the node was closed at creation */
        public void Reset()
        {
            //Unset any values which could have been altered during the search.
            InOpenNodes = false;
            Previous = null;
            F = 0;
            G = 0;
        }

    }
}
