using System.Collections;
using System.Collections.Generic;

using UnityEngine.Events;/*for UnityAction*/
using UnityEngine.EventSystems; /*events button*/
using UnityEngine.UI; /*for image*/
using UnityEngine;
using TMPro;

/*abstract class used by Dynamic and Static interface!*/
public abstract class UserInterface : MonoBehaviour
{
    /*to be changedd later*/
    public Animator_Player player;

    public InventoryObject inventory;

    public Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            /*linking items in the iventory to function as parent, 
            so we can see if item exists in the inventory and who it belongs to*/
            inventory.Container.Items[i].parent = this;
        }
        CreateSlots();
    }

    // Update is called once per frame
    void Update()
    {
        /*use notify desing pattern, to be changed later*/
        UpdateSlots();
    }

    /*this method creates the slots on the inv screen*/
    public abstract void CreateSlots();

    public void UpdateSlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in itemsDisplayed)
        {
            if (_slot.Value.ID >= 0)
            {
                // Debug.Log("Child added");

                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[_slot.Value.item.Id].uiDisplay;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
                // _slot.Key.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");

            }
            else
            {
                // Debug.Log("Child not added");

                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        /* this method is used to add events each time we want to create one*/

        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    /*methods to decide what to do when we press on object*/
    public void OnEnter(GameObject obj)
    {
        /*this method checks that the object we clikc really exists, 
        and if so to keep a refrence to it, so it wont corrupt*/
        player.mouseItem.hoverObj = obj;
        if (itemsDisplayed.ContainsKey(obj))
        {
            player.mouseItem.hoverItem = itemsDisplayed[obj];
        }
    }
    public void OnExit(GameObject obj)
    {
        /*reset the on enter method*/
        player.mouseItem.hoverObj = null;
        player.mouseItem.hoverItem = null;

    }
    public void OnDragStart(GameObject obj)
    {
        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(50, 50); /*50,50 -> the size of the object we drag*/
        mouseObject.transform.SetParent(transform.parent);

        if (itemsDisplayed[obj].ID >= 0)
        {
            /*if there's an item inside the invenotry,
             add a image component and get the ui display of what we drag so we can place it correctly*/
            var img = mouseObject.AddComponent<Image>();
            img.sprite = inventory.database.GetItem[itemsDisplayed[obj].ID].uiDisplay;
            img.raycastTarget = false; /*disable the raycasting of the mouse "ignore the item"*/
        }
        player.mouseItem.obj = mouseObject;
        player.mouseItem.item = itemsDisplayed[obj];

    }
    public void OnDragEnd(GameObject obj)
    {
        var itemOnMouse = player.mouseItem;
        var mouseHoverItem = itemOnMouse.hoverItem;
        var mouseHoverObj = itemOnMouse.hoverObj;
        var GetItemObject = inventory.database.GetItem;

        if (mouseHoverObj)
        {
            if (mouseHoverItem.CanPlaceInSlot(GetItemObject[itemsDisplayed[obj].ID]))
            {
                inventory.MoveItem(itemsDisplayed[obj], mouseHoverItem.parent.itemsDisplayed[itemOnMouse.hoverObj]);
            }
        }
        else
        {
            /*this removes item if not placed in slot*/
            // inventory.RemoveItem(itemsDisplayed[obj].item);
        }
        /*making sure item wont stick to screen*/
        Destroy(itemOnMouse.obj);
        itemOnMouse.item = null;

    }
    public void OnDrag(GameObject obj)
    {
        /*this method checks if where clicking on an item, 
        if so, grab the item according to the mouse location*/
        if (player.mouseItem.obj != null)
        {
            player.mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }


}

/*basic class to handle the item the mouse is pressing/hovering on
 might be changed later..*/
public class MouseItem
{
    public GameObject obj;
    public InventorySlot item;
    public InventorySlot hoverItem;
    public GameObject hoverObj;
}
