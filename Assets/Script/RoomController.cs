using UnityEngine;
using Cinemachine;
using Unity.AI.Navigation;
using System.Collections.Generic;

public class RoomController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _roomCamera;
    [SerializeField] private List<Renderer> _renderers = new List<Renderer>();

    [SerializeField] private Transform[] _environmentObjects;
    [SerializeField, Tooltip("Exits should be in order of N,E,S,W.")] private ExitController[] _exits;
    private Room _roomInfo;
    private NavMeshSurface _navMeshSurface;
    [SerializeField] private GameObject _enemyPrefab;

    private void OnEnable()
    {
        if (_roomCamera == null)
            Debug.LogWarning($"{transform.name}'s Camera is Null", this.gameObject);
        _renderers.AddRange(GetComponentsInChildren<Renderer>(true));
        _navMeshSurface = GetComponent<NavMeshSurface>();
        //RenderRoom(false);
        //RandomizeEnvironment();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RenderRoom(true);
            _roomCamera.Priority = 12;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RenderRoom(false);
            _roomCamera.Priority = 10;
        }
    }

    private void RenderRoom(bool active)
    {
        foreach (var renderer in _renderers)
        {
            renderer.enabled = active;
        }
    }

    public void RandomizeEnvironment()
    {
        float rng = 0;

        _renderers.Add(Instantiate(_enemyPrefab, transform.position, Quaternion.identity, transform).GetComponent<Renderer>()); 

        foreach(Transform t in _environmentObjects)
        {
            rng = Random.value;
            if (rng >= 0.75f)
                t.gameObject.SetActive(true);
            else t.gameObject.SetActive(false);
        }
        RenderRoom(false);
    }

    public void SetRoom(Room room)
    {
        _roomInfo = room;
        for (int i = 0;  i < _roomInfo.Exits.Length; i++)
        {
            _exits[i].SetExit(_roomInfo.Exits[i]);
        }
    }

    public void BuildNavMesh()
    {
        _navMeshSurface.BuildNavMesh();
        
    }
}
