using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.Events;
using TMPro;
using System.Linq;

public class ShopManager : SingletonWindow<ShopManager>
{
    [SerializeField] private Transform shopPanel;
    [SerializeField] private ComboPanel comboPanelPrefab;
    [SerializeField] private Item simpleItem;
    [SerializeField] private Item itemInCombo;
    [SerializeField] private Transform background;
    [SerializeField] private Button backButton;

    private PurchasesData purchasesData;
    private GameObject[] spawnedObjects = new GameObject[] { };

    protected override void Awake()
    {
        base.Awake();

        backButton.onClick.AddListener(Hide);

        Hide();
    }

    public override void Show()
    {
        base.Show();

        LoadJsonData();
    }

    public override void Hide()
    {
        base.Hide();

        if (spawnedObjects != null)
        {
            foreach (var item in spawnedObjects)
            {
                Destroy(item);
            }
        }
    }

    void LoadJsonData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath + "/", "shopData.json");
        StartCoroutine(LoadJsonData(filePath));
    }

    IEnumerator LoadJsonData(string filePath)
    {
        string dataAsJson;
        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(filePath);
            var requestOperation = www.SendWebRequest();
            while (!requestOperation.isDone)
            {
                yield return null;
            }

            dataAsJson = www.downloadHandler.text;

            www.Dispose();
        }
        else
        {
            dataAsJson = File.ReadAllText(filePath);
        }

        purchasesData = JsonUtility.FromJson<PurchasesData>(dataAsJson);
        DisplayItems();
    }

    void DisplayItems()
    {
        if (purchasesData != null)
        {
            foreach (ShopItem item in purchasesData.shopItems)
            {
                // спавним предмет в магазине
                var simpleItemTransform = Instantiate(simpleItem, shopPanel);
                spawnedObjects = spawnedObjects.Append(simpleItemTransform.gameObject).ToArray();

                ComboPanel comboPanel = null;
                if (item.items != null)
                {
                    // спавним комбо панель
                    comboPanel = Instantiate(comboPanelPrefab.transform, background).GetComponent<ComboPanel>();
                    spawnedObjects = spawnedObjects.Append(comboPanel.gameObject).ToArray();

                    // инициализируем комбо панель
                    comboPanel.Init(item.price + " " + item.currency, simpleItemTransform);

                    // спавн элементов в комбо панели
                    foreach (ItemDetails details in item.items)
                    {
                        var itemInComboTransform = Instantiate(itemInCombo, comboPanel.Container.transform);

                        itemInComboTransform.Init(details);
                    }

                    Canvas.ForceUpdateCanvases();
                    comboPanel.Hide();
                }

                // инициализируем предмет магазина
                simpleItemTransform.Init(item, comboPanel);
            }

            UpdateCanvas();
        }
        else
        {
            Debug.LogError("No purchases data loaded!");
        }
    }

    private void UpdateCanvas()
    {
        Canvas.ForceUpdateCanvases();
        shopPanel.gameObject.SetActive(false);
        shopPanel.gameObject.SetActive(true);
    }
}
