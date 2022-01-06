using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;

public class ShopsDashboard : MonoBehaviour
{
    public RectTransform content;
    public GameObject shopListing;
    public RectTransform productListContent;
    public GameObject productDashboard;
    public Cart cart;

    private JsonData shopsJSON;

    public void GetShops(string userType)
    {
        StartCoroutine(Shops(userType));
    }

    private IEnumerator Shops(string userType)
    {
        UnityWebRequest unityWebRequest;

        string url = "https://oop-proj.herokuapp.com/user/" + userType;
        using (unityWebRequest = UnityWebRequest.Get(url))
        {
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
            {
                Debug.LogError("ERROR: " + unityWebRequest.error);
            }
            else
            {
                shopsJSON = JsonMapper.ToObject(unityWebRequest.downloadHandler.text);
                PopulateScreen();
            }
        }
    }

    private void PopulateScreen()
    { 
        int count = int.Parse(shopsJSON["count"].ToString());
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < count; i++)
        {
            GameObject g = Instantiate(shopListing, content);
            g.GetComponent<ShopListing>().FillDetails(shopsJSON["user"][i]["name"].ToString(), 
                shopsJSON["user"][i]["_id"].ToString(),
                productListContent, productDashboard, this.gameObject, cart);
        }
    }
}
