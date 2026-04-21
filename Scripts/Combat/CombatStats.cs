using System;
using UnityEngine;

public class CombatStats : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth = 100;

    [Header("Attack")]
    [SerializeField] private int attackDamage = 10;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;
    public int AttackDamage => attackDamage;
    public bool IsDead => currentHealth <= 0;

    public event Action<int> OnDamageTaken;
    public event Action OnDeath;
    public event Action OnRespawned;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (IsDead) return;

        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

        Debug.Log($"{gameObject.name} recebeu {damage} de dano. HP: {currentHealth}/{maxHealth}");

        OnDamageTaken?.Invoke(damage);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} morreu.");
        OnDeath?.Invoke();
    }

    public void RestoreFullHealth()
    {
        currentHealth = maxHealth;
        Debug.Log($"{gameObject.name} teve a vida restaurada. HP: {currentHealth}/{maxHealth}");
        OnRespawned?.Invoke();
    }
}