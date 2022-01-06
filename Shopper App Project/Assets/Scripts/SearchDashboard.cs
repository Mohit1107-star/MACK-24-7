using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;
using TMPro;

public class SearchDashboard : MonoBehaviour
{
    public RectTransform content;
    public GameObject productDashboard;
    public GameObject productListing;
    public TMP_InputField searchInput;

    private Cart cart;
    private JsonData productsJSON;
    private string searchText;

    public void UpdateSearchText()
    {
        searchText = searchInput.text;
    }

    public void GetProducts(string userType)
    {
        StartCoroutine(Products(userType));
    }

    private IEnumerator Products(string userType)
    {
        UnityWebRequest unityWebRequest;

        string url = "https://oop-proj.herokuapp.com/products/" + userType;
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

        this.gameObject.SetActive(false);
        productDashboard.SetActive(true);

        for (int i = 0; i < count; i++)
        {
            if (productsJSON["products"][i]["name"].ToString().Contains(searchText))
            {
                GameObject g = Instantiate(productListing, content);
                g.GetComponent<ProductListing>().FillDetails(productsJSON["products"][i]["name"].ToString(),
                    productsJSON["products"][i]["userName"].ToString(),
                    productsJSON["products"][i]["_id"].ToString(),
                    productsJSON["products"][i]["quantity"].ToString(),
                    productsJSON["products"][i]["price"].ToString(),
                    cart);
            }
        }
    }
}
