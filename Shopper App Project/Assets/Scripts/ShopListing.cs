using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using LitJson;

public class ShopListing : MonoBehaviour
{
    public TextMeshProUGUI shopName;
    public RectTransform content;
    public GameObject productDashboard;
    public GameObject shopsDashboard;
    public GameObject productListing;
    
    private Cart cart;
    private JsonData productsJSON;
    private string userID;

    public void FillDetails(string shop, string ID, RectTransform cont, GameObject prodD, GameObject shopsD, Cart cart)
    {
        shopName.text = shop;
        userID = ID;
        content = cont;
        productDashboard = prodD;
        shopsDashboard = shopsD;
        this.cart = cart;
    }

    public void GetProducts()
    {
        StartCoroutine(Products());
    }

    private IEnumerator Products()
    {
        UnityWebRequest unityWebRequest;

        string url = "https://oop-proj.herokuapp.com/user/" + userID;
        using (unityWebRequest = UnityWebRequest.Get(url))
        {
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
            {
                Debug.LogError("ERROR: " + unityWebRequest.error);
            }
            else
            {
                productsJSON = JsonMapper.ToObject(unityWebRequest.downloadHandler.text);
                PopulateScreen();
            }
        }
    }

    private void PopulateScreen()
    {
        int count = int.Parse(productsJSON["count"].ToString());

        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        shopsDashboard.SetActive(false);
        productDashboard.SetActive(true);
        for (int i = 0; i < count; i++)
        {
            GameObject g = Instantiate(productListing, content);
            g.GetComponent<ProductListing>().FillDetails(productsJSON["products"][i]["name"].ToString(), 
                shopName.text,
                productsJSON["products"][i]["_id"].ToString(),
                productsJSON["products"][i]["quantity"].ToString(),
                productsJSON["products"][i]["price"].ToString(),
                cart);
        }
    }
}
