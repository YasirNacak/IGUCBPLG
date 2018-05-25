using UnityEngine;
using UnityEngine.UI;

public class LivingThingComponent : MonoBehaviour
{
    private LivingThing livingThing;
    public int id;

    void Start()
    {
        livingThing = World.getDatabase().find(new Animal(id));
    }

    private void OnMouseDown()
    {
        string name = livingThing.getName();
        string description = livingThing.getDefinition();
        GameObject.Find("PopupName").GetComponent<Text>().text = name;
        GameObject.Find("PopupDescription").GetComponent<Text>().text = description;
        if (!World.getExplored().Contains(livingThing))
        {
            World.getExplored().Add(livingThing);
        }
    }
}
