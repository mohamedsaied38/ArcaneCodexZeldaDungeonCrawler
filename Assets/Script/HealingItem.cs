using UnityEngine;

public class HealingItem : MonoBehaviour
{
    [SerializeField] int _healAmount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerInformation> (out PlayerInformation info))
        {
            info.HealDamage(_healAmount);
            Destroy(this.gameObject);
        }
    }
}
