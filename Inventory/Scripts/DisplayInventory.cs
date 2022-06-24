using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI; /*for image*/
using UnityEngine;
using TMPro;

public class DisplayInventory : MonoBehaviour
{

    public GameObject inventoryPrefab;
    [SerializeField] private InventoryObject inventory;

    [SerializeField] private int X_START;
    [SerializeField] private int Y_START;
    [SerializeField] private int X_SPACE_BETWEEN_ITEM;
    [SerializeField] private int NUMBER_OF_COLUMN;
    [SerializeField] private  int Y_SPACE_BETWEEN_ITEMS;
    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        for(int i = 0; i<inventory.Container.Items.Count; ++i)
        {   
            InventorySlot slot = inventory.Container.Items[i];

            if(itemsDisplayed.ContainsKey(slot))
            {
                itemsDisplayed[slot].GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
            }
            else
            {   
                /*create object case*/
                var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
                obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.item.Id].uiDisplay;
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
                itemsDisplayed.Add(slot,obj);/*add to dictionary*/

            }
        }
    }

    public void CreateDisplay()
    {   
        /*inits the display at startup*/
        for(int i = 0; i<inventory.Container.Items.Count; ++i)
        {   
            InventorySlot slot = inventory.Container.Items[i];

            /*create each object*/
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.item.Id].uiDisplay;
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");

            itemsDisplayed.Add(slot,obj);/*add to dictionary*/
        }
    }

    public Vector3 GetPosition(int i)
    {   
        /*this will return a vector3 to the right position in the screen display for the objects*/
        return new Vector3((X_START+(X_SPACE_BETWEEN_ITEM *(i % NUMBER_OF_COLUMN))),(Y_START+(-Y_SPACE_BETWEEN_ITEMS *(i/NUMBER_OF_COLUMN))),0f);
    }
}
