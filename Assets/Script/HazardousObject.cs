using UnityEngine;
using UnityEngine.Events;

public class HazardousObject : MonoBehaviour, ICollidable
{
    [SerializeField] private UnityEvent _hazardEffectEvent;

    public void OnCollide(Transform other)
    {
        Debug.Log($"Hit by {other.name}");
        _hazardEffectEvent?.Invoke();
    }

    public void DamagePlayer(int amount)
    {
        GameObject.FindFirstObjectByType<PlayerInformation>().CauseDamge(amount);
    }

    public void HealPlayer(int amount) 
    {
        GameObject.FindFirstObjectByType<PlayerInformation>().HealDamage(amount);
    }
}
