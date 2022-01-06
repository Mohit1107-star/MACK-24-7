using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Networking;
using System.Text;

public class IncomingOrderListing : MonoBehaviour
{
    public TextMeshProUGUI productName;
    public TextMeshProUGUI customerName;
    public TextMeshProUGUI deliveryGuy;
    public TextMeshProUGUI deliveryContact;
    public TMP_Dropdown status;

    private string productID;
    private string orderID;

    public void FillDetails(string product, string customer, string pID, string dGuy, string dContact, string oID)
    {
        productName.text = product;
        customerName.text = customer;
        deliveryGuy.text = dGuy;
        deliveryContact.text = dContact;
        productID = pID;
        orderID = oID;
    }

    public void UpdateStatus()
    {
        Debug.Log(status.options[status.value].text);
        StartCoroutine(Status(status.options[status.value].text));
    }

    private IEnumerator Status(string status)
    {
        WWWForm form = new WWWForm();
        form.AddField("status", status);
        form.AddField("deliveryPerson", deliveryGuy.text);
        form.AddField("phone", deliveryContact.text);

        var data = Encoding.UTF8.GetBytes($"{{\"status\":\"{status}\",\"deliveryPerson\":\"{deliveryGuy.text}\",\"phone\":\"{deliveryContact.text}\"}}");
        using (UnityWebRequest www = UnityWebRequest.Put("http://oop-proj.herokuapp.com/order/update/" + orderID, data))
        {
            //www.method = "PATCH";
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
}
