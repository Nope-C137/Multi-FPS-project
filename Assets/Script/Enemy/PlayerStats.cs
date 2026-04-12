using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth; // Initialize current health to max health at the start
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reduce current health by the damage taken
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject); // Destroy the player object when they die
    }
}
