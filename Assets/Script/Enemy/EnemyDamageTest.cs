using UnityEngine;

public class EnemyDamageTest : MonoBehaviour
{
    public int damageAmount = 20; // Amount of damage the enemy will deal

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = other.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damageAmount); // Call the TakeDamage method on the PlayerStats component
            }
        }
    }
}
