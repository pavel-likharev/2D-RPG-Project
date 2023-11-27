using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_EndScreen : MonoBehaviour
{
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject diedText;
    [SerializeField] private GameObject restartButton;

    private void Start()
    {
        restartButton.GetComponent<Button>().onClick.AddListener(RestartGameButton);

        fadeScreen.gameObject.SetActive(true);
    }

    public void SwitchOnEndScreen()
    {
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCoroutine());
    }

    IEnumerator EndScreenCoroutine()
    {
        yield return new WaitForSeconds(1);
        diedText.SetActive(true);

        yield return new WaitForSeconds(1);
        restartButton.SetActive(true);
    }

    private void RestartGameButton() => GameManager.Instance.RestartScene();
}
