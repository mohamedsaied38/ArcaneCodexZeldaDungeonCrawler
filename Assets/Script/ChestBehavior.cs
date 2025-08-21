using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ChestBehavior : MonoBehaviour
{
    private Animator _anim;
    private Canvas _canvas;
    private bool _isInteractable = false;
    private InputSystem_Actions _input;

    [SerializeField] private int _contentAmount;
    [SerializeField] private bool _healthFountain;


    private void Start()
    {
        _anim = GetComponent<Animator>();
        if (_anim == null )
            _anim = this.gameObject.AddComponent<Animator>();
        _canvas = GetComponentInChildren<Canvas>(true);
        if (_canvas != null)
            _canvas.worldCamera = Camera.main;

        _input = new InputSystem_Actions();
        _input.Player.Enable();
        _input.Player.Interact.performed += OpenChest;
    }    
       
    [ContextMenu("Test Open Chest")]
    private void OpenChest(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!_isInteractable) return;

        _anim.SetBool("IsOpen", true);

        if (_healthFountain)
        {
            FindFirstObjectByType<PlayerInformation>().HealDamage(_contentAmount);
        }
        else
        {
            //Open container Inventory window
        }
    }

    [ContextMenu("Test Close Chest")]
    private void CloseChest()
    {
        _anim.SetInteger("Contents", _contentAmount);
        _anim.SetBool("IsOpen", false);
        //close Container INventory Window
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isInteractable = true;
            Debug.Log("Player has entered the area of this object.");
            CanvasActive(_isInteractable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_isInteractable) 
                CloseChest();

            _isInteractable = false;
            Debug.Log("Player has exited the area of this object.");
            CanvasActive(_isInteractable);
        }
    }

    private void CanvasActive(bool state)
    {
        _canvas.enabled = state;
    }

}
