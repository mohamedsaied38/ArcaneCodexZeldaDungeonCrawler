using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] GameObject _overlayUI;
    [SerializeField] GameObject _menuUI;
    private InputSystem_Actions _input;
    private bool _isMenuActive = false;

    public bool IsMenuActive => _isMenuActive;

    public override void Init()
    {
        base.Init();
        _input = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        _input.Player.Enable();
        _input.Player.Menu.performed += MenuAction_Performed;
    }

    private void MenuAction_Performed(InputAction.CallbackContext obj)
    {
        _isMenuActive = !_isMenuActive;
        _menuUI.gameObject.SetActive(_isMenuActive);
        _overlayUI.gameObject.SetActive(!_isMenuActive);

        if (_isMenuActive)
            Time.timeScale = 0;
        else Time.timeScale = 1;
    }
}

