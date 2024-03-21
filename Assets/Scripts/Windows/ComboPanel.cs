using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ComboPanel : SingletonWindow<ComboPanel>
{
    public GridLayoutGroup Container => container;

    [Header("Paramenters")]
    [SerializeField] private string confirmationText = "Вы уверены что хотите совершить покупку?";
    [SerializeField] private string purchaseCompletedText = "Уже куплено";

    [Header("UI")]
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button buyButton, closeButton;
    [SerializeField] private GridLayoutGroup container;

    void Start()
    {
        closeButton.onClick.AddListener(Hide);
    }

    public void Init(string price, Item parentItem)
    {
        priceText.text = price;
        buyButton.onClick.AddListener(() => DialogWindow.Instance.ShowBuyWindow(confirmationText, this, parentItem));
    }

    public void PurchaseCompleted()
    {
        priceText.text = purchaseCompletedText;
        buyButton.onClick.RemoveAllListeners();
    }
}
