using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Pathfinding
{
    /**
    * Main class to find the best path from A to B.
    */
    public class Pathfinding
    {
        /**
         * The method that uses a grid of nodes to find the path
         * between startPos and targetPos
         * grid: grid to search in
         * startPos: starting position
         * targetPos: ending position
         */
        public static List<Point> FindPath(Grid grid, Point startPos, Point targetPos)
        {
            var nodesPath = _ImpFindPath(grid, startPos, targetPos);

            // convert path to a list of points and return
            var ret = new List<Point>();
            if (nodesPath == null) return ret;
            foreach (var node in nodesPath)
            {
                ret.Add(new Point(node.gridX, node.gridY));
            }
            return ret;
        }

        private static List<Node> _ImpFindPath(Grid grid, Point startPos, Point targetPos)
        {
            var startNode = grid.nodes[startPos.x, startPos.y];
            var targetNode = grid.nodes[targetPos.x, targetPos.y];

            var openSet = new List<Node>();
            var closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                var currentNode = openSet[0];
                for (var i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    return RetracePath(grid, startNode, targetNode);
                }

                foreach (var neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    var newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) * (int)(10.0f * neighbour.penalty);
                    if (newMovementCostToNeighbour >= neighbour.gCost && openSet.Contains(neighbour)) continue;
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }

            return null;
        }

        private static List<Node> RetracePath(Grid grid, Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Reverse();
            return path;
        }

        private static int GetDistance(Node nodeA, Node nodeB)
        {
            int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
            int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }
    }

}
