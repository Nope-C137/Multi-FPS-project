using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void SelectLevelMain()
    {
        SceneManager.LoadScene("SelectScene");
    }

    public void SelectLevelGame1()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void SelectLevelGame2()
    {
        SceneManager.LoadScene("Level 2");
    }

    public void SelectLevelGame3()
    {
        SceneManager.LoadScene("Level 3");
    }

    public void RetunMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
