using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSetting : MonoBehaviour
{
    public GameObject audioMenu;
    private bool isLocked;
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            audioMenu.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            isLocked = false;
        }    

    }

    public void Onresume()
    {
        audioMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isLocked = true;

    }
}
