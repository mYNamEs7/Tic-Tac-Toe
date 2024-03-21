using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataHandler : MonoBehaviour
{
    private readonly string dataDirPath;
    private readonly string dataFileName;

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public void Load(ref GameData gameData)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        gameData.trophiesCount = 0;
        gameData.levelNumber = 1;
        gameData.playerName = "игрок";
        gameData.boughtItems = new string[] { };

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using FileStream stream = new(fullPath, FileMode.Open);
                {
                    using StreamReader reader = new(stream);
                    dataToLoad = reader.ReadToEnd();
                }

                JsonUtility.FromJsonOverwrite(dataToLoad, gameData);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            using FileStream stream = new(fullPath, FileMode.Create);
            using StreamWriter writer = new(stream);
            writer.Write(dataToStore);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}
