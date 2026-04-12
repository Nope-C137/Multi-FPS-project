using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using TMPro;
using Photon.Pun.UtilityScripts;
public class LeaderBoard : MonoBehaviour
{
    public GameObject PlayerHolder;

    [Header("Options")]
    public float refreshRate = 1f;

    [Header("UI")]
    public GameObject[] slots;

    [Space]
    public TextMeshProUGUI[] scoreTexts;
    public TextMeshProUGUI[] nameTexts;
    public TextMeshProUGUI[] kdTexts;


    private void Start()
    {
        InvokeRepeating(nameof(Refresh), 1f, refreshRate);
    }

    public void Refresh()
    {
        foreach (var slot in slots) 
        {
            slot.SetActive(false);
        }

        // Sort players by Photon score (descending)
        var sortedPlayerList = PhotonNetwork.PlayerList
            .OrderByDescending(p => p.GetScore())
            .ToList();

        //var sortedPlayerList = (from player in PhotonNetwork.PlayerList orderby player.GetScore() descending select player).ToList();

        int i = 0;
        foreach (var player in sortedPlayerList) 
        {
            //slots[i].SetActive(true);

            //if (player.NickName == "")
            //    player.NickName = "Unnamed";

            //nameTexts[i].text = player.NickName;
            //scoreTexts[i].text = player.GetScore().ToString();

            //if (player.CustomProperties["kills"] != null)
            //{
            //    kdTexts[i].text = player.CustomProperties["kills"] + "/" + player.CustomProperties["deaths"];
            //}
            //else
            //{
            //    kdTexts[i].text = "0/0";
            //}

            //i++;

            if (i >= slots.Length) break;

            slots[i].SetActive(true);

            // Handle empty nickname
            string nickname = string.IsNullOrEmpty(player.NickName) ? "Unnamed" : player.NickName;
            nameTexts[i].text = nickname;

            // Display Photon score
            scoreTexts[i].text = player.GetScore().ToString();

            // Display K/D from custom properties
            int kills = player.CustomProperties.ContainsKey("kills") ? (int)player.CustomProperties["kills"] : 0;
            int deaths = player.CustomProperties.ContainsKey("deaths") ? (int)player.CustomProperties["deaths"] : 0;
            kdTexts[i].text = $"{kills}/{deaths}";

            i++;

        }
    }

    private void Update()
    {
        //PlayerHolder.SetActive(Input.GetKey(KeyCode.Tab));
    }
}
