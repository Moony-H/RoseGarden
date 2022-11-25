using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class LoadingSceneManager : MonoBehaviour
{
    public static int nextSceneNumber;
    public static string LoadingText;

    public Image ProgressBar;
    public TextMeshProUGUI TextBox;

    public static void LoadScene(int sceneNumber, string loadingText)
    {
        LoadingText = loadingText;
        nextSceneNumber = sceneNumber;

        SceneManager.LoadScene(1);
    }

    public static void LoadScene(int sceneNumber)
    {
        LoadingText = "Now Loading...";
        nextSceneNumber = sceneNumber;

        SceneManager.LoadScene(1);
    }

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    public IEnumerator LoadScene()
    {
        TextBox.text = LoadingText;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneNumber);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime;

            if (op.progress < 0.9f)
            {
                ProgressBar.fillAmount = Mathf.Lerp(ProgressBar.fillAmount, op.progress, timer);
                if (ProgressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                ProgressBar.fillAmount = Mathf.Lerp(ProgressBar.fillAmount, 1f, timer);
                if (ProgressBar.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }

    }
}
