using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;

    public GameObject player;

    [Space]
    public Transform[] spawnPoints;

    [Space]
    public GameObject roomCam;

    [Space]
    public GameObject nameUI;

    public GameObject connectingUI;

    public GameObject timer;

    public GameObject gameLeaderBoard;

    public GameObject audioSetting;

    private string nickName = "Unnamed";

    public string roomNameToJoin = "Test";

    [HideInInspector]
    public int kills = 0;
    public int deaths = 0;

    private void Awake()
    {
        instance = this;
    }

    public void ChangeNickname(string _name)
    {
        nickName = _name;
    }

    public void JoinRoomButtonPressed()
    {
        Debug.Log(message: "Connecting...");

        PhotonNetwork.JoinOrCreateRoom(roomNameToJoin, roomOptions: null, typedLobby: null);

        nameUI.SetActive(false);
        connectingUI.SetActive(true);
        audioSetting.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log(message: "We're connected and in a room now");

        roomCam.SetActive(false);
        timer.SetActive(true);
        gameLeaderBoard.SetActive(true);

        RespawnPlayer();

    }

    public void RespawnPlayer()
    {
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];

        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);
        _player.GetComponent<PlayerSetup>().IsLocalPlayer();
        _player.GetComponent<Health>().isLocalPlayer = true;

        _player.GetComponent<PhotonView>().RPC("SetNickName", RpcTarget.AllBuffered, nickName);
        PhotonNetwork.LocalPlayer.NickName = nickName;
    }

    public void SetHashes()
    {
        try
        {
            Hashtable hash = PhotonNetwork.LocalPlayer.CustomProperties;

            hash["kills"] = kills;
            hash["deaths"] = deaths;

            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
        catch
        {
            //Do nothing
        }
    }

}
