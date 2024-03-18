using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

// Ваш скрипт загрузки ресурсов или сцены
public class YourLoaderScript : MonoBehaviour
{
    // public async void OpenScene()
    // {
    //     using (new Load("s"))
    //     {
    //         AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);

    //         // Ждем завершения загрузки сцены
    //         while (!asyncLoad.isDone)
    //         {
    //             await Task.Yield();
    //         }

    //         // Загрузка завершена
    //         Debug.Log("Сцена загружена.");
    //     }
    // }

    // IEnumerator LoadYourResourcesOrScene()
    // {
    //     GC.Collect();
    //     LoadingScreen.Show();
    //     AsyncOperation async = SceneManager.LoadSceneAsync(1);
    //     async.allowSceneActivation = false;

    //     float multiplier = 3f;

    //     while (LoadingScreen.GetProgress() < 0.9f)
    //     {
    //         yield return new WaitForSeconds(multiplier * 0.01f);
    //         LoadingScreen.SetProgress(LoadingScreen.GetProgress() + 0.01f);
    //     }

    //     while (!async.isDone)
    //     {
    //         if (async.progress >= 0.9f)
    //         {
    //             LoadingScreen.SetProgress(1.0f);
    //             yield return new WaitForSeconds(0.2f);
    //             async.allowSceneActivation = true;
    //         }
    //     }
    // }

    // IEnumerator LoadYourResourcesOrScene1()
    // {
    //     AsyncOperation loadOperation = SceneManager.LoadSceneAsync(1);
    //     // loadOperation.allowSceneActivation = false;
    //     float currentProgress = 0;
    //     float a = 0;
    //     float e = 0;

    //     while (!loadOperation.isDone)
    //     {
    //         yield return null;
    //         e += Time.deltaTime;
    //         float targetProgress = Mathf.Clamp01(loadOperation.progress / 0.9f);
    //         currentProgress = Mathf.Lerp(0, targetProgress, e / 2);
    //         LoadingScreen.SetProgress(currentProgress);

    //     }

    //     // loadOperation.allowSceneActivation = true;
    // }
}
