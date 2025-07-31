using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private InputSystem_Actions _input;
    private Vector2 _inputMovement;
    private Vector3 _direction;
    private Vector3 _lookDirection;
    private CharacterController _characterController;

    private float _speed;
    [SerializeField] Transform _model;
    [SerializeField] float _gravity = -9.81f;

    PlayerInformation _playerInformation;

    private bool _canBeHit = true;
    private bool _isKickedBack = false;

    [SerializeField] private float _shortWaitLength = .25f;
    //[SerializeField] private float _mediumWaitLength = 1.0f;
    [SerializeField] private float _longWaitlength = 2;

    private WaitForSeconds _shortWait;
    private WaitForSeconds _longWait;

    private Coroutine _invisibiltyRoutine;
    private Coroutine _kickbackRoutine;


    private void OnEnable()
    {
        _input = new InputSystem_Actions();
        _input.Player.Enable();
    }

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInformation = GetComponent<PlayerInformation>();

        _shortWait = new WaitForSeconds(_shortWaitLength);
        _longWait = new WaitForSeconds(_longWaitlength);
    }

    private void Update()
    {
        if (_playerInformation.CurrentHealth <= 0 )
        {
            return;
        }

        _speed = _playerInformation.Speed;

        if (!_isKickedBack)
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
        else
        {            
            //transform.Translate(_direction * (_speed * Time.deltaTime), Space.World);
            _characterController.Move(_direction * (_speed * Time.deltaTime * -.5f));
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.TryGetComponent<ICollidable>(out ICollidable collidable))
        {
            if (_canBeHit)
            {
                collidable.OnCollide(this.transform);
                _canBeHit = false;
                _isKickedBack = true;
                if (_kickbackRoutine == null)
                    _kickbackRoutine = StartCoroutine(KickBackRoutine());
                if (_invisibiltyRoutine == null)
                    _invisibiltyRoutine = StartCoroutine(InvisibilityFramesRoutine());
            }
        }
    }

    IEnumerator KickBackRoutine()
    {
        yield return _shortWait;
        _isKickedBack = false;
        _kickbackRoutine = null;
    }

    IEnumerator InvisibilityFramesRoutine()
    {
        yield return _longWait;
        _canBeHit = true;
        _invisibiltyRoutine = null;
    }

    private void OnDisable()
    {
        _input.Player.Disable();
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, _model.forward);
    }

    public void AdjustDirection(Vector3 direction)
    {
        Debug.Log("AdjustDirection");
        _direction = direction;

        _isKickedBack = true;
        if (_kickbackRoutine == null)
            _kickbackRoutine = StartCoroutine(KickBackRoutine());
    }
}
