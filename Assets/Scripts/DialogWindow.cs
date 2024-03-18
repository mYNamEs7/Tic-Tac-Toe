using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum Action { quitFromApplication, quitToMainMenu, BuyItem };

public class DialogWindow : Window
{
    public static DialogWindow Instance { get; private set; }

    [SerializeField] private TMP_Text text;
    [SerializeField] private Button okButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private TMP_Text okButtonText;
    [SerializeField] private TMP_Text cancelButtonText;
    [SerializeField] private Slider progressBar;

    private Action action;
    private string okButtonString;
    private string cancelButtonString;

    private ComboPanel comboPanel;
    private Item parentItem;

    void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(transform.parent);
        Hide();

        okButtonString = okButtonText.text;
        cancelButtonString = cancelButtonText.text;
    }

    public void Show(string message, Action action)
    {
        this.action = action;

        ResetButtons();
        InitButtons();
        ResetPanelElements();

        Show();
        text.text = message;
    }

    public void ShowBuyWindow(string message, ComboPanel comboPanel, Item parentItem)
    {
        action = Action.BuyItem;

        ResetButtons();
        InitButtons();
        ResetPanelElements();

        Show();
        text.text = message;
        this.comboPanel = comboPanel;
        this.parentItem = parentItem;
    }

    private void ResetPanelElements()
    {
        okButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(true);

        progressBar.gameObject.SetActive(false);
        progressBar.value = 0f;

        okButtonText.text = okButtonString;
        cancelButtonText.text = cancelButtonString;
    }

    private void ResetButtons()
    {
        okButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
    }

    private void InitButtons()
    {
        switch (action)
        {
            case Action.quitFromApplication:
                okButton.onClick.AddListener(() => Application.Quit());
                break;
            case Action.quitToMainMenu:
                okButton.onClick.AddListener(() => LoadingScreen.Instance.Show("MainMenu"));
                break;
            case Action.BuyItem:
                okButton.onClick.AddListener(BuyProcess);
                break;
            default:
                okButton.onClick.AddListener(Hide);
                break;
        }

        cancelButton.onClick.AddListener(Hide);
    }

    private void BuyProcess()
    {
        text.text = "Идет процесс покупки...";
        okButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(false);
        StartCoroutine(ShowLoader());
    }

    private IEnumerator ShowLoader()
    {
        progressBar.gameObject.SetActive(true);

        float multiplier = 1f;

        while (progressBar.value < 0.9f)
        {
            yield return new WaitForSeconds(multiplier * 0.01f);
            progressBar.value += 0.01f;
        }

        progressBar.value = 1.0f;
        yield return new WaitForSeconds(0.2f);

        progressBar.gameObject.SetActive(false);

        text.text = "Покупка прошла успешно!";

        EnableOkButton();

        if (comboPanel != null)
            comboPanel.PurchaseCompleted();
        parentItem.PurchaseCompleted();
    }

    private void EnableOkButton()
    {
        okButton.gameObject.SetActive(true);
        okButtonText.text = "ОК";
        okButton.onClick.RemoveAllListeners();
        okButton.onClick.AddListener(Hide);
    }
}
