using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBullet : MonoBehaviour
{
    //Assingables
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whatIsEnemies;

    //Stat
    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;

    //Damage
    public int explosionDamage;
    public float explosionRange;
}
