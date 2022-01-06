using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CartItemListing : MonoBehaviour
{
    public TextMeshProUGUI productName;
    public TextMeshProUGUI shopName;
    public TextMeshProUGUI quantity;
    public TextMeshProUGUI price;
    [HideInInspector] public string pID;

    private Cart cart;

    public void FillDetails(string product, string shop, string quant, string cost, Cart cart, string PID)
    {
        productName.text = product;
        shopName.text = shop;
        quantity.text = quant;
        price.text = "INR " + cost + "/Unit";
        pID = PID;
        this.cart = cart;
    }

    public void RemoveItem()
    {
        cart.RemoveItem(this.gameObject);
    }
}
