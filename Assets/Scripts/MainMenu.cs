using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button shopButton;

    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text levelNumber;
    [SerializeField] private TMP_Text fieldSize;
    [SerializeField] private TMP_Text seriesLength;

    void Awake()
    {
        playButton.onClick.AddListener(() => LoadingScreen.Instance.Show("GameScene"));
        playButton.onClick.AddListener(SendData);
        shopButton.onClick.AddListener(() => ShopManager.Instance.Show());
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            DialogWindow.Instance.Show("Вы уверены что хотите выйти из игры?", Action.quitFromApplication);
        }
    }

    void SendData()
    {
        GameController.Instance.FillData(playerName.text, "противник", int.Parse(levelNumber.text), fieldSize.text, int.Parse(seriesLength.text));
    }
}
