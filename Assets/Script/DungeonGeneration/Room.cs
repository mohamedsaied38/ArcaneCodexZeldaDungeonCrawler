using UnityEngine;

[System.Serializable]
public class Room
{
    public string name;
    private int x;
    private int y;
    private bool[] _exitsOpen = new bool[] { false, false, false, false };
    private bool _visited = false;
    private bool _voidRoom = false;

    public bool Visited => _visited;
    public string Name => name;
    public bool[] Exits => _exitsOpen;
    public int X => x;
    public int Y => y;

    public bool VoidRoom => _voidRoom;

    public Room(string name, int x, int y)
    {
        this.name = name;
        this.x = x;
        this.y = y;
    }

    public void VisitRoom(DungeonDirections direction)
    {
        _visited = true;
        _exitsOpen[(int)direction] = true;
    }

    public void OpenExit(DungeonDirections direction)
    {
        _exitsOpen[(int)direction] = true;
    }

    public Vector2Int GetGridPosition()
    {
        return new Vector2Int(x, y);
    }

    public void CreateVoid()
    {
        _voidRoom = true;
    }
}
