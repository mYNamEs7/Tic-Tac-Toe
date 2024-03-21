using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData : MonoBehaviour
{
    public static GameData Instance { get; private set; }

    [HideInInspector] public int trophiesCount;
    [HideInInspector] public int levelNumber;
    [HideInInspector] public string playerName;
    [HideInInspector] public string[] boughtItems;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        LoadData();
    }

    private void LoadData()
    {
        GameData gameData = GetComponent<GameData>();
        FileDataHandler fileDataHandler = new(Application.persistentDataPath, "data.json");
        fileDataHandler.Load(ref gameData);
    }

    public void SaveData()
    {
        GameData gameData = GetComponent<GameData>();
        FileDataHandler fileDataHandler = new(Application.persistentDataPath, "data.json");
        fileDataHandler.Save(gameData);
    }
}
