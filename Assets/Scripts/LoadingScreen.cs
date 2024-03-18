using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : Window
{
    public static LoadingScreen Instance { get; private set; }

    public Slider progressBar;
    public Text progressText;

    void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(transform.parent);
        Hide();
    }

    public void SetProgress(float progress, string text)
    {
        progressBar.value = Mathf.Clamp01(progress);
        progressText.text = text;
    }

    public void Show(string sceneName)
    {
        SetProgress(0, "Загрузка...");
        Show();
        StartCoroutine(ShowLoader(sceneName));
    }

    private IEnumerator ShowLoader(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;
        async.completed += (async) => Hide();

        float multiplier = 1.5f;

        while (progressBar.value < 0.9f)
        {
            yield return new WaitForSeconds(multiplier * 0.01f);
            SetProgress(progressBar.value + 0.01f, "Загрузка сцены...");
        }

        while (!async.isDone)
        {
            if (async.progress >= 0.9f)
            {
                SetProgress(1.0f, "Откртыие сцены...");
                yield return new WaitForSeconds(0.2f);
                async.allowSceneActivation = true;
                break;
            }
        }
    }
}
