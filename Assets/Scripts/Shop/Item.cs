using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [Header("Paramenters")]
    [SerializeField] private string confirmationText = "Вы уверены что хотите совершить покупку?";
    [SerializeField] private string purchaseCompletedText = "Уже куплено";

    [Header("UI")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Image image;
    [SerializeField] private Image iconImage;
    private ItemImageSO[] itemImages;
    private Button buyButton;
    private ComboPanel comboPanel;
    private string price;

    void Awake()
    {
        itemImages = Resources.LoadAll<ItemImageSO>(@"ScriptableObjects\ItemImages");
        buyButton = GetComponent<Button>();
    }

    public void Init(ShopItem shopItem, ComboPanel comboPanel)
    {
        this.comboPanel = comboPanel;

        nameText.text = shopItem.key;
        if (shopItem.amount != 0)
            amountText.text = shopItem.amount.ToString();
        else
        {
            Destroy(amountText.gameObject);
            Destroy(iconImage.gameObject);
            nameText.alignment = TextAlignmentOptions.Midline;
        }

        InitBuyButton();

        price = shopItem.price + " " + shopItem.currency;
        if (!GameData.Instance.boughtItems.Contains(nameText.text + " " + price))
            priceText.text = price;
        else
            PurchaseCompleted();

        SetImages(shopItem.key);
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

    private void InitBuyButton()
    {
        if (comboPanel != null)
            buyButton.onClick.AddListener(() => comboPanel.Show());
        else
            buyButton.onClick.AddListener(() => DialogWindow.Instance.ShowBuyWindow(confirmationText, comboPanel, this));
    }

    public void PurchaseCompleted()
    {
        priceText.text = purchaseCompletedText;
        if (comboPanel == null)
            buyButton.onClick.RemoveAllListeners();
        else
            comboPanel.PurchaseCompleted();

        if (!GameData.Instance.boughtItems.Contains(nameText.text + " " + price))
        {
            GameData.Instance.boughtItems = GameData.Instance.boughtItems.Append(nameText.text + " " + price).ToArray();
            GameData.Instance.SaveData();
        }
    }
}
