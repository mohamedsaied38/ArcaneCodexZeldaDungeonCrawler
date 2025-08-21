using System;
using UnityEngine;

public class PlayerInformation : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _defensePoints;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private int _attackDamage = 10;

    Animator _animator;

    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;
    public float Speed => _speed;
    public int AttackDamage => _attackDamage;


    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void CauseDamge(int damageAmount)
    {
        if (_defensePoints > 0 && damageAmount > 0)
        {
            damageAmount -= _defensePoints;
            if (damageAmount < 0) 
                damageAmount = 0;
        }

        _currentHealth -= damageAmount;
        if (_currentHealth < 0) 
            _currentHealth = 0;

        UIManager.Instance.UpdateHealth(_currentHealth);
        _animator.SetTrigger("Damage");

        if (_currentHealth == 0)
            Destroy(this.gameObject); //Change Later
    }

    public void HealDamage(int healAmount)
    {
        _currentHealth += healAmount;
        if ( _currentHealth > _maxHealth)
            _currentHealth = _maxHealth;

        UIManager.Instance.UpdateHealth( _currentHealth);
    }   

    /// <summary>
    /// Equip(+) or unEquip(-) armor items
    /// </summary>
    public void EquipArmor(int armorDefense)
    {
        _defensePoints += armorDefense;
    }
}
