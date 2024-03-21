using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : SingletonWindow<GameOverPanel>
{
    [Header("Paramenters")]
    [SerializeField] private string winText = "победа";
    [SerializeField] private string loseText = "поражение";
    [SerializeField] private float panelAlpha = 1f;
    [SerializeField] private float changeAlphaSpeed = 0.5f;
    [SerializeField] private float showTime = 1f;

    [Header("UI")]
    [SerializeField] private Image background;
    [SerializeField] private TMP_Text winTextUI;
    [SerializeField] private TMP_Text trophiesCountText;

    private Color bgColor;

    protected override void Awake()
    {
        base.Awake();

        Hide();
    }

    public void Show(Players winner, int trophiesCount, string elapsedTime)
    {
        Show();

        UpdateLayout();

        winTextUI.text = winner == Players.Player ? winText : loseText;
        trophiesCountText.text = trophiesCount > 0 ? "+" + trophiesCount : trophiesCount.ToString();

        GameController.Instance.SendGameResult(elapsedTime, winner);

        bgColor = background.color;
        bgColor.a = 0f;
        background.color = bgColor;

        StartCoroutine(ChangeAlpha(panelAlpha, changeAlphaSpeed));
    }

    private IEnumerator ChangeAlpha(float targetAlpha, float speed)
    {
        float elepsedTime = 0f;

        while (bgColor.a < targetAlpha)
        {
            elepsedTime += Time.deltaTime;
            bgColor.a = Mathf.Lerp(0, targetAlpha, elepsedTime / speed);
            background.color = bgColor;
            yield return null;
        }

        yield return new WaitForSeconds(showTime);
        LoadingScreen.Instance.Show("MainMenu", "Загружается главное меню...", "Открывается главное меню...");
    }

    private void UpdateLayout()
    {
        Canvas.ForceUpdateCanvases();
        Hide();
        base.Show();
    }
}
