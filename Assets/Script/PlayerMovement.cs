using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private InputSystem_Actions _input;
    private Vector2 _inputMovement;
    private Vector3 _direction;
    private Vector3 _lookDirection;
    private CharacterController _characterController;

    [SerializeField] private float _speed = 5f;
    [SerializeField] Transform _model;
    [SerializeField] float _gravity = -9.81f;

    private void OnEnable()
    {
        _input = new InputSystem_Actions();
        _input.Player.Enable();
    }

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        _inputMovement = _input.Player.Move.ReadValue<Vector2>();
        _direction.x = _inputMovement.x;
        _direction.z = _inputMovement.y;

        if (!_characterController.isGrounded)
            _direction.y += _gravity * Time.deltaTime;
        else
            _direction.y = 0;

        //transform.Translate(_direction * (_speed * Time.deltaTime), Space.World);
        _characterController.Move(_direction * (_speed * Time.deltaTime));

        _lookDirection = _direction;
        _lookDirection.y = 0;
        _lookDirection.Normalize();
        _lookDirection += transform.position;
        _model.LookAt(_lookDirection);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.TryGetComponent<ICollidable>(out ICollidable collidable))
        {
            collidable.OnCollide(this.transform);
        }
    }

    private void OnDisable()
    {
        _input.Player.Disable();
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, _model.forward);
    }
}
