using UnityEngine;
using UnityEngine.UI;

public class EntityInfo : MonoBehaviour
{
    public Button livingThingButton;
    public Text theNameOfThing;

    private LivingThing thing;
    private EntityScrollList list;

	public void Setup (LivingThing currentItem, EntityScrollList entityList)
    {
        thing = currentItem;
        list = entityList;
        theNameOfThing.text = thing.getName();
	}
}
