using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.Events;

public class ShopManager : Window
{
    public static ShopManager Instance { get; private set; }

    [SerializeField] private Transform shopPanel;
    [SerializeField] private ComboPanel comboPanelPrefab;
    [SerializeField] private Item simpleItem;
    [SerializeField] private Item itemInCombo;
    [SerializeField] private Transform background;
    [SerializeField] private Button backButton;

    private PurchasesData purchasesData;

    void Awake()
    {
        Instance = this;

        backButton.onClick.AddListener(() => Hide());
    }

    void Start()
    {
        LoadJsonData();
        DisplayItems();

        Hide();
    }

    void LoadJsonData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "shopData.json");

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            purchasesData = JsonConvert.DeserializeObject<PurchasesData>(dataAsJson);
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

    void DisplayItems()
    {
        if (purchasesData != null)
        {
            foreach (ShopItem item in purchasesData.shopItems)
            {
                // спавним предмет в магазине
                var simpleItemTransform = Instantiate(simpleItem, shopPanel);

                ComboPanel comboPanel = null;
                if (item.items != null)
                {
                    // спавним комбо панель
                    comboPanel = Instantiate(comboPanelPrefab.transform, background).GetComponent<ComboPanel>();

                    // инициализируем комбо панель
                    comboPanel.Init(item.price + " " + item.currency, simpleItemTransform);

                    // спавн элементов в комбо панели
                    foreach (ItemDetails details in item.items)
                    {
                        var itemInComboTransform = Instantiate(itemInCombo, comboPanel.transform);

                        itemInComboTransform.Init(details);
                    }

                    Canvas.ForceUpdateCanvases();
                    comboPanel.Hide();
                }

                // инициализируем предмет магазина
                simpleItemTransform.Init(item, comboPanel);
            }

            Canvas.ForceUpdateCanvases();
        }
        else
        {
            Debug.LogError("No purchases data loaded!");
        }
    }
}
