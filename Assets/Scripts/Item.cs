using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Image image;
    [SerializeField] private Image iconImage;
    private ItemImageSO[] itemImages;
    private Button buyButton;

    void Awake()
    {
        itemImages = Resources.LoadAll<ItemImageSO>(@"ScriptableObjects\ItemImages");
        buyButton = GetComponent<Button>();
    }

    public void Init(ShopItem shopItem, ComboPanel comboPanel)
    {
        nameText.text = shopItem.key;
        if (shopItem.amount != 0)
            amountText.text = shopItem.amount.ToString();
        else
        {
            Destroy(amountText.gameObject);
            Destroy(iconImage.gameObject);
            nameText.alignment = TextAlignmentOptions.Midline;
        }

        priceText.text = shopItem.price + " " + shopItem.currency;

        SetImages(shopItem.key);
        InitBuyButton(comboPanel);
    }

    public void Init(ItemDetails itemDetails)
    {
        nameText.text = itemDetails.key;
        if (itemDetails.amount != 0)
            amountText.text = itemDetails.amount.ToString();
        else
            amountText.text = itemDetails.damage.ToString();

        SetImages(itemDetails.key);
        buyButton.onClick.RemoveAllListeners();
    }

    private void SetImages(string key)
    {
        var itemImage = itemImages.First((itemImage) => itemImage.key == key);
        image.sprite = itemImage.mainImage;
        iconImage.sprite = itemImage.iconImage;
    }

    private void InitBuyButton(ComboPanel comboPanel)
    {
        if (comboPanel != null)
            buyButton.onClick.AddListener(() => comboPanel.Show());
        else
            buyButton.onClick.AddListener(() => DialogWindow.Instance.ShowBuyWindow("Вы уверены что хотите совершить покупку?", comboPanel, this));
    }

    public void PurchaseCompleted()
    {
        priceText.text = "Уже куплено";
        GetComponent<Button>().onClick.RemoveAllListeners();
    }
}
