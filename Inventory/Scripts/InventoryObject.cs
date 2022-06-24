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
        if(_item.buffs.Length > 0)
        {
            Container.Items.Add(new InventorySlot(_item.Id, _item, _amount));
            return;
        }

        for (int i = 0; i < Container.Items.Count; ++i)
        {
            if (Container.Items[i].item.Id == _item.Id)
            {
                Container.Items[i].AddAmount(_amount);
                return;
            }
        }
        Container.Items.Add(new InventorySlot(_item.Id, _item, _amount));
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
            Container = (Inventory)formatter.Deserialize(stream);
            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void ClearInventory()
    {
        Container = new Inventory();
    }

}

/*this class simply holdes the invenotry, so we can pull the items from it easily*/
[System.Serializable]
public class Inventory
{
    public List<InventorySlot> Items = new List<InventorySlot>();
}


/*this class handles the inventory slots*/
[System.Serializable]
public class InventorySlot
{
    public int ID;
    public Item item;
    public int amount;
    public InventorySlot(int _id, Item _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}
