using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public string PlayrName { get; set; }
    public string OpponentName { get; set; }
    public int LevelNumber { get; set; }
    public string FieldSize { get; set; }
    public int SeriesLength { get; set; }

    void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void FillData(string playrName, string opponentName, int levelNumber, string fieldSize, int seriesLength)
    {
        PlayrName = playrName;
        OpponentName = opponentName;
        LevelNumber = levelNumber;
        FieldSize = fieldSize;
        SeriesLength = seriesLength;
    }
}
