using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "Game";
    [SerializeField] private GameObject continueButton;
    [SerializeField] private UI_FadeScreen fadeScreen;

    [SerializeField] private float loadDelay = 1.5f;

    private void Start()
    {
        if (SaveManager.Instance.HasSavedData())
            continueButton.SetActive(true);
        else
            continueButton.SetActive(false);
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadSceneWithFadeEffect(loadDelay));
    }

    public void NewGame()
    {
        SaveManager.Instance.DeleteSavedData();
        StartCoroutine(LoadSceneWithFadeEffect(loadDelay));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadSceneWithFadeEffect(float delay)
    {
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(sceneName);
    }
}
