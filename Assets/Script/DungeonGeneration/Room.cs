using UnityEngine;

[System.Serializable]
public class Room
{
    public string name;
    private bool[] _exitsOpen = new bool[] { false, false, false, false };
    private bool _visited = false;

    public bool Visited => _visited;
    public string Name => name;
    public bool[] Exits => _exitsOpen;

    public Room(string name)
    {
        this.name = name;
    }

    public void VisitRoom(DungeonDirections direction)
    {
        _visited = true;
        _exitsOpen[(int)direction] = true;
    }
}
