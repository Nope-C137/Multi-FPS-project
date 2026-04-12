using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyAI_1 : MonoBehaviour
{
    [Header("Enemy AI Settings")]
    public Transform player;
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    public float groundCheckDistance = 1.1f;
    public LayerMask groundLayer;
    public float gravityForce = 20f;

    [Header("Climb")]
    public float climbSpeed = 3f;
    public float wallCheckDistance = 1f;
    public LayerMask wallLayer;
    public float climbHopForce = 5f; // New: force to hop over the ledge
    private bool wasClimbing = false; // Add this at the top of the class

    [Header("Attack Settings")]
    private bool isAttacking = false;
    bool alreadyAttacked;
    public float timeBetweenAttacks;
    public float attackRange = 2f; // You can adjust this range

    private Rigidbody rb;
    EnemyAttackAnimation enemyAttackAnimation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        enemyAttackAnimation = GetComponent<EnemyAttackAnimation>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (player == null)
        {
            StartCoroutine(SpawnDelay());
        }
    }

    void FixedUpdate()
    {
        if (player == null || isAttacking) return;

        // Direction toward player (for rotation)
        Vector3 direction = (player.position - transform.position);
        direction.y = 0;
        direction = direction.normalized;

        // Wall detection (in front of enemy)
        bool wallAhead = Physics.Raycast(transform.position, transform.forward, wallCheckDistance, wallLayer);

        if (wallAhead)
        {
            // Stick to wall and move up
            rb.useGravity = false;
            rb.linearVelocity = Vector3.up * climbSpeed;
        }
        else
        {
            // If we were climbing last frame, apply a hop force to clear the ledge
            if (wasClimbing)
            {
                rb.useGravity = true;
                // Apply a hop forward and up
                Vector3 hopDirection = (transform.forward + Vector3.up).normalized;
                rb.AddForce(hopDirection * climbHopForce, ForceMode.VelocityChange);
            }
            else
            {
                rb.useGravity = true;
            }

            bool isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
            rb.linearVelocity = new Vector3(direction.x * moveSpeed, rb.linearVelocity.y, direction.z * moveSpeed);

            if (!isGrounded)
            {
                rb.AddForce(Vector3.down * gravityForce, ForceMode.Acceleration);
            }
        }

        // Rotate to face the player
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));

        // Update climbing state for next frame
        wasClimbing = wallAhead;

        // Check attack range
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer(); // Call the attack method
        }

    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * wallCheckDistance);
    }

    IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(2f);
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    [PunRPC]
    private void AttackPlayer()
    {
        //Make sure enemy doesn't move

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            isAttacking = true; // Stop movement

            // Attack logic here...
            enemyAttackAnimation.PlayAttackAnimation();

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
        isAttacking = false; // Resume movement
        enemyAttackAnimation.StopAttackAnimation();
    }
}
