using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //Master Item List ( ID, Item Class)
    private Dictionary<int, Item> _masterItemList = new Dictionary<int, Item>();
    //Player Inventory ( ID, Count)
    private Dictionary<int, int> _playerInventory = new Dictionary<int, int>();

    public void AddToInventory(int id, int count)
    {
        if (!_masterItemList.ContainsKey(id))
        {
            Debug.LogWarning($"Item Id:{id} does not exist in Master Item List.");
            return;
        }
        if (_playerInventory.ContainsKey(id))
        {
            _playerInventory[id] += count;
        }
        else
            _playerInventory.Add(id, count);
    }

    public void RemoveFromInventory(int id, int count)
    {
        if (!_masterItemList.ContainsKey(id))
        {
            Debug.LogWarning($"Item Id:{id} does not exist in Master Item List.");
            return;
        }
        if (!_playerInventory.ContainsKey(id))
        {
            //May report to somewhere that the player doesn't have the item
            Debug.LogWarning($"Item ID:{id} does not exist in the Player Inventory.");
            return;
        }
        if (_playerInventory[id] > count)
        {
            _playerInventory[id] -= count;
        }
        else
        {
            _playerInventory.Remove(id);
        }

    }

    public int InventoryAmount(int id)
    {
        if (!_masterItemList.ContainsKey(id))
        {
            Debug.LogWarning($"Item Id:{id} does not exist in Master Item List.");
            return 0;
        }
        if (!_playerInventory.ContainsKey(id) ) return 0;
        else return _playerInventory[id];
    }

    [ContextMenu("Next Key ID")]
    private int GetNextKeyID()
    {
        Debug.Log($"Next Key Value = {_masterItemList.Count}");
        return _masterItemList.Count;
    }

    private bool AddToMasterList(Item item)
    {
        if (!_masterItemList.ContainsValue(item))
        {
            int id = GetNextKeyID();
            if (!_masterItemList.ContainsKey(id))
            {
                item.id = id;
                _masterItemList.Add(id, item);
                return true;
            }
        }
        return false;
    }

    [ContextMenu("Add Test Item")]
    private void AddTestItem()
    {
        Item test = new Item("Test", ItemType.Weapon, 34);
        if (!AddToMasterList(test)) Debug.LogWarning("Load Failed");
        else Debug.Log("Load Success");
    }

    [ContextMenu("Load Master List")]
    public void ReadInJsonList()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "MasterItems.json");
        if (File.Exists(filePath))
        {
            string masterList = File.ReadAllText(filePath);
            _masterItemList = JsonUtility.FromJson<Dictionary<int, Item>>(masterList);
        }
    }

    [ContextMenu("Save Master List")]
    public void WriteOutToJson()
    {
        string masterList = JsonUtility.ToJson(_masterItemList);

        Debug.Log(masterList);

        string filePath = Path.Combine(Application.persistentDataPath, "MasterItems.json");
        File.WriteAllText(filePath, masterList);
        
    }
}
