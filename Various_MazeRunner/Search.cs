using System;
using System.Collections.Generic;
using System.Drawing;

namespace Various_MazeRunner
{
    /* Search finds a path from a Start node to an End node.
     * Finished remains false until a path is found,
     * or until all nodes have been checked.
     * Upon finding a successful path FoundNodes will be filled with the path.
     * Otherwise, FoundNodes count will be 0.  */
    public class Search
    {
        //Initializes Search
        public Search(Node[,] nodes, Node start, Node end, SearchAlgorithm searchAlgorithm)
        {
            //Set starting variables
            Start = start;
            End = end;
            SearchAlgorithm = searchAlgorithm;
            //Starting node should always be open, so add it to the OpenNodes list.
            OpenList.Add(start);
            //Set start node as in OpenNodes.
            start.InOpenNodes = true;
            //Apply Start F cost
            start.F = CalculateAStarH(start, end);
        }

        List<Node> OpenList = new List<Node>();
        //Start node, and End node. 
        private Node Start { get; }
        private Node End { get; }
        //Type of search to perform
        SearchAlgorithm SearchAlgorithm;
        //Is set to true when searching finishes.
        public bool Finished = false;
        /* List of nodes which are traversed, and aren't set to closed.
         * This list may contain nodes that have already been checked */
        //private List<Node> OpenNodes { get; set; } = new List<Node>();
        /* Index in which to read from OpenNodes. This is more efficient than
         * removing at index 0 when we're done with a node. */
        private int OpenNodeReadIndex { get; set; }
        //FoundPath contains the node path from Start to End
        public List<Node> FoundPath { get; private set; } = new List<Node>();

        public void StepSearch()
        {
            /* If the read index is the same as open nodes count then 
             * all nodes have been traveled, and a path could not be found */
            if (OpenList.Count == 0)
            {
                //Indicate that search is finished and exit method.
                Finished = true;
                return;
            }

            //Get the current node available in our OpenNodes list.
            //Node node = BetterNodeArray.OpenNodes[BetterNodeArray.GetOpenNodesReadIndex()];
            Node node = ReturnNextNode();
            //Update node visual color.
            node.NodeVisual.ColorNodeVisual(Color.Yellow);

            //If the current node is the End node, we have reached the end of our path.
            if (node == End)
            {
                //Build solution.
                BuildPath();
                //Indicate that search has finished.
                Finished = true;
                //Exit method.
                return;
            }

            ///* If StepSearch has made it this far we know that there are still potentially nodes left
            // * to check, and that the path hasn't successfully been found yet. */

            //Search for node's neighbors and add them to the OpenNodes list.
            switch (SearchAlgorithm)
            {
                case SearchAlgorithm.AStar:
                    FullAStar(node);
                    break;
                case SearchAlgorithm.BFS:
                    BFS(node);
                    break;
            }

        }

        //Returns next node to be checked.
        private Node ReturnNextNode()
        {
            switch (SearchAlgorithm)
            {
                case SearchAlgorithm.AStar:
                    return ReturnLowestAStarNode();
                case SearchAlgorithm.BFS:
                    return ReturnFirstNode();
            }
            return null;
        }
        //Returns node with lowest F cost in OpenList
        private Node ReturnLowestAStarNode()
        {
            int lowestGIndex = 0;

            for (int i = 0; i < OpenList.Count; i++)
            {
                if (OpenList[i].F < OpenList[lowestGIndex].F)
                    lowestGIndex = i;
            }
            Node node = OpenList[lowestGIndex];
            OpenList.RemoveAt(lowestGIndex);
            return node;
        }
        //Returns first node in OpenList
        private Node ReturnFirstNode()
        {
            Node node = OpenList[0];
            OpenList.RemoveAt(0);
            return node;
        }

        //Adds neighbors using the BFS algorithm.
        private void BFS(Node node)
        {
            /* Check each neighbor for current node.
             * If the neighbor hasn't been checked, and isn't already in the OpenNodes list
             * then add it to the OpenNodes list. */
            for (int i = 0; i < node.Neighbors.Count; i++)
            {
                Neighbor neighbor = node.Neighbors[i];
                /* If neighbor is already in the OpenNodes continue through
                 * the for loop skipping this neighbor */
                if (neighbor.Node.InOpenNodes)
                    continue;
                //Assign neighbors previous node to the node which discovered it.
                neighbor.Node.Previous = node;
                //Set neighbor as InOpenNodes true.
                neighbor.Node.InOpenNodes = true;
                //Add neighbor to OpenNodes list.
                OpenList.Add(neighbor.Node);
            }

        }

        //Add neighbors based on AStar algorithm.
        private void FullAStar(Node node)
        {
            int gParent = node.G;
            /* Check each neighbor for current node.
             * If the neighbor isn't already in the OpenNodes list
             * then add it to the OpenNodes list. */
            for (int i = 0; i < node.Neighbors.Count; i++)
            {
                Neighbor neighbor = node.Neighbors[i];
                /* Calculate the distance from this neighbor to start, and this neighbor to end.
                * This simulates H and G cost. */
                int g;
                if (neighbor.Diagonal)
                    g = gParent + 14;
                else
                    g = gParent + 10;
                int h = CalculateAStarH(neighbor.Node, End);
                int f = g + h;

                /*  If node is already in the open list determine if 
                 *  g is lower than stored G. If so update the neighbor's
                 *  G and F values as well parent. */
                if (neighbor.Node.InOpenNodes)
                {
                    //Update values if new data is a quicker route.
                    if (g < neighbor.Node.G)
                    {
                        neighbor.Node.G = g;
                        neighbor.Node.F = f;
                        neighbor.Node.Previous = node;
                    }
                    /* Since node has already been added to list
                     * continue onward after updating values. */
                    continue;
                }

                //Apply cost to neighbor.
                neighbor.Node.G = g;
                neighbor.Node.F = f;
                //Assign neighbors previous node to the node which discovered it.
                neighbor.Node.Previous = node;
                //Set neighbor as InOpenNodes true.
                neighbor.Node.InOpenNodes = true;
                //Color node as its being added to the list.
                neighbor.Node.NodeVisual.ColorNodeVisual(Color.Gray);
                OpenList.Add(neighbor.Node);

            }
        }

        //Calculates the H distance for A* pathfinding.
        private int CalculateAStarH(Node pointA, Node pointB)
        {
            int hor = Math.Abs(pointA.Coordinates.Column - pointB.Coordinates.Column);
            int ver = Math.Abs(pointA.Coordinates.Row - pointB.Coordinates.Row);
            return (hor + ver) * 10;
        }

        /* Only called when a path is succesfully found. BuildPath
         * will build the path from End to Start */
        private void BuildPath()
        {
            //If the path is behing build the End node has been reached. Set End as the starting node to build backwards from.
            Node node = End;
            //place closest node of dest in list and each previous node to it, and so on
            while (node != null)
            {
                /* Add current node to FoundPath, then set it's previous
                 * node (the open node which discovered current node) as the next node to check.
                 * Eventually node.Previous will be null; this will occur on the Start node. Once we
                 * see that node.Previous is null we know that we are at the start, and found path is built, in reverse. */
                FoundPath.Add(node);
                //set next node to check as nodes previous.
                node = node.Previous;
            }
            //Reverse the path to display nodes from Start to End, rather than End to Start. (optional).
            //FoundPath.Reverse();
        }


    }
}
