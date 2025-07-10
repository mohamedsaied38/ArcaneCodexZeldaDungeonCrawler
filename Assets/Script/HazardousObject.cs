using UnityEngine;
using UnityEngine.Events;

public class HazardousObject : MonoBehaviour, ICollidable
{
    [SerializeField] private UnityEvent _hazardEffectEvent;
    private Transform _collidedTransform;

    public void OnCollide(Transform other)
    {
        Debug.Log($"Hit by {other.name}");
        _collidedTransform = other;
        _hazardEffectEvent?.Invoke();
    }

    public void DamagePlayer(int amount)
    {
        if (_collidedTransform.TryGetComponent<PlayerInformation>(out PlayerInformation info))
        {
            info.CauseDamge(amount);
        }
    }

    public void HealPlayer(int amount)
    {
        if (_collidedTransform.TryGetComponent<PlayerInformation>(out PlayerInformation info))
        {
            info.HealDamage(amount);
        }
    }
}
