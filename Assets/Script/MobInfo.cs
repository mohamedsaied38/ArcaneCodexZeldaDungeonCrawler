using UnityEngine;

public class MobInfo : MonoBehaviour
{
    [SerializeField] int _maxHealth = 50;
    int _currentHealth;
    [SerializeField] bool _isAttackable = true;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    private void OnMobDeath()
    {
        RoomController rc = GetComponentInParent<RoomController>();
        if (rc != null)
        {
            rc.RemoveRenderer(GetComponent<Renderer>());
        }

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isAttackable && other.CompareTag("Weapon"))
        {
            _currentHealth -= other.GetComponentInParent<PlayerInformation>().AttackDamage;
            if (_currentHealth <= 0)
                OnMobDeath();
        }
    }
}
