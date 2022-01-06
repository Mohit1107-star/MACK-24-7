using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.Networking;

public class IncomingOrdersDashboard : MonoBehaviour
{
    public RectTransform content;
    public GameObject orderListing;

    private JsonData ordersJSON;

    public void GetOrders()
    {
        StartCoroutine(Orders());
    }

    private IEnumerator Orders()
    {
        UnityWebRequest unityWebRequest;

        string url = "https://oop-proj.herokuapp.com/order/soldBy/Retailer 2" /*+*/ /*PlayerPrefs.GetString("Mail ID")*/;
        using (unityWebRequest = UnityWebRequest.Get(url))
        {
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
            {
                Debug.LogError("ERROR: " + unityWebRequest.error);
            }
            else
            {
                ordersJSON = JsonMapper.ToObject(unityWebRequest.downloadHandler.text);
                PopulateScreen();
            }
        }
    }

    private void PopulateScreen()
    {
        int count = int.Parse(ordersJSON["count"].ToString());

        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < count; i++)
        {
            GameObject g = Instantiate(orderListing, content);
            g.GetComponent<IncomingOrderListing>().FillDetails(ordersJSON["order"][i]["product"].ToString(),
                ordersJSON["order"][i]["boughtBy"].ToString(),
                ordersJSON["order"][i]["productId"].ToString(),
                ordersJSON["order"][i]["deliveryPerson"].ToString(),
                ordersJSON["order"][i]["phone"].ToString(),
                ordersJSON["order"][i]["_id"].ToString());
        }
    }
}
