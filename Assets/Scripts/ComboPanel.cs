using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ComboPanel : Window
{
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button buyButton, closeButton;

    void Start()
    {
        closeButton.onClick.AddListener(Hide);
    }

    public void Init(string price, Item parentItem)
    {
        priceText.text = price;
        buyButton.onClick.AddListener(() => DialogWindow.Instance.ShowBuyWindow("Вы уверены что хотите совершить покупку?", this, parentItem));
    }

    public void PurchaseCompleted()
    {
        priceText.text = "Уже куплено";
        buyButton.onClick.RemoveAllListeners();
    }
}
