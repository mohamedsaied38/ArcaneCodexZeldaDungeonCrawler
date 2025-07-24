using UnityEngine;
using UnityEngine.AI;

public class TestAIScript : MonoBehaviour
{
    [SerializeField] GameObject _target;
    NavMeshAgent _agent;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (_agent == null) return;

        if (_target == null) return;

        _agent.SetDestination(_target.transform.position);

    }
}
