using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProductListing : MonoBehaviour
{
    public TextMeshProUGUI productName;
    public TextMeshProUGUI shopName;
    public TextMeshProUGUI quantity;
    public TextMeshProUGUI price;

    private Cart cart;
    private string productID;

    public void FillDetails(string product, string shop, string pID, string quant, string cost, Cart cart)
    {
        productName.text = product;
        shopName.text = shop;
        quantity.text = quant;
        price.text = "INR " + cost + "/Unit";
        productID = pID;
        this.cart = cart;
    }

    public void Add()
    {
        /*string dateTimeString = "2020-05-05";
        System.DateTime dateTime = System.DateTime.Parse(dateTimeString);
        print(dateTime.ToString("yyyy-MM-dd"));
        CalendarEvent.AddEvent("Reminder about " + productName.text, System.DateTime.Now, System.DateTime.Now, true, Success);*/
        cart.AddToCart(productName.text, shopName.text, quantity.text, price.text.Substring(4, 2), productID);
    }

    private void Success(int eventID)
    {
        Debug.Log("Success");
    }
}
