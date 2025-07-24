using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyAI : MonoBehaviour
{
    NavMeshAgent _agent;
    Transform[] _targets;
    int _targetID = 0;
    GameObject _player;

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
        if (_player != null && _agent.destination != _player.transform.position)
        {
            _agent.destination = _player.transform.position;
        }
        else if (_player == null && _agent.remainingDistance <= .01f)
        {
            _targetID++;
            if (_targetID == _targets.Length) _targetID = 0;

            _agent.destination = _targets[_targetID].position;
        }
        else if (_player != null && _agent.remainingDistance <= .01f)
        {
            //Attack Player
            Debug.Log("Player Reached");
        }

    }
}
