using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayInventory : MonoBehaviour
{

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

    public void CreateDisplay()
    {   
        /*inits the display at startup*/
        for(int i = 0; i<inventory.Container.Count; ++i)
        {   
            /*create each object*/
            var obj = Instantiate(inventory.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
            itemsDisplayed.Add(inventory.Container[i],obj);/*add to dictionary*/
        }
    }

    public void UpdateDisplay()
    {
        for(int i = 0; i<inventory.Container.Count; ++i)
        {
            if(itemsDisplayed.ContainsKey(inventory.Container[i]))
            {
                itemsDisplayed[inventory.Container[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
            }
            else
            {   
                /*create object case*/
                var obj = Instantiate(inventory.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
                itemsDisplayed.Add(inventory.Container[i],obj);/*add to dictionary*/

            }
        }
    }

    public Vector3 GetPosition(int i)
    {   
        /*this will return a vector3 to the right position in the screen display for the objects*/
        return new Vector3((X_START+(X_SPACE_BETWEEN_ITEM *(i % NUMBER_OF_COLUMN))),(Y_START+(-Y_SPACE_BETWEEN_ITEMS *(i/NUMBER_OF_COLUMN))),0f);
    }
}
