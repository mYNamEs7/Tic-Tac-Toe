using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellButton : MonoBehaviour
{
    public char Value => char.Parse(text.text);

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
            MakeMove('x');
        else
            return;

        if (GameField.Instance.CheckWin('x'))
        {
            print("x win");
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

    public void MarkCell()
    {
        image.color = Color.blue;
    }
}
