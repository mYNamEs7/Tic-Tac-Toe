using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    public static Opponent Instance;

    public bool IsMakeMove { get; private set; }

    private CellButton[,] cells;

    void Awake()
    {
        Instance = this;

        GameField.OnCellsFilled += GetFieldCells;
    }

    private void GetFieldCells(CellButton[,] cells)
    {
        this.cells = cells;
    }

    public async void MakeMove()
    {
        IsMakeMove = true;

        await Task.Delay(1000);

        var availableCells = cells.Cast<CellButton>().Where((cell) => cell.Value == ' ').ToList();
        availableCells[Random.Range(0, availableCells.Count())].MakeMove('o');

        if (GameField.Instance.CheckWin('o'))
            print("o win");

        IsMakeMove = false;
    }

    void OnDestroy()
    {
        GameField.OnCellsFilled -= GetFieldCells;
    }
}
