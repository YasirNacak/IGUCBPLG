using System.Collections.Generic;

public class HeapSort
{
    public static void Sort(List<LivingThing> table)
    {
        BuildHeap(table);
        ShrinkHeap(table);
    }

    private static void BuildHeap(List<LivingThing> table)
    {
        int n = 1;

        while (n < table.Count)
        {
            n++;
            int child = n - 1;
            int parent = (child - 1) / 2;
            while (parent >= 0 && table[parent].getName().CompareTo(table[child].getName()) < 0)
            {
                Swap(table, parent, child);
                child = parent;
                parent = (child - 1) / 2;
            }
        }
    }

    private static void ShrinkHeap(List<LivingThing> table)
    {
        int n = table.Count;

        while (n > 0)
        {
            n--;
            Swap(table, 0, n);
            int parent = 0;
            while (true)
            {
                int leftChild = 2 * parent + 1;
                if (leftChild >= n)
                {
                    break;
                }
                int rightChild = leftChild + 1;
                int maxChild = leftChild;
                if (rightChild < n // There is a right child.
                    && table[leftChild].getName().CompareTo(table[rightChild].getName()) < 0)
                {
                    maxChild = rightChild;
                }
                if (table[parent].getName().CompareTo(table[maxChild].getName()) < 0)
                {
                    Swap(table, parent, maxChild);
                    parent = maxChild;
                }
                else
                {
                    break;
                }
            }
        }
    }

    private static void Swap<T>(List<T> table, int i, int j)
    {
        T temp = table[i];
        table[i] = table[j];
        table[j] = temp;
    }

}