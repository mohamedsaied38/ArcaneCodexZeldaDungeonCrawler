using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ChestBehavior : MonoBehaviour
{
    private Animator _anim;
    private Canvas _canvas;
    private bool _isInteractable = false;

    [SerializeField] private int _contentAmount;


    private void Start()
    {
        _anim = GetComponent<Animator>();
        if (_anim == null )
            _anim = this.gameObject.AddComponent<Animator>();
        _canvas = GetComponentInChildren<Canvas>();
        if (_canvas != null)
            _canvas.worldCamera = Camera.main;
    }

    [ContextMenu("Test Open Chest")]
    private void OpenChest()
    {
        _anim.SetTrigger("OpenChest");
    }

    [ContextMenu("Test Close Chest")]
    private void CloseChest()
    {
        _anim.SetInteger("Contents", _contentAmount);
        _anim.SetTrigger("CloseChest");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isInteractable = true;
            Debug.Log("Player has entered the area of this object.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isInteractable = false;
            Debug.Log("Player has exited the area of this object.");
        }
    }

}
