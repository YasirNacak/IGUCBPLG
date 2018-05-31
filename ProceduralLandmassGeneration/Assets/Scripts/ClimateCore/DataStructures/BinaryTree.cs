using System.Text;

public class BinaryTree<E>
{
    /** Class to encapsulate a tree node. */
    protected class Node<E>
    {
        /** The information stored in this node. */
        internal E data;

        /** Reference to the left child. */
        internal Node<E> left;

        /** Reference to the right child. */
        internal Node<E> right;

        /** Construct a node with given data and no children.
            @param data The data to store in this node
            */
        public Node(E data)
        {
            this.data = data;
            left = null;
            right = null;
        }

        /** Return a string representation of the node.
            @return A string representation of the data fields
            */
        public override string ToString()
        {
            return data.ToString();
        }
    }

    // Data Field
    /** The root of the binary tree */
    protected Node<E> root;

    public BinaryTree()
    {
        root = null;
    }

    protected BinaryTree(Node<E> root)
    {
        this.root = root;
    }

    /** Constructs a new binary tree with data in its root,leftTree
        as its left subtree and rightTree as its right subtree.
        */
    public BinaryTree(E data, BinaryTree<E> leftTree,
                        BinaryTree<E> rightTree)
    {
        root = new Node<E>(data);
        if (leftTree != null)
        {
            root.left = leftTree.root;
        }
        else
        {
            root.left = null;
        }
        if (rightTree != null)
        {
            root.right = rightTree.root;
        }
        else
        {
            root.right = null;
        }
    }

    /** Return the left subtree.
        @return The left subtree or null if either the root or
        the left subtree is null
        */
    public BinaryTree<E> getLeftSubtree()
    {
        if (root != null && root.left != null)
        {
            return new BinaryTree<E>(root.left);
        }
        else
        {
            return null;
        }
    }

    /** Return the right sub-tree
            @return the right sub-tree or
            null if either the root or the
            right subtree is null.
        */
    public BinaryTree<E> getRightSubtree()
    {
        if (root != null && root.right != null)
        {
            return new BinaryTree<E>(root.right);
        }
        else
        {
            return null;
        }
    }

    public E getData()
    {
        if (root != null)
        {
            return root.data;
        }
        else
        {
            return default(E);
        }
    }

    public bool isLeaf()
    {
        return (root.left == null && root.right == null);
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        preOrderTraverse(root, 1, sb);
        return sb.ToString();
    }

    private void preOrderTraverse(Node<E> node, int depth,
                                    StringBuilder sb)
    {
        for (int i = 1; i < depth; i++)
        {
            sb.Append("  ");
        }
        if (node == null)
        {
            sb.Append("null\n");
        }
        else
        {
            sb.Append(node.ToString());
            sb.Append("\n");
            preOrderTraverse(node.left, depth + 1, sb);
            preOrderTraverse(node.right, depth + 1, sb);
        }
    }

}
