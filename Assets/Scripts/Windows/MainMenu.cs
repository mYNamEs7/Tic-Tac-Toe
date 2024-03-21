using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : SingletonWindow<MainMenu>
{
    [Header("Paramenters")]
    [SerializeField] private int fieldIncreaseRate = 5;
    [SerializeField] private int maxFieldSize = 7;
    [SerializeField] private int sequenceIncreaseRate = 10;
    [SerializeField] private int maxSequenceSize = 7;
    [SerializeField] private string onApplicationQuitText = "Вы уверены что хотите выйти из игры?";

    [Header("UI")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button shopButton;

    [SerializeField] private TMP_InputField playerName;
    [SerializeField] private TMP_Text levelNumber;
    [SerializeField] private TMP_Text fieldSize;
    [SerializeField] private TMP_Text seriesLength;

    [SerializeField] private TMP_Text trophiesCountText;
    [SerializeField] private TMP_Text winnerName;
    [SerializeField] private TMP_Text elapsedTime;
    [SerializeField] private GameObject gameResultsContainer;

    private string opponentName = "противник";

    protected override void Awake()
    {
        base.Awake();

        playButton.onClick.AddListener(() => LoadingScreen.Instance.Show("GameScene", "Загружается уровень...", "Открывается уровень..."));
        playButton.onClick.AddListener(SendData);
        shopButton.onClick.AddListener(() => ShopManager.Instance.Show());
    }

    void Start()
    {
        playerName.text = GameData.Instance.playerName;

        if (GameController.Instance.Winner != Players.None)
        {
            winnerName.text = GameController.Instance.Winner == Players.Player ? playerName.text : opponentName;
            elapsedTime.text = GameController.Instance.ElapsedTime;
        }
        else
            gameResultsContainer.SetActive(false);

        trophiesCountText.text = GameData.Instance.trophiesCount.ToString();

        int levelNum = GameData.Instance.levelNumber;
        levelNumber.text = levelNum.ToString();

        int startSize = 3;
        int endSize = ValueByLevel(startSize, levelNum, fieldIncreaseRate, 0, maxFieldSize - startSize);
        fieldSize.text = endSize + "x" + endSize;

        int startLength = 3;
        int endLength = ValueByLevel(startLength, levelNum, sequenceIncreaseRate, 0, maxSequenceSize - startLength);
        seriesLength.text = endLength.ToString();

        UpdateLayout();
    }

    private int ValueByLevel(int startValue, int level, int rate, int minValue, int maxValue)
    {
        return startValue + Mathf.Clamp(level / rate, minValue, maxValue);
    }

    void Update()
    {
        if (GameController.Instance.WindowsStack.Peek() == this && Input.GetKeyUp(KeyCode.Escape))
            DialogWindow.Instance.Show(onApplicationQuitText, Action.quitFromApplication);
    }

    void SendData()
    {
        GameController.Instance.FillData(playerName.text, opponentName, fieldSize.text, int.Parse(seriesLength.text));

        GameData.Instance.playerName = playerName.text;
        GameData.Instance.SaveData();
    }

    private void UpdateLayout()
    {
        Canvas.ForceUpdateCanvases();
        Hide();
        base.Show();
    }

    void OnApplicationQuit()
    {
        GameData.Instance.playerName = playerName.text;
        GameData.Instance.SaveData();
    }
}
