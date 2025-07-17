using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    private Room[,] _roomGrid;
    [SerializeField] private int _gridXSize;
    [SerializeField] private int _gridYSize;
    private int _gridSize;
    [SerializeField] private List<Room> _rooms = new List<Room>();

    private Vector2Int _currentRoom;
    private Vector2Int _targetRoom;
    [SerializeField]
    private List<Room> _visitedRooms = new List<Room>();
    private List<Vector2Int> _path = new List<Vector2Int>();

    private Vector2Int _falseRoom = new Vector2Int(-1, -1); //Subsitutes for null checks

    [SerializeField] private GameObject _roomObject;
    [SerializeField] private int _roomSize = 1;

    [SerializeField] private GameObject _player;

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
        //_visitedRooms[0].VisitRoom();
        _path.Clear();
        _path.Add(_currentRoom);    

        Debug.Log($"Starting Room is {_roomGrid[_currentRoom.x, _currentRoom.y].Name}.");

        StartCoroutine(DungeonRoutine());
    }

    IEnumerator DungeonRoutine()
    {
        while (_visitedRooms.Count < _gridSize)
        {
            _targetRoom = ValidRandomDirection(_currentRoom);
            if (_targetRoom != _falseRoom)
            {
                _roomGrid[_currentRoom.x, _currentRoom.y].VisitRoom(GetDirection(_currentRoom, _targetRoom));
                _roomGrid[_targetRoom.x, _targetRoom.y].VisitRoom(GetDirection(_targetRoom, _currentRoom));
                _path.Add(_targetRoom);
                _currentRoom = _targetRoom;
                _visitedRooms.Add(_roomGrid[_currentRoom.x, _currentRoom.y]);
            }
            else break;

            yield return null;
        }
        BuildDungeon(_path);
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
                    if (CheckingDirection.y - 1 >= 0 &&
                        !_roomGrid[CheckingDirection.x, CheckingDirection.y - 1].Visited)
                    {
                        CheckingDirection.y -= 1;
                        directions.Add(CheckingDirection);
                    }
                    break;
                case DungeonDirections.West:
                    if (CheckingDirection.x - 1 >= 0 && 
                        !_roomGrid[CheckingDirection.x - 1, CheckingDirection.y].Visited)
                    {
                        CheckingDirection.x -= 1;
                        directions.Add(CheckingDirection);
                    }
                    break;
                default:
                    break;
            }
        }


        return directions.Count > 0 ? directions[Random.Range(0, directions.Count)] : _falseRoom;
    }

    private void BuildDungeon(List<Vector2Int> grid)
    {
        Vector3 pos = Vector3.zero;
        foreach (Vector2Int room in grid)
        {
            pos.x = room.x * _roomSize;
            pos.z = room.y * _roomSize;
            RoomController rc = Instantiate(_roomObject, pos, Quaternion.identity).GetComponent<RoomController>();
            rc.SetRoom(_roomGrid[room.x, room.y]);
        }

        if (_player != null)
        {
            pos.x = grid[0].x * _roomSize;
            pos.z = grid[0].y * _roomSize;
            pos.y = 1.05f;

            _player.GetComponent<CharacterController>().enabled = false;
            _player.transform.position = pos;
            _player.GetComponent<CharacterController>().enabled = true;
        }
    }

    private DungeonDirections GetDirection(Vector2Int current, Vector2Int target)
    {
        var diff = target - current;

        return diff.x switch
        {
            1 => DungeonDirections.East,
            -1 => DungeonDirections.West,
            _ => diff.y switch
            {
                1 => DungeonDirections.North,
                -1 => DungeonDirections.South,
                _ => DungeonDirections.North
            }
        };
    }
}
