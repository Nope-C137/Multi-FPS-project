using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class Health : MonoBehaviour
{
    public int health;
    public bool isLocalPlayer;

    public RectTransform healthBar;

    private float originalHealthBarSize;


    [Header("UI")]
    public TextMeshProUGUI healthText;
    public GameObject gotHitScreen;


    private bool hasDied;

    private void Start()
    {
        originalHealthBarSize = healthBar.sizeDelta.x;
    }

    private void Update()
    {
        //healthBar.sizeDelta = new Vector2(originalHealthBarSize * health / 100f, healthBar.sizeDelta.y);

        if (gotHitScreen != null) 
        {
            if (gotHitScreen.GetComponent<Image>().color.a > 0)
            {
                var color = gotHitScreen.GetComponent<Image>().color;
                color.a -= 0.01f;

                gotHitScreen.GetComponent<Image>().color = color;
            }
        }
    }

    [PunRPC]
    public void TakeDamage(int _damage)
    {
        if (hasDied)
        {
            return;
        }

        health -= _damage;

        GotHurt();

        healthBar.sizeDelta = new Vector2(originalHealthBarSize * health / 100f, healthBar.sizeDelta.y);

        healthText.text = health.ToString();

        if(health <= 0)
        {
            hasDied = true;

            if (isLocalPlayer) 
            {
                RoomManager.instance.RespawnPlayer();

                RoomManager.instance.deaths++;
                RoomManager.instance.SetHashes();
            }

            Destroy(gameObject);
        }
    }

    void GotHurt()
    {
        var color = gotHitScreen.GetComponent<Image>().color;
        color.a = 0.8f;

        gotHitScreen.GetComponent<Image>().color = color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "battarang")
        {
            TakeDamage(15);
            Debug.Log("PlayerDamage");
        }
    }
}
