using System.Collections.Generic;

public class Deque<E>
{
    private LinkedList<E> listForDeque = new LinkedList<E>();
    
    public void OfferFirst(E element)
    {
        listForDeque.AddFirst(element);
    }

    public void OfferLast(E element)
    {
        listForDeque.AddLast(element);
    }

    public E PollLast()
    {
        E retVal = PeekLast();
        listForDeque.RemoveLast();
        return (retVal);
    }

    public E PollFirst()
    {
        E retVal = PeekFirst();
        listForDeque.RemoveFirst();
        return (retVal);
    }

    public E PeekFirst()
    {
        return (listForDeque.First.Value);
    }

    public E PeekLast()
    {
        return (listForDeque.Last.Value);
    }

    public int GetSize()
    {
        return (listForDeque.Count);
    }
}