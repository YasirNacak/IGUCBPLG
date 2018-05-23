using UnityEngine;

public class LivingThingComponent : MonoBehaviour {
    private LivingThing livingThing;
    public int id;

    void Start()
    {
        livingThing = World.getDatabase().find(new Animal(id));
    }

    private void OnMouseDown()
    {
        Debug.Log(livingThing.getName());
        Debug.Log(livingThing.getDefinition());
        if (!World.getExplored().Contains(livingThing))
        {
            World.getExplored().Add(livingThing);
        }
    }
}
