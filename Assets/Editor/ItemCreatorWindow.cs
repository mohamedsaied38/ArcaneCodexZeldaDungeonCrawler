#if UNITY_EDITOR
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public class ItemCreatorWindow : EditorWindow
{
    // Fields where the student enters item details
    private string _name = "New Item";
    private ItemType _type = ItemType.Consumable;
    private int _value = 0;
    private int _id = -1;
    bool _isDup = false;

    private InventoryManager _mgr;
    private Dictionary<int,Item> _masterList = new Dictionary<int,Item>();

    private Vector2 _scrollPos;

    private string _message;
    Item _newItem;

    private void OnEnable()
    {
        // Look for the InventoryManager in the open scene
        _mgr = FindFirstObjectByType<InventoryManager>();
        _mgr.ReadInJsonList();
        _masterList = _mgr.MasterItemList;
    }

    // Adds menu option under "Tools/Inventory"
    [MenuItem("Tools/Inventory/Create Item")]
    public static void Open()
    {
        // Creates and shows the editor window
        GetWindow<ItemCreatorWindow>("Create Item");
    }

    // Unity calls this every frame the window is visible to draw the UI
    private void OnGUI()
    {
        GUIStyle editorErrorMessageStyle = new GUIStyle(GUI.skin.label)
        {
            richText = true,
            alignment = TextAnchor.MiddleCenter,
            wordWrap = true,
            fontSize = 16
        };
       
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical(GUILayout.MinWidth(300));
      
        // Title
        EditorGUILayout.LabelField("Create New Item", EditorStyles.boldLabel);

        // Editable fields for the new item
        _name  = EditorGUILayout.TextField("Name", _name);
        _type  = (ItemType)EditorGUILayout.EnumPopup("Type", _type);
        _value = EditorGUILayout.IntField("Value", _value);
        _id    = EditorGUILayout.IntField("ID", _id);

        EditorGUILayout.Space();

        // Warn the user if there isn't one
        if (_mgr == null)
        {
            EditorGUILayout.HelpBox("No InventoryManager found in the scene.", MessageType.Warning);
            return;
        }

        // Main button to create and add the item
        if (GUILayout.Button("Add to Master List", GUILayout.Height(30)))
        {
            // Create a new Item object.
            // ID will be assigned by the InventoryManager itself.
            _newItem = new Item(_name, _type, _id);
            _newItem.value = _value;

            Debug.Log($"{_newItem.name} {_newItem.id}");

            // Ask the manager to add the item and save the list to JSON
            if (_mgr.AddItemToMasterList(_newItem, saveAfter: true))
            {
                _message = $"<color=green>Added '{_name}' to master list and saved.</color>";
                _isDup = false;
            }
            else
            {
                _message = $"<color=yellow>Failed to add '{_name}' (duplicate or error).</color>";
               _isDup = true;
            }
            Debug.Log(_message);
        }

        EditorGUILayout.LabelField(_message, editorErrorMessageStyle);
        if (_isDup)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Update"))
            { 
                _mgr.UpdateItemInMasterList(_newItem);
                _isDup = false;
                _message = string.Empty;
            }
            if (GUILayout.Button("Assign New ID"))
            {
                _newItem.id = _mgr.GetNextKeyID();
                _id = _newItem.id;
                _isDup = false;
                _message = string.Empty;
            }
            EditorGUILayout.EndHorizontal();
        }

        //GUILayout.Label(_message,richTextStyle);
        EditorGUILayout.EndVertical();


        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("Master Item List", EditorStyles.boldLabel);

        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos,GUILayout.Height(150));

        foreach(var kvp in _masterList)
        {
            if (GUILayout.Button(kvp.Value.name))
            {
                _name = kvp.Value.name;
                _type = kvp.Value.type;
                _value = kvp.Value.value;
                _id = kvp.Value.id;
            }
        }

        EditorGUILayout.EndScrollView();
   
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

    }
}
#endif