using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using WebSocketSharp;
using Photon.Pun;

public class GameChat : MonoBehaviour
{
    public TextMeshProUGUI chatText;
    public TMP_InputField inputField;


    private bool isInputFieldToggLed;

    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Y) && !isInputFieldToggLed)
        {
            isInputFieldToggLed = true;

            inputField.Select();
            inputField.ActivateInputField();

            Debug.Log("Toggle on");
        }

        if(Input.GetKeyDown(KeyCode.Escape) && isInputFieldToggLed)
        {
            isInputFieldToggLed = false;

            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);

            Debug.Log("Toggle off");
        }

        //Sending Message
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && isInputFieldToggLed && !inputField.text.IsNullOrEmpty())
        {
            //Sending a Message

            string messageToSend = $"{PhotonNetwork.LocalPlayer.NickName}: {inputField.text}";

            GetComponent<PhotonView>().RPC("SendChatMessages", RpcTarget.All, messageToSend);

            inputField.text = "";

            isInputFieldToggLed = false;

            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);

            Debug.Log("Message sent");
        }
    }

    [PunRPC]
    public void SendChatMessages(string _message)
    {
        chatText.text = chatText.text + "\n" + _message;
    }
}
