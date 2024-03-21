using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : SingletonWindow<PausePanel>
{
    [Header("Paramenters")]
    [SerializeField] private string mainMenuQuitText = "Вы уверены, что хотите выйти в главное меню? Весь прогресс будет потерян.";

    [Header("UI")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button mainMenuButton;

    protected override void Awake()
    {
        base.Awake();

        mainMenuButton.onClick.AddListener(OnMainMenuClick);
        backButton.onClick.AddListener(Hide);
        Hide();
    }

    private void OnMainMenuClick()
    {
        DialogWindow.Instance.Show(mainMenuQuitText, Action.quitToMainMenu);
        Time.timeScale = 1f;
    }

    public override void Show()
    {
        base.Show();

        Time.timeScale = 0f;
    }

    public override void Hide()
    {
        base.Hide();

        Time.timeScale = 1f;
    }
}
