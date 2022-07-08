using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

/*this class is dynamic given the to the Inventory Screen, 
it inherets from user interface because i needed to be able to have also the static interface which is linked
to the Equipment slot, that is static because no need to update it everytime, only when player decides*/
public class DynamicInterface : UserInterface
{   
    public GameObject inventoryPrefab;

    /*vars for positioning the items in the inventory*/
    public int X_START;
    public  int Y_START;
    public int X_SPACE_BETWEEN_ITEM;
    public int NUMBER_OF_COLUMN;
    public int Y_SPACE_BETWEEN_ITEMS;
    public override void CreateSlots()
    {
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>(); /*making sure dictionary is init*/

        for (int i = 0; i < inventory.Container.Items.Length; ++i)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            /*adding each slot to function as a type event
            thos 5 lines of codes that will add the events will function as functions to what to do when we click on an item.
            delegate - allows transferings variables to a callback function!*/
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            itemsDisplayed.Add(obj, inventory.Container.Items[i]);
        }
    }

    private Vector3 GetPosition(int i)
    {
        /*this will return a vector3 to the right position in the screen display for the objects*/
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUMN)), 0f);
    }

}
