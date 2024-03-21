using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public enum Players { None, Player, Opponent };

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public string PlayrName { get; set; }
    public string OpponentName { get; set; }
    public string FieldSize { get; set; }
    public int SeriesLength { get; set; }

    public string ElapsedTime { get; set; }
    public Players Winner { get; set; }

    public Stack<Window> WindowsStack { get; set; } = new Stack<Window>();

    [Header("Paramenters")]
    [SerializeField] private int outTimeMS = 2000;
    [SerializeField] private int trophiesOnWin = 100;
    [SerializeField] private int trophiesOnLose = 100;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void FillData(string playrName, string opponentName, string fieldSize, int seriesLength)
    {
        PlayrName = playrName;
        OpponentName = opponentName;
        FieldSize = fieldSize;
        SeriesLength = seriesLength;
    }

    public void SendGameResult(string elapsedTime, Players winner)
    {
        ElapsedTime = elapsedTime;
        Winner = winner;
    }

    public async void GameOver(CellButton[] winCells, Players winner)
    {
        GamePanel.Instance.StopTimer();

        foreach (var cell in winCells)
        {
            cell.MarkCell(Color.blue);
        }

        foreach (var cell in GameField.Instance.Cells)
        {
            cell.RemoveButton();
        }

        await Task.Delay(outTimeMS);

        int trophiesCount = winner == Players.Player ? trophiesOnWin : -trophiesOnLose;
        GameOverPanel.Instance.Show(winner, trophiesCount, GamePanel.Instance.GetTimerValue());

        GameData.Instance.trophiesCount = Mathf.Clamp(GameData.Instance.trophiesCount + trophiesCount, 0, int.MaxValue);
        if (winner == Players.Player)
            GameData.Instance.levelNumber++;
        GameData.Instance.SaveData();
    }
}
