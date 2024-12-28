using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RestoreBullet : MonoBehaviour
{
    Weapon weapon1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            weapon1 = FindAnyObjectByType<Weapon>();
            weapon1.ReloadMag();

            Debug.Log("ReloadMag");
            Destroy(gameObject);
        }
    }
}
