using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryItemListing : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI categoryText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI quantityText;

    public void FillDetails(string name, string price, string category, string quantity)
    {
        itemNameText.text = name;
        categoryText.text = category;
        priceText.text = "INR " + price + "/Unit";
        if (int.Parse(quantity) <= 0)
        {
            quantityText.text = System.DateTime.Now.ToString("MM/dd");
        }
        else
        {
            quantityText.text = quantity;
        }
    }
}
