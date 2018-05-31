using System;

public class AVLTree<E> :  BinarySearchTree < E > where E : IComparable<E>
{
  private bool increase;
  private bool decrease;
    private const int LEFT_HEAVY = -1;

    private const int BALANCED = 0;

    private const int RIGHT_HEAVY = 1;


    protected class AVLNode<E> : Node<E> {

    internal int balance;

    public AVLNode(E item) : base(item) {
      balance = BALANCED;
    }

    public override string ToString() {
      return balance + ": " + base.ToString();
    }
  }
  
  	protected Node<E> rotateRight(Node<E> root) {
		Node<E> temp = root.left;
		root.left = temp.right;
		temp.right = root;
		return temp;
	}

 
	protected Node<E> rotateLeft(Node<E> localRoot) {
	 Node<E> temp = localRoot.right;
	 localRoot.right = temp.left;
	 temp.left = localRoot;
	 return temp;
	}
  
  
  public bool add(E item) {
    increase = false;
    root = add( (AVLNode < E > ) root, item);
    return addReturn;
  }

  private AVLNode < E > add(AVLNode < E > localRoot, E item) {
    if (localRoot == null) {
      addReturn = true;
      increase = true;
      return new AVLNode < E > (item);
    }

    if (item.CompareTo(localRoot.data) == 0) {
      increase = false;
      addReturn = false;
      return localRoot;
    }

    else if (item.CompareTo(localRoot.data) < 0) {
      localRoot.left = add( (AVLNode < E > ) localRoot.left, item);

      if (increase) {
        decrementBalance(localRoot);
        if (localRoot.balance < LEFT_HEAVY) {
          increase = false;
          return rebalanceLeft(localRoot);
        }
      }
      return localRoot; // Rebalance not needed.
    }
    else {
      // item > data
      localRoot.right = add( (AVLNode < E > ) localRoot.right, item);
      if (increase) {
        incrementBalance(localRoot);
        if (localRoot.balance > RIGHT_HEAVY) {
          return rebalanceRight(localRoot);
        }
        else {
          return localRoot;
        }
      }
      else {
        return localRoot;
      }
    }

  }

  public E delete(E item) {
    decrease = false;
    root = delete( (AVLNode < E > ) root, item);
    return deleteReturn;
  }


  private AVLNode < E > delete(AVLNode < E > localRoot, E item) {
    if (localRoot == null) { // item is not in tree
      deleteReturn = default(E);
      return localRoot;
    }
    if (item.CompareTo(localRoot.data) == 0) {
      // item is in the tree -- need to remove it
      deleteReturn = localRoot.data;
      return findReplacementNode(localRoot);
    }
    else if (item.CompareTo(localRoot.data) < 0) {
      // item is < localRoot.data
      localRoot.left = delete( (AVLNode < E > ) localRoot.left, item);
      if (decrease) {
        incrementBalance(localRoot);
        if (localRoot.balance > RIGHT_HEAVY) {
          return rebalanceRightL( (AVLNode < E > ) localRoot);
        }
        else {
          return localRoot;
        }
      }
      else {
        return localRoot;
      }
    }
    else {
      // item is > localRoot.data
      localRoot.right = delete( (AVLNode < E > ) localRoot.right, item);
      if (decrease) {
        decrementBalance(localRoot);
        if (localRoot.balance < LEFT_HEAVY) {
          return rebalanceLeftR(localRoot);
        }
        else {
          return localRoot;
        }
      }
      else {
        return localRoot;
      }
    }
  }

  private AVLNode < E > findReplacementNode(AVLNode < E > node) {
    if (node.left == null) {
      decrease = true;
      return (AVLNode < E > ) node.right;
    }
    else if (node.right == null) {
      decrease = true;
      return (AVLNode < E > ) node.left;
    }
    else {
      if (node.left.right == null) {
        node.data = node.left.data;
        node.left = node.left.left;
        incrementBalance(node);
        return node;
      }
      else {
        node.data = findLargestChild( (AVLNode < E > ) node.left);
        if ( ( (AVLNode < E > ) node.left).balance < LEFT_HEAVY) {
          node.left = rebalanceLeft( (AVLNode < E > ) node.left);
        }
        if (decrease) {
          incrementBalance(node);
        }
        return node;
      }
    }
  }

  private E findLargestChild(AVLNode < E > parent) {
    if (parent.right.right == null) {
      E returnValue = parent.right.data;
      parent.right = parent.right.left;
      decrementBalance(parent);
      return returnValue;
    }
    else {
      E returnValue = findLargestChild( (AVLNode < E > ) parent.right);
      if ( ( (AVLNode < E > ) parent.right).balance < LEFT_HEAVY) {
        parent.right = rebalanceLeft( (AVLNode < E > ) parent.right);
      }
      if (decrease) {
        decrementBalance(parent);
      }
      return returnValue;
    }
  }

  private void incrementBalance(AVLNode < E > node) {
    node.balance++;
    if (node.balance > BALANCED) {
      increase = true;
      decrease = false;
    }
    else {
      increase = false;
      decrease = true;
    }
  }

