using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cart : MonoBehaviour
{
    public RectTransform content;
    public GameObject cartItemListing;
    public TextMeshProUGUI placeOrderText;

    private int total = 0;

    private void Start()
    {
        total = 0;
        UpdateTotal();
    }

    public void AddToCart(string productName, string shopName, string quantity, string price, string productID)
    {
        GameObject g = Instantiate(cartItemListing, content);
        g.GetComponent<CartItemListing>().FillDetails(productName, shopName, quantity, price, this, productID);
    }

    public void UpdateTotal()
    {
        foreach(RectTransform g in content.transform)
        {
            total += int.Parse(g.GetComponent<CartItemListing>().price.text.Substring(4, 2));
        }
        placeOrderText.text = "Place Order! - COD - INR " + total;
    }

    public void RemoveItem(GameObject g)
    {
        total -= int.Parse(g.GetComponent<CartItemListing>().price.text.Substring(4, 2));
        placeOrderText.text = "Place Order! - COD - INR " + total;
        Destroy(g);
    }
}
