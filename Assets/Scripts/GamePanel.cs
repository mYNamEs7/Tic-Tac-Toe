using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamePanel : MonoBehaviour
{
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text opponentName;
    [SerializeField] private TMP_Text levelNumber;

    void Start()
    {
        playerName.text = GameController.Instance.PlayrName;
        opponentName.text = GameController.Instance.OpponentName;
        levelNumber.text += GameController.Instance.LevelNumber.ToString();
    }
}
