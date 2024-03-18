using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameField : MonoBehaviour
{
    public static GameField Instance;

    public static Action<CellButton[,]> OnCellsFilled;

    [SerializeField] private CellButton cell;

    private CellButton[,] cells;
    private int size;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GridLayoutGroup grid = GetComponent<GridLayoutGroup>();

        int fieldSize;
        string[] fieldSizeNums = GameController.Instance.FieldSize.Split('x');

        size = int.Parse(fieldSizeNums[0]);

        if (fieldSizeNums.Length > 2)
        {
            Debug.LogError("Неправильный размер поля");
            return;
        }

        fieldSize = size * size;

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = size;

        // назначаем размер ячеек в зависимости от размера поля
        float height = 830;
        float space = grid.spacing.x;
        float cellSize = (height - space * (size + 1)) / size;

        grid.cellSize = new Vector2(cellSize, cellSize);

        cells = new CellButton[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                cells[i, j] = Instantiate(cell, transform);
            }
        }

        OnCellsFilled?.Invoke(cells);
    }

    private bool CheckRow(CellButton[,] cells, int row, int size, char symbol, int winSequence)
    {
        int count = 0;
        for (int i = 0; i < size; i++)
        {
            if (cells[row, i].Value == symbol)
            {
                count++;
                if (count == winSequence)
                {
                    for (int j = i - winSequence + 1; j <= i; j++)
                    {
                        cells[row, j].MarkCell();
                    }
                    GameOver();
                    return true;
                }
            }
            else
            {
                count = 0;
            }
        }
        return false;
    }

    private bool CheckColumn(CellButton[,] cells, int col, int size, char symbol, int winSequence)
    {
        int count = 0;
        for (int i = 0; i < size; i++)
        {
            if (cells[i, col].Value == symbol)
            {
                count++;
                if (count == winSequence)
                {
                    for (int j = i - winSequence + 1; j <= i; j++)
                    {
                        cells[j, col].MarkCell();
                    }
                    GameOver();
                    return true;
                }
            }
            else
            {
                count = 0;
            }
        }
        return false;
    }

    private bool CheckDiagonal(CellButton[,] cells, int size, char symbol, int winSequence)
    {
        for (int i = 0; i <= size - winSequence; i++)
        {
            for (int j = 0; j <= size - winSequence; j++)
            {
                bool win = true;
                for (int k = 0; k < winSequence; k++)
                {
                    if (cells[i + k, j + k].Value != symbol)
                    {
                        win = false;
                        break;
                    }
                }
                if (win)
                {
                    for (int k = 0; k < winSequence; k++)
                    {
                        cells[i + k, j + k].MarkCell();
                    }
                    GameOver();
                    return true;
                }
            }
        }

        for (int i = 0; i <= size - winSequence; i++)
        {
            for (int j = winSequence - 1; j < size; j++)
            {
                bool win = true;
                for (int k = 0; k < winSequence; k++)
                {
                    if (cells[i + k, j - k].Value != symbol)
                    {
                        win = false;
                        break;
                    }
                }
                if (win)
                {
                    for (int k = 0; k < winSequence; k++)
                    {
                        cells[i + k, j - k].MarkCell();
                    }
                    GameOver();
                    return true;
                }
            }
        }

        return false;
    }

    public bool CheckWin(char symbol)
    {
        int winSequence = GameController.Instance.SeriesLength;

        for (int i = 0; i < size; i++)
        {
            if (CheckRow(cells, i, size, symbol, winSequence))
                return true;
        }

        for (int i = 0; i < size; i++)
        {
            if (CheckColumn(cells, i, size, symbol, winSequence))
                return true;
        }

        if (CheckDiagonal(cells, size, symbol, winSequence))
            return true;

        return false;
    }

    private async void GameOver()
    {
        RemoveButtonsFromAllCells();
        await Task.Delay(3000);
        LoadingScreen.Instance.Show("MainMenu");
    }

    private void RemoveButtonsFromAllCells()
    {
        foreach (var cell in cells)
        {
            cell.RemoveButton();
        }
    }
}
