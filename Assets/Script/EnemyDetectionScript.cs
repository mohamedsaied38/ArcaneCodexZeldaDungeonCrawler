using UnityEngine;

public class EnemyDetectionScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Detected");
            transform.parent.GetComponent<BaseEnemyAI>().SetPlayerTarget(other.gameObject);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            transform.parent.GetComponent<BaseEnemyAI>().SetPlayerTarget(null);
    }
}
