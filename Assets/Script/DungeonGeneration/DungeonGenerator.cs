using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DungeonDirections
{
    North,
    East,
    South,
    West
}


public class DungeonGenerator : MonoBehaviour
{
    private Room[,] _roomGrid;
    [SerializeField] private int _gridXSize;
    [SerializeField] private int _gridYSize;
    private int _gridSize;
    [SerializeField] private List<Room> _rooms = new List<Room>();

    private Vector2Int _currentRoom;
    private Vector2Int _targetRoom;    
    private List<Room> _visitedRooms = new List<Room>();
    private List<Vector2Int> _path = new List<Vector2Int>();

    private void Start()
    {
        _roomGrid = new Room[_gridXSize, _gridYSize];
        _gridSize = _gridXSize * _gridYSize;
        
        for (int row = 0; row < _gridXSize;row++)
        {
            for (int col = 0; col < _gridYSize;col++)
            {
                _roomGrid[row,col] = new Room($"{row}-{col}");
                _rooms.Add(_roomGrid[row,col]); //Just so we can look at in the inspector
            }
        }

        foreach (Room r in _rooms)
            Debug.Log($"Room: {r.Name}");
    }

    //Pick start Position

    //Pick a random direction
    //check if random Direction valid
    //Valid is within grid borders and not visited
    //Check target against grid Borders 
    //Check if Target Visited
    //Move to Target
    //Mark target as visited
    //Repeat

    //If no valid Direction Got back to previous

    [ContextMenu("Generate Dungeon")]
    public void GenerateDungeon()
    {
        int rndX = Random.Range(0, _gridXSize);
        int rndY = Random.Range(0, _gridYSize);
        _currentRoom = new Vector2Int(rndX,rndY);
        _visitedRooms.Clear();
        _visitedRooms.Add(_roomGrid[_currentRoom.x, _currentRoom.y]);

        Debug.Log($"Starting Room is {_roomGrid[_currentRoom.x, _currentRoom.y].Name}.");

        StartCoroutine(DungeonRoutine());
    }

    IEnumerator DungeonRoutine()
    {
        while (_visitedRooms.Count < _gridSize)
        {
            _targetRoom = ValidRandomDirection(_currentRoom);
            yield return null;
        }
    }


    private Vector2Int ValidRandomDirection(Vector2Int currentPOS)
    {
        List<Vector2Int> directions = new List<Vector2Int>();
        Vector2Int CheckingDirection = currentPOS;
        for (int i = 0; i < 4;i++)
        {
            CheckingDirection = currentPOS;
            switch ((DungeonDirections)i)
            {
                case DungeonDirections.North:
                    if (CheckingDirection.y + 1 < _gridYSize &&
                        !_roomGrid[CheckingDirection.x, CheckingDirection.y + 1].Visited)
                    {
                        CheckingDirection.y += 1;
                        directions.Add(CheckingDirection);                        
                    }
                    break;
                case DungeonDirections.East:
                    if (CheckingDirection.x + 1 < _gridXSize && !_roomGrid[CheckingDirection.x + 1, CheckingDirection.y].Visited)
                    {
                        CheckingDirection.x += 1;
                        directions.Add(CheckingDirection);
                    }
                    break;
                case DungeonDirections.South:
                    break;
                case DungeonDirections.West:
                    break;
                default:
                    break;
            }
        }
        return directions[Random.Range(0, directions.Count)];
    }

}
