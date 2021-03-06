using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor; /*for LoadAssetAtPath*/
using System.Runtime.Serialization.Formatters.Binary; /*json*/
using System.Runtime.Serialization;/*IFORMATTER*/
using System.IO; /*file stream*/


[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savePath;
    public ItemDatabaseObject database;
    public Inventory Container;

    /*this method adds an items to the inventory, if one exists it will increase its amount*/
    public void AddItem(Item _item, int _amount)
    {

        /*simple fix for not stacking diffrenet items!*/
        /*TODO: fix this later*/
        if (_item.buffs.Length > 0)
        {
            SetEmptySlot(_item, _amount);
            return;
        }

        for (int i = 0; i < Container.Items.Length; ++i)
        {
            if (Container.Items[i].ID == _item.Id)
            {
                Container.Items[i].AddAmount(_amount);
                return;
            }
        }
        SetEmptySlot(_item, _amount);
    }

    public InventorySlot SetEmptySlot(Item _item, int _amount)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].ID <= -1)
            {
                Container.Items[i].UpdateSlot(_item.Id, _item, _amount);
                return Container.Items[i];
            }
        }

        /*return later, when inventory is full what to do*/
        return null;
    }

    public void MoveItem(InventorySlot item1, InventorySlot item2)
    {
        /* simple SWAP method for items */
        InventorySlot temp = new InventorySlot(item2.ID, item2.item, item2.amount);
        item2.UpdateSlot(item1.ID, item1.item, item1.amount);
        item1.UpdateSlot(temp.ID, temp.item, temp.amount);
    }

    public void RemoveItem(Item _item)
    {
        /*removing item from inventory database*/
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if(Container.Items[i].item == _item)
            {
                Container.Items[i].UpdateSlot(-1,null,0);
            }
        }
    }


    [ContextMenu("Save")]
    public void Save()
    {
        /*first apporach - allows user to change item values:*/

        // string saveData = JsonUtility.ToJson(this,true);
        // BinaryFormatter bf = new BinaryFormatter();

        // /*concat is to combine strings, its more memory efficeint.*/
        // FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        // bf.Serialize(file,saveData);

        // file.Close();

        /*second approach - does not allows for item changes*/

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();

    }

    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            /*first apporach - allows user to change item values:*/

            // BinaryFormatter bf = new BinaryFormatter();
            // FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            // JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(),this);
            // file.Close();

            /*second approach - does not allows for item changes*/
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);
            /*loading each slot again from the save slot*/
            for (int i = 0; i < Container.Items.Length; i++)
            {
                Container.Items[i].UpdateSlot(newContainer.Items[i].ID, newContainer.Items[i].item, newContainer.Items[i].amount);
            }
            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void ClearInventory()
    {
        Container.Clear();
    }

}

/*this class simply holdes the invenotry, so we can pull the items from it easily*/
[System.Serializable]
public class Inventory
{
    public InventorySlot[] Items = new InventorySlot[28];
    public void Clear()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i].UpdateSlot(-1,new Item(),0);
        }
    }
}


/*this class handles the inventory slots*/
[System.Serializable]
public class InventorySlot
{   
    public ItemType[] AllowedItems = new ItemType[0]; /*used in static mainly part 6 23:15 for dynamic maybe*/
    public UserInterface parent; /*we use it to help us drag the item from inventory to the equipment inv!*/
    public int ID = -1;
    public Item item;
    public int amount;

    public InventorySlot()
    {
        ID = -1;
        item = null;
        amount = 0;
    }
    public InventorySlot(int _id, Item _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }

    public void UpdateSlot(int _id, Item _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }

    public bool CanPlaceInSlot(ItemObject _item)
    {
        if(AllowedItems.Length <=0)
        {
            return true;
        }
        for (int i = 0; i < AllowedItems.Length; i++)
        {
            if(_item.type == AllowedItems[i])
            {
                return true;
            }
        }
        return false;
    }
}
