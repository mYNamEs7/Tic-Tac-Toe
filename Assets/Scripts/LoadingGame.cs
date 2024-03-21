using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingGame : MonoBehaviour
{
    void Start()
    {
        LoadingScreen.Instance.Show("MainMenu", "Запуск игры...", "Открытие игры...");
    }
}
