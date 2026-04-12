using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviourPun
{
    public int health;
    private int attackerViewID;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            TakeDamage(25);
        }
    }

    [PunRPC]
    private void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            PhotonNetwork.LocalPlayer.AddScore(100);

            int currentKills = PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("kills")
            ? (int)PhotonNetwork.LocalPlayer.CustomProperties["kills"] : 0;

            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
            {
                { 
                    "kills", currentKills + 1 
                }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);

            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }

    [PunRPC]
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
