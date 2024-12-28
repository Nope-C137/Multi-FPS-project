using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class AsyncLoader : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject mainMenu;

    [SerializeField] private Slider loadSlider;

    public void LoadLevelBG(string levelToLoad)
    {
        loadingScreen.SetActive(true);
        mainMenu.SetActive(false);

        //Run the A sync
        StartCoroutine(LoadLevelAsync(levelToLoad));
    }

    IEnumerator LoadLevelAsync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);
        
        while(!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadSlider.value = progressValue;
            yield return null;
        }
    }
}
