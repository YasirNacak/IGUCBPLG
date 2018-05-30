using UnityEngine;
using UnityEngine.UI;

public class EntityInfo : MonoBehaviour
{
    public Button livingThingButton;
    public Text theNameOfThing;
    private LivingThing thing;
    private const int ITEM_LIMIT = 5;

    public void Setup(LivingThing currentItem)
    {
        thing = currentItem;
        theNameOfThing.text = thing.getName();
        livingThingButton.onClick.AddListener( delegate { EntityOnClick(thing); });
    }
    
    public void EntityOnClick(LivingThing thing)
    {
        EntityScrollList.Definition.SetActive(true);
        EntityScrollList.Header.SetActive(true);
        EntityScrollList.NothingSelected.SetActive(false);
        GameObject.Find("EntityName").GetComponent<Text>().text = thing.getName();
        GameObject.Find("EntityDef").GetComponent<Text>().text = thing.getDefinition();
    }

}
