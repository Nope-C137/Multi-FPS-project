using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void ReturnButton()
    {
        SceneManager.LoadScene("SelectScene");
    }

   public void ExitButton()
    {
        Application.Quit();
    }
}
