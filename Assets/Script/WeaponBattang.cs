using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class WeaponBattang : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(BattarangTime());
    }

    IEnumerator BattarangTime()
    {
        yield return new WaitForSeconds(10f);

        Destroy(gameObject);
    }
}
