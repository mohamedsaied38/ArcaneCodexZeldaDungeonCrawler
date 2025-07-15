using UnityEngine;

[System.Serializable]
public class Room : MonoBehaviour
{
    private string _name;
    private bool[] _exitsOpen = new bool[] { false, false, false, false };
    private bool _visited = false;

    public bool Visited => _visited;
    public string Name => _name;

    public Room(string name)
    {
        _name = name;
    }

}
