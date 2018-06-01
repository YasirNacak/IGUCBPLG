using System.Collections.Generic;

namespace Assets.Scripts.Pathfinding
{
    /**
    * The class that holds grid of nodes we use to find path
    */
    public class Grid
    {
        public Node[,] nodes;
        int gridSizeX, gridSizeY;

        /**
        * Create a new grid with tile prices
        * width: grid width
        * height: grid height
        * tiles_costs: 2d array of floats, representing the cost of every tile
        */
        public Grid(int width, int height, float[,] tiles_costs)
        {
            gridSizeX = width;
            gridSizeY = height;
            nodes = new Node[width, height];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var cost = tiles_costs[x, y];
                    var node = new Node(cost, x, y);
                    if (cost == 0.0f)
                    {
                        node.walkable = true;
                    }
                    nodes[x, y] = node;

                }
            }
        }

        /**
        * Create a new grid of just walkable / unwalkable
        * width: grid width
        * height: grid height
        * walkable_tiles: the tilemap. true for walkable, false for blocking
        */
        public Grid(int width, int height, bool[,] walkableTiles)
        {
            gridSizeX = width;
            gridSizeY = height;
            nodes = new Node[width, height];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    nodes[x, y] = new Node(walkableTiles[x, y] ? 1.0f : 0.0f, x, y);
                }
            }
        }

        /**
         * Find all the neighbours of a node and return
         * it as an array of nodes
         */
        public List<Node> GetNeighbours(Node node)
        {
            var neighbours = new List<Node>();

            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    var checkX = node.gridX + x;
                    var checkY = node.gridY + y;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        neighbours.Add(nodes[checkX, checkY]);
                    }
                }
            }
            return neighbours;
        }
    }
}