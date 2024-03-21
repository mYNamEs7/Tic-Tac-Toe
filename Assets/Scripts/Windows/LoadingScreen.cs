using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : SingletonWindow<LoadingScreen>
{
    [Header("Paramenters")]
    [SerializeField] private string defaultText = "Загрузка...";

    [Header("UI")]
    [SerializeField] private Slider progressBar;
    [SerializeField] private Text progressText;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(transform.parent.gameObject);
        Hide();
    }

    public void SetProgress(float progress, string text)
    {
        progressBar.value = Mathf.Clamp01(progress);
        progressText.text = text;
    }

    public void Show(string sceneName, string mainText, string onCompleteText)
    {
        SetProgress(0, defaultText);
        Show();
        StartCoroutine(ShowLoader(sceneName, mainText, onCompleteText));
    }

    private IEnumerator ShowLoader(string sceneName, string mainText, string onCompleteText)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;
        async.completed += (async) => Hide();

        float multiplier = 1.5f;

        while (progressBar.value < 0.9f)
        {
            yield return new WaitForSeconds(multiplier * 0.01f);
            SetProgress(progressBar.value + 0.01f, mainText);
        }

        while (!async.isDone)
        {
            if (async.progress >= 0.9f)
            {
                SetProgress(1.0f, onCompleteText);
                yield return new WaitForSeconds(0.2f);
                async.allowSceneActivation = true;
                break;
            }
        }
    }
}
