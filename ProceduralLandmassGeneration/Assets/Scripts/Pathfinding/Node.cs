namespace Assets.Scripts.Pathfinding
{
    /**
    * A node in the grid map
    */
    public class Node
    {
        /**
         * Is the node walkable
         */
        public bool walkable;

        /**
         * X axis coordinate of the node
         */
        public int gridX;

        /**
         * Y axis coordinate of the node
         */
        public int gridY;

        /**
         * Price of the node
         */
        public float penalty;

        /**
         * Cost variables
         */
        public int gCost;
        public int hCost;

        /**
         * The node that is used before this node
         */
        public Node parent;

        /**
         * Create a new node
         * _price: how much does it cost to pass this tile. less is better, but 0.0f is for non-walkable.
         * _gridX, _gridY: tile location in grid.
         */
        public Node(float price, int gridX, int gridY)
        {
            walkable = price != 0.0f;
            penalty = price;
            this.gridX = gridX;
            this.gridY = gridY;
        }

        /**
         * returns: sum of gCost and hCost
         */
        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }
    }
}