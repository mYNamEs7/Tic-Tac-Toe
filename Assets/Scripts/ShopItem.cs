using System;

[Serializable]
public class ShopItem
{
    public string key;
    public int amount;
    public string price;
    public string currency;
    public ItemDetails[] items; // This array can contain multiple items
}

[Serializable]
public class ItemDetails
{
    public string key;
    public int damage;
    public int amount;
}

[Serializable]
public class PurchasesData
{
    public ShopItem[] shopItems;
}
