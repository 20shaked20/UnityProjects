using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] Items;

    /* this approach is made in order to be able to get the ID & ITEM in o(1)
    * it might be memory heavy, but more effeicnt than running a loop to find the Item
    (each approach is fine, it depends wether we pref memory or speed)
    */
    public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();


    /*every time unity will desrialze my items, it will fill the dictionary with the items*/
    public void OnAfterDeserialize()
    {
        for (int i = 0; i < Items.Length; ++i)
        {
            Items[i].Id = i;
            GetItem.Add(i, Items[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        GetItem = new Dictionary<int, ItemObject>();

    }
}
