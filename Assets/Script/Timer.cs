using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using Photon.Realtime;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remaingTime;
    public GameObject gameOverleaderboard;
    public GameObject audioSetting;

    void Update()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            RPC_CountDownTimer(name);
        }
        string countDown = JsonUtility.ToJson(timerText.text);
        GetComponent<PhotonView>().RPC("RPC_CountDownTimer", RpcTarget.OthersBuffered, countDown);
    }

    [PunRPC]
    public void RPC_CountDownTimer(string _timer)
    {
        if (remaingTime > 0)
        {
            remaingTime -= Time.deltaTime;
        }
        else if (remaingTime < 0)
        {
            remaingTime = 0;
            //GameOver()
            timerText.color = Color.red;
            gameOverleaderboard.SetActive(true);
            audioSetting.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            GameObject.FindWithTag("Player").GetComponent<Movement>().enabled = false;
            GameObject.FindWithTag("sound").SetActive(false);

        }

        remaingTime -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(remaingTime / 60);
        int seconds = Mathf.FloorToInt(remaingTime % 60);
        _timer = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerText.text = _timer;
        
    }
}