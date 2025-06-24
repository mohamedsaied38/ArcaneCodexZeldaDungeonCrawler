using UnityEngine;
using Cinemachine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _roomCamera;
    [SerializeField] private Renderer[] _renderers;

    private void OnEnable()
    {
        if (_roomCamera == null)
            Debug.LogWarning($"{transform.name}'s Camera is Null", this.gameObject);
        _renderers = GetComponentsInChildren<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var renderer in _renderers)
            {
                renderer.enabled = true;
            }
            _roomCamera.Priority = 12;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var renderer in _renderers)
            {
                renderer.enabled = false;
            }
            _roomCamera.Priority = 10;
        }
    }
}
