using UnityEngine;
using Cinemachine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _roomCamera;
    [SerializeField] private Renderer[] _renderers;

    [SerializeField] private Transform[] _environmentObjects;
    [SerializeField, Tooltip("Exits should be in order of N,E,S,W.")] private ExitController[] _exits;
    private Room _roomInfo;

    private void OnEnable()
    {
        if (_roomCamera == null)
            Debug.LogWarning($"{transform.name}'s Camera is Null", this.gameObject);
        _renderers = GetComponentsInChildren<Renderer>(true);
        RenderRoom(false);
        RandomizeEnvironment();
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

    private void RandomizeEnvironment()
    {
        float rng = 0;
        foreach(Transform t in _environmentObjects)
        {
            rng = Random.value;
            if (rng >= 0.75f)
                t.gameObject.SetActive(true);
            else t.gameObject.SetActive(false);
        }
    }

    public void SetRoom(Room room)
    {
        _roomInfo = room;
        for (int i = 0;  i < _roomInfo.Exits.Length; i++)
        {
            _exits[i].SetExit(_roomInfo.Exits[i]);
        }
    }
}
