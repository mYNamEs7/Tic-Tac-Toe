using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameField : MonoBehaviour
{
    public static GameField Instance;

    public static event Action<CellButton[,]> OnCellsFilled;

    public CellButton[,] Cells => cells;

    [SerializeField] private CellButton cell;

    private CellButton[,] cells;
    private int size;
    private GridLayoutGroup grid;

    void Awake()
    {
        Instance = this;

        grid = GetComponent<GridLayoutGroup>();
    }

    void Start()
    {
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

        SetCellsSize();
        SpawnCells();

        OnCellsFilled?.Invoke(cells);
    }

    private void SetCellsSize()
    {
        // назначаем размер ячеек в зависимости от размера поля
        float height = 830;
        float space = grid.spacing.x;
        float cellSize = (height - space * (size + 1)) / size;

        grid.cellSize = new Vector2(cellSize, cellSize);
    }

    private void SpawnCells()
    {
        cells = new CellButton[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                cells[i, j] = Instantiate(cell, transform);
            }
        }
    }

    private bool CheckRow(CellButton[,] cells, int row, int size, char symbol, int winSequence, out CellButton[] winCells)
    {
        winCells = new CellButton[] { };
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
                        winCells = winCells.Append(cells[row, j]).ToArray();
                    }
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

    private bool CheckColumn(CellButton[,] cells, int col, int size, char symbol, int winSequence, out CellButton[] winCells)
    {
        winCells = new CellButton[] { };
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
                        winCells = winCells.Append(cells[j, col]).ToArray();
                    }
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

    private bool CheckDiagonal(CellButton[,] cells, int size, char symbol, int winSequence, out CellButton[] winCells)
    {
        winCells = new CellButton[] { };
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
                        winCells = winCells.Append(cells[i + k, j + k]).ToArray();
                    }
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
                        winCells = winCells.Append(cells[i + k, j - k]).ToArray();
                    }
                    return true;
                }
            }
        }

        return false;
    }

    public bool CheckWin(char symbol, out CellButton[] winCells)
    {
        int winSequence = GameController.Instance.SeriesLength;

        for (int i = 0; i < size; i++)
        {
            if (CheckRow(cells, i, size, symbol, winSequence, out winCells))
                return true;
        }

        for (int i = 0; i < size; i++)
        {
            if (CheckColumn(cells, i, size, symbol, winSequence, out winCells))
                return true;
        }

        if (CheckDiagonal(cells, size, symbol, winSequence, out winCells))
            return true;

        return false;
    }
}