  private AVLNode < E > rebalanceRight(AVLNode < E > localRoot) {
    // Obtain reference to right child
    AVLNode < E > rightChild = (AVLNode < E > ) localRoot.right;
    // See if right-left heavy
    if (rightChild.balance < BALANCED) {
      // Obtain reference to right-left child
      AVLNode < E > rightLeftChild = (AVLNode < E > ) rightChild.left;
      /* Adjust the balances to be their new values after
         the rotates are performed.
       */
      if (rightLeftChild.balance > BALANCED) {
        rightChild.balance = BALANCED;
        rightLeftChild.balance = BALANCED;
        localRoot.balance = LEFT_HEAVY;
      }
      else if (rightLeftChild.balance < BALANCED) {
        rightChild.balance = RIGHT_HEAVY;
        rightLeftChild.balance = BALANCED;
        localRoot.balance = BALANCED;
      }
      else {
        rightChild.balance = BALANCED;
        rightLeftChild.balance = BALANCED;
        localRoot.balance = BALANCED;
      }

      increase = false;
      decrease = true;
      localRoot.right = rotateRight(rightChild);
      return (AVLNode < E > ) rotateLeft(localRoot);
    }
    else {

      rightChild.balance = BALANCED;
      localRoot.balance = BALANCED;
      increase = false;
      decrease = true;
      return (AVLNode < E > ) rotateLeft(localRoot);
    }
  }

  private AVLNode < E > rebalanceLeftR(AVLNode < E > localRoot) {
    AVLNode < E > leftChild = (AVLNode < E > ) localRoot.left;
    if (leftChild.balance > BALANCED) {
      AVLNode < E > leftRightChild = (AVLNode < E > ) leftChild.right;
      if (leftRightChild.balance < BALANCED) {
        leftChild.balance = LEFT_HEAVY;
        leftRightChild.balance = BALANCED;
        localRoot.balance = BALANCED;
      }
      else if (leftRightChild.balance > BALANCED) {
        leftChild.balance = BALANCED;
        leftRightChild.balance = BALANCED;
        localRoot.balance = RIGHT_HEAVY;
      }
      else {
        leftChild.balance = BALANCED;
        localRoot.balance = BALANCED;
      }

      increase = false;
      decrease = true;
      localRoot.left = rotateLeft(leftChild);
      return (AVLNode < E > ) rotateRight(localRoot);
    }
    if (leftChild.balance < BALANCED) {
      leftChild.balance = BALANCED;
      localRoot.balance = BALANCED;
      increase = false;
      decrease = true;
    }
    else {
      leftChild.balance = RIGHT_HEAVY;
      localRoot.balance = LEFT_HEAVY;
    }
    // Now rotate the
    return (AVLNode < E > ) rotateRight(localRoot);
  }

  private AVLNode < E > rebalanceRightL(AVLNode < E > localRoot) {
    AVLNode < E > rightChild = (AVLNode < E > ) localRoot.right;
    if (rightChild.balance < BALANCED) {
      AVLNode < E > rightLeftChild = (AVLNode < E > ) rightChild.left;

      if (rightLeftChild.balance > BALANCED) {
        rightChild.balance = RIGHT_HEAVY;
        rightLeftChild.balance = BALANCED;
        localRoot.balance = BALANCED;
      }
      else if (rightLeftChild.balance < BALANCED) {
        rightChild.balance = BALANCED;
        rightLeftChild.balance = BALANCED;
        localRoot.balance = LEFT_HEAVY;
      }
      else {
        rightChild.balance = BALANCED;
        localRoot.balance = BALANCED;
      }
      increase = false;
      decrease = true;
      localRoot.right = rotateRight(rightChild);
      return (AVLNode < E > ) rotateLeft(localRoot);
    }
    if (rightChild.balance > BALANCED) {

      rightChild.balance = BALANCED;
      localRoot.balance = BALANCED;
      increase = false;
      decrease = true;
    }
    else {

      rightChild.balance = LEFT_HEAVY;
      localRoot.balance = RIGHT_HEAVY;
    }
    return (AVLNode < E > ) rotateLeft(localRoot);
  }

  private AVLNode < E > rebalanceLeft(AVLNode < E > localRoot) {
    AVLNode < E > leftChild = (AVLNode < E > ) localRoot.left;
    if (leftChild.balance > BALANCED) {
      AVLNode < E > leftRightChild = (AVLNode < E > ) leftChild.right;

      if (leftRightChild.balance < BALANCED) {
        leftChild.balance = BALANCED;
        leftRightChild.balance = BALANCED;
        localRoot.balance = RIGHT_HEAVY;
      }
      else {
        leftChild.balance = LEFT_HEAVY;
        leftRightChild.balance = BALANCED;
        localRoot.balance = BALANCED;
      }
      // Perform left rotation.
      localRoot.left = rotateLeft(leftChild);
    }
    else { 

      leftChild.balance = BALANCED;
      localRoot.balance = BALANCED;
    }
    return (AVLNode < E > ) rotateRight(localRoot);
  }

  private void decrementBalance(AVLNode < E > node) {
    node.balance--;
    if (node.balance == BALANCED) {
      increase = false;
    }
  }

}
