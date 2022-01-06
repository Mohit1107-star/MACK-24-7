using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.Networking;
using TMPro;

public class InventoryDashboard : MonoBehaviour
{
    public RectTransform content;
    public GameObject inventoryListing;

    [Header("Item Addition Menu")]
    public TMP_InputField productNameInput;
    public TMP_InputField quantityInput;
    public TMP_InputField priceInput;
    public TMP_InputField categoryInput;
    public TMP_InputField dateInput;
    public TMP_InputField shopNameInput;

    private string productName;
    private string quantity;
    private string price;
    private string category;
    private string date;
    private string shopName;

    private JsonData inventoryJSON;

    #region Inventory Display
    public void GetInventory(string shopName)
    {
        StartCoroutine(Inventory(shopName));
    }

    private IEnumerator Inventory(string shopName)
    {
        UnityWebRequest unityWebRequest;

        string url = "https://oop-proj.herokuapp.com/products/ofShop/" + shopName;
        using (unityWebRequest = UnityWebRequest.Get(url))
        {
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
            {
                Debug.LogError("ERROR: " + unityWebRequest.error);
            }
            else
            {
                inventoryJSON = JsonMapper.ToObject(unityWebRequest.downloadHandler.text);
                PopulateScreen();
            }
        }
    }

    private void PopulateScreen()
    {
        
        int count = int.Parse(inventoryJSON["count"].ToString());

        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
        
        for (int i = 0; i < count; i++)
        {
            GameObject o = Instantiate(inventoryListing, content) as GameObject;
            o.GetComponent<InventoryItemListing>().FillDetails(inventoryJSON["product"][i]["name"].ToString(),
                inventoryJSON["product"][i]["category"].ToString(),
                inventoryJSON["product"][i]["price"].ToString(),
                inventoryJSON["product"][i]["quantity"].ToString());
        }
    }
    #endregion

    #region Add Items
    public void ProductNameUpdate()
    {
        productName = productNameInput.text; 
    }

    public void QuantityUpdate()
    {
        quantity = quantityInput.text;
    }

    public void PriceUpdate()
    {
        price = priceInput.text;
    }

    public void CategoryUpdate()
    {
        category = categoryInput.text;
    }

    public void DateUpdate()
    {
        date = dateInput.text;
    }

    public void ShopNameUpdate()
    {
        shopName = shopNameInput.text;
    }

    public void Save()
    {
        StartCoroutine(Upload());
    }

    private IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", productName);
        form.AddField("price", price);
        form.AddField("userName", shopName);
        form.AddField("userType", "retailer");
        form.AddField("category", category);
        form.AddField("quantity", quantity);
        //form.AddField("date", date);

        using (UnityWebRequest www = UnityWebRequest.Post("https://oop-proj.herokuapp.com/products", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }
    #endregion
}
