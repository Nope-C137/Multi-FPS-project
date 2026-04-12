using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerCountdown : MonoBehaviourPun
{
    public GameObject enemySpawn;
    public TextMeshProUGUI countdownText; // Or use UnityEngine.UI.Text
    private int countdownTime = 3;
    public CanvasGroup canvasGroup;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("StartCountdown", RpcTarget.All);
        }
    }

    [PunRPC]
    void StartCountdown()
    {
        StartCoroutine(CountdownCoroutine());
    }

    IEnumerator CountdownCoroutine()
    {
        canvasGroup.alpha = 1; // Make sure the canvas is visible
        countdownText.text = "Don't Fall Asleep";
        yield return new WaitForSeconds(5f);

        while (countdownTime > 0)
        {
            countdownText.text = countdownTime.ToString();
            yield return new WaitForSeconds(2f);
            countdownTime--;
        }
        countdownText.text = "Fright Night!";
        enemySpawn.SetActive(true);
        yield return new WaitForSeconds(2f);
        canvasGroup.alpha = 0; // Hide the canvas after countdown
        countdownText.gameObject.SetActive(false);
    }
       
}
