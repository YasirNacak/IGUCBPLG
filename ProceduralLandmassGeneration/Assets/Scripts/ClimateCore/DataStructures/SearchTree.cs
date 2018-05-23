public interface SearchTree<E>
{
    bool add(E item);
    bool contains(E target);
    E find(E target);
    E delete(E target);
    bool remove(E target);
}
