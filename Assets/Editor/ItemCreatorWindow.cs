#if UNITY_EDITOR
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ItemCreatorWindow : EditorWindow
{
    // Fields where the student enters item details
    private string _name = "New Item";
    private ItemType _type = ItemType.Consumable;
    private int _value = 0;
    private InventoryManager _mgr;
    private string _message;

    private void OnEnable()
    {
        // Look for the InventoryManager in the open scene
        _mgr = FindFirstObjectByType<InventoryManager>();
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
       /* GUIStyle richTextStyle = new GUIStyle(GUI.skin.label);
        richTextStyle.richText = true;*/
        EditorStyles.centeredGreyMiniLabel.richText = true;
        EditorStyles.centeredGreyMiniLabel.fontSize = 18;

        // Title
        EditorGUILayout.LabelField("Create New Item", EditorStyles.boldLabel);

        // Editable fields for the new item
        _name  = EditorGUILayout.TextField("Name", _name);
        _type  = (ItemType)EditorGUILayout.EnumPopup("Type", _type);
        _value = EditorGUILayout.IntField("Value", _value);

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
            Item newItem = new Item(_name, _type, -1);
            newItem.value = _value;

            // Ask the manager to add the item and save the list to JSON
            if (_mgr.AddItemToMasterList(newItem, saveAfter: true))
            {
                _message = $"<color=green>Added '{_name}' to master list and saved.</color>";
                
            }
            else
            {
                _message = $"<color=yellow>Failed to add '{_name}' (duplicate or error).</color>";
                
            }
            Debug.Log(_message);
        }

        EditorGUILayout.LabelField(_message, EditorStyles.centeredGreyMiniLabel);
        //GUILayout.Label(_message,richTextStyle);
    }
}
#endif