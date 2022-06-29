using System.Collections;
using System.Collections.Generic;

using UnityEngine.Events;/*for UnityAction*/
using UnityEngine.EventSystems; /*events button*/
using UnityEngine.UI; /*for image*/
using UnityEngine;
using TMPro;

public class DisplayInventory : MonoBehaviour
{
    public MouseItem mouseItem = new MouseItem();
    public GameObject inventoryPrefab;
    [SerializeField] private InventoryObject inventory;

    [SerializeField] private int X_START;
    [SerializeField] private int Y_START;
    [SerializeField] private int X_SPACE_BETWEEN_ITEM;
    [SerializeField] private int NUMBER_OF_COLUMN;
    [SerializeField] private int Y_SPACE_BETWEEN_ITEMS;
    Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    // Start is called before the first frame update
    void Start()
    {
        CreateSlots();
    }

    // Update is called once per frame
    void Update()
    {
        /*use notify desing pattern, to be changed later*/
        UpdateSlots();
    }

    public void CreateSlots()
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

    public void UpdateSlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in itemsDisplayed)
        {
            if (_slot.Value.ID >= 0)
            {
                Debug.Log("Child added");

                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[_slot.Value.item.Id].uiDisplay;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
                // _slot.Key.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");

            }
            else
            {
                Debug.Log("Child not added");

                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }

    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
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
        mouseItem.hoverObj = obj;
        if(itemsDisplayed.ContainsKey(obj))
        {
            mouseItem.hoverItem = itemsDisplayed[obj];
        }
    }
    public void OnExit(GameObject obj)
    {   
        /*reset the on enter method*/
        mouseItem.hoverObj = null;
        mouseItem.hoverItem = null;
        
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
        mouseItem.obj = mouseObject;
        mouseItem.item = itemsDisplayed[obj];

    }
    public void OnDragEnd(GameObject obj)
    {
        if(mouseItem.hoverObj)
        {
            inventory.MoveItem(itemsDisplayed[obj],itemsDisplayed[mouseItem.hoverObj]);
        }
        else
        {
            inventory.RemoveItem(itemsDisplayed[obj].item);
        }
        /*making sure item wont stick to screen*/
        Destroy(mouseItem.obj);
        mouseItem.item = null;

    }
    public void OnDrag(GameObject obj)
    {
        /*this method checks if where clicking on an item, 
        if so, grab the item according to the mouse location*/
        if (mouseItem.obj != null)
        {
            mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    public Vector3 GetPosition(int i)
    {
        /*this will return a vector3 to the right position in the screen display for the objects*/
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUMN)), 0f);
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
