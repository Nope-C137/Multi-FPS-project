using Photon.Pun.Demo.Asteroids;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class EnemyAiBasic : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public int Health;

    public GameObject newAmmoPrefab;

    //Platroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectTile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    Animator batanim;
    public AudioSource battarangSound;
    public AudioSource batmanvoice;

    public delegate void EnemyKilled();
    public static event EnemyKilled OnEnemyKilled;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        batanim = GetComponent<Animator>();
    }

    private void Update()
    {
        player = GameObject.FindWithTag("Player").transform;

        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        batanim.SetBool("Attack", false);
        batmanvoice.Stop();

        if(walkPointSet)
        {
            //agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //WalkPoint reached
        if(distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    [PunRPC]
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);

        batanim.SetBool("Attack", false);

        batmanvoice.Play();
    }

    [PunRPC]
    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        batanim.SetBool("Attack", true);

        if(!alreadyAttacked)
        {
            ///Attack code here!
            Rigidbody rb = Instantiate(projectTile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            battarangSound.Play();


            ///
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;

        batanim.SetBool("Attack", false);
        battarangSound.Stop();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            TakeDamge(15);
            Debug.Log("Damage Enemy Ai");
        }
    }

    [PunRPC]
    private void TakeDamge(int damage)
    {
        Health -= damage;

        if (Health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);

    }

    [PunRPC]
    private void DestroyEnemy()
    {
        PhotonNetwork.LocalPlayer.AddScore(100);

        GameObject secondObject = Instantiate(newAmmoPrefab, transform.position, Quaternion.identity);
        secondObject.transform.parent = transform.parent;

        if (OnEnemyKilled != null)
        {
            OnEnemyKilled();
        }

        batmanvoice.Stop();
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    
}
