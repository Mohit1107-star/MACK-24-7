using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;

public class CategoriesDashboard : MonoBehaviour
{
    public RectTransform content;
    public GameObject productDashboard;
    public GameObject productListing;
    public Cart cart;
    public string userType;

    private JsonData productsJSON;

    public void GetProducts(string category)
    {
        StartCoroutine(Products(category));
    }

    private IEnumerator Products(string category)
    {
        UnityWebRequest unityWebRequest;

        string url = "https://oop-proj.herokuapp.com/products/" + userType + "/category/" + category;
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
            GameObject g = Instantiate(productListing, content);
            g.GetComponent<ProductListing>().FillDetails(productsJSON["product"][i]["name"].ToString(), 
                productsJSON["product"][i]["userName"].ToString(),
                productsJSON["product"][i]["_id"].ToString(),
                productsJSON["product"][i]["quantity"].ToString(),
                productsJSON["product"][i]["price"].ToString(),
                cart);
        }
    }
}
