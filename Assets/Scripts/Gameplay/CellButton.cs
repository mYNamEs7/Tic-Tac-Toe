using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellButton : MonoBehaviour
{
    public char Value => char.Parse(text.text);

    [SerializeField] private char playerMove = 'x';

    private Button button;
    private TMP_Text text;
    private Image image;

    void Awake()
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<TMP_Text>();
        image = GetComponent<Image>();
        text.text = " ";

        button.onClick.AddListener(OnMakeMove);
    }

    private void OnMakeMove()
    {
        if (Opponent.Instance.IsMakeMove)
            return;

        if (Value == ' ')
            MakeMove(playerMove);
        else
            return;

        if (GameField.Instance.CheckWin(playerMove, out CellButton[] winCells))
        {
            GameController.Instance.GameOver(winCells, Players.Player);
            return;
        }

        Opponent.Instance.MakeMove();
    }

    public void MakeMove(char move)
    {
        text.text = move.ToString();
    }

    public void RemoveButton()
    {
        button.onClick.RemoveAllListeners();
    }

    public void MarkCell(Color color)
    {
        image.color = color;
    }
}
