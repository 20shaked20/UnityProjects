using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor; /*for LoadAssetAtPath*/
using System.Runtime.Serialization.Formatters.Binary; /*json*/
using System.IO; /*file stream*/

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver
{   
    public string savePath;
    private ItemDatabaseObject database;
    public List<InventorySlot> Container = new List<InventorySlot>();

    private void OnEnable()
    {   
        /*this method exists because the database needs to be private so it wont intereferce with the json parser,
        this method simply reaches the database from the location, and loads it from there!*/
#if UNITY_EDITOR
        database = (ItemDatabaseObject)AssetDatabase.LoadAssetAtPath("Assets/Resources/Database.asset", typeof(ItemDatabaseObject));
#else
        database = Resources.Load<ItemDatabaseObject>("Database");
#endif
    }

    /*this method adds an items to the inventory, if one exists it will increase its amount*/
    public void AddItem(ItemObject _item, int _amount)
    {

        for (int i = 0; i < Container.Count; ++i)
        {
            if (Container[i].item == _item)
            {
                Container[i].AddAmount(_amount);
                return;
            }
        }
        Container.Add(new InventorySlot(database.GetId[_item],_item, _amount));
    }

    /*this method saves the inventory into a json file*/
    public void Save()
    {   
        string saveData = JsonUtility.ToJson(this,true);
        BinaryFormatter bf = new BinaryFormatter();

        /*concat is to combine strings, its more memory efficeint.*/
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file,saveData);

        file.Close();

    }

    /*this method loads the inventory to the player (if exists!)*/
    public void Load()
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(),this);
            file.Close();
        }
    }

    /*every time unity will desrialze my items, it will fill the dictionary with the items*/
   public void OnAfterDeserialize()
   {
        for( int i =0; i < Container.Count; ++i )
        {
            Container[i].item = database.GetItem[Container[i].ID];
        }
   }

   public void OnBeforeSerialize()
   {

   }

}

/*this class handles the inventory slots*/
[System.Serializable]
public class InventorySlot
{   
    public int ID;
    public ItemObject item;
    public int amount;
    public InventorySlot(int _id, ItemObject _item, int _amount)
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
