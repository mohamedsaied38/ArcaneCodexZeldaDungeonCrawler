using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private InputSystem_Actions _input;
    private Vector2 _inputMovement;
    private Vector3 _direction;
    private Vector3 _lookDirection;

    [SerializeField] private float _speed = 5f;
    [SerializeField] Transform _model;

    private void OnEnable()
    {
        _input = new InputSystem_Actions();
        _input.Player.Enable();
    }

    private void Update()
    {
        _inputMovement = _input.Player.Move.ReadValue<Vector2>();
        _direction.x = _inputMovement.x;
        _direction.z = _inputMovement.y;

        transform.Translate(_direction * (_speed * Time.deltaTime), Space.World);

        _lookDirection = transform.position + _direction.normalized;
        _model.LookAt(_lookDirection);
    }




    private void OnDisable()
    {
        _input.Player.Disable();
    }
}
