using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyAI : MonoBehaviour
{
    NavMeshAgent _agent;
    Transform[] _targets;
    int _targetID = 0;
    GameObject _player;
    PlayerInformation _playerInfo;

    [SerializeField] float _attackTime = 2f;
    bool _isAttacking = false;
    [SerializeField] float _attackDistance = 1f;

    [SerializeField] int _attackDamage = 5;

    private void Start()
    {
        if (transform.parent.CompareTag("Room"))
            _targets = transform.parent.Find("Patrol Route").GetComponentsInChildren<Transform>();

        _agent = GetComponent<NavMeshAgent>();
        if (_agent == null) Debug.LogError("No Agent found on this Enemy", this.gameObject);

        _agent.SetDestination(_targets[0].position);
    }

    private void Update()
    {
        if (_player != null && _agent.remainingDistance <= _attackDistance && !_isAttacking)
        {
            StartCoroutine(AttackPlayer());
            Debug.Log("Player Reached");
            _playerInfo.CauseDamge(_attackDamage);
            DirectPlayer();
        }
        
        if (_player != null && _agent.destination != _player.transform.position)
        {
            _agent.destination = _player.transform.position;
        }
        
        if (_player == null && _agent.remainingDistance <= _attackDistance)
        {
            _targetID++;
            if (_targetID == _targets.Length) _targetID = 0;

            _agent.destination = _targets[_targetID].position;
        }    
        
    }

    public void DirectPlayer()
    {
        Vector3 direction = transform.position - _player.transform.position;
        //direction.Normalize();
        _player.GetComponent<PlayerMovement>().AdjustDirection(direction);
    }

    public void SetPlayerTarget(GameObject target)
    {
        _player = target;
        if (_player != null) 
            _playerInfo = _player.GetComponent<PlayerInformation>();
    }

    IEnumerator AttackPlayer()
    {
        _isAttacking = true;
        yield return new WaitForSeconds(_attackTime);
        _isAttacking = false;
    }
}
