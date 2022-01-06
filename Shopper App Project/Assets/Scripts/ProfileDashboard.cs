using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using LitJson;
using System;

public class ProfileDashboard : MonoBehaviour
{
    [Header("Personal Details")]
    public TMP_InputField nameInput;
    public TMP_InputField emailInput;
    public TMP_InputField numberInput;
    public TMP_InputField alternateNumberInput;
    public TMP_InputField addressInput;

    [Header("Cart")]
    public RectTransform content;

    [Header("Order Status")]
    public RectTransform orderStatusContent;
    public GameObject placeOrderListing;

    private string fullName;
    private string mailID;
    private string number;
    private string alternateNumber;
    private string address;
    private string orderID;

    #region Personal Details
    public void DiplayPersonalDetails()
    {
        fullName = PlayerPrefs.GetString("Full Name");
        mailID = PlayerPrefs.GetString("Mail ID");
        number = PlayerPrefs.GetString("Number");
        alternateNumber = PlayerPrefs.GetString("Alternate Number");
        address = PlayerPrefs.GetString("Address");

        nameInput.text = fullName;
        emailInput.text = mailID;
        numberInput.text = number;
        alternateNumberInput.text = alternateNumber;
        addressInput.text = address;
    }

    public void NameUpdate()
    {
        fullName = nameInput.text;
    }

    public void EmailUpdate()
    {
        mailID = emailInput.text;
    }

    public void NumberUpdate()
    {
        number = numberInput.text;
    }

    public void AlternateNumberUpdate()
    {
        alternateNumber = alternateNumberInput.text;
    }

    public void AddressUpdate()
    {
        address = addressInput.text;
    }

    public void SaveDetails()
    {
        StartCoroutine(UpdateDetails());
    }

    private IEnumerator UpdateDetails()
    {
        PlayerPrefs.SetString("Full Name", fullName);
        PlayerPrefs.SetString("Mail ID", mailID);
        PlayerPrefs.SetString("Number", number);
        PlayerPrefs.SetString("Alternate Number", alternateNumber);
        PlayerPrefs.SetString("Address", address);

        WWWForm form = new WWWForm();
        form.AddField("name", fullName);
        form.AddField("phone", number);
        form.AddField("userType", "wholesaler");
        //form.AddField("ID", authUID);

        //var data = Encoding.UTF8.GetBytes($"{{\"name\":\"{fullName}\",\"phone\":\"{number}\",\"userType\":\"wholesaler\"}}");
        using (UnityWebRequest www = UnityWebRequest.Post("https://oop-proj.herokuapp.com/user", form))
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

    #region Cart
    public void PlaceOrder()
    {
        foreach (CartItemListing cartItemListing in content.GetComponentsInChildren<CartItemListing>())
        {
            StartCoroutine(UpdateOrder(cartItemListing));
        }

        UpdateScreen();
    }

    private IEnumerator UpdateOrder(CartItemListing c)
    {
        WWWForm orderForm = new WWWForm();
        orderForm.AddField("product", c.productName.text);
        orderForm.AddField("soldBy", c.shopName.text);
        orderForm.AddField("boughtBy", PlayerPrefs.GetString("Full Name"));
        orderForm.AddField("productId", c.pID);
        orderForm.AddField("quantity", 1.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post("https://oop-proj.herokuapp.com/order", orderForm))
        {

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                JsonData j = JsonMapper.ToObject(www.downloadHandler.text);
                orderID = j["order"]["_id"].ToString();
            }
        }
    }

    private void UpdateScreen()
    {
        foreach (CartItemListing cartItemListing in content.GetComponentsInChildren<CartItemListing>())
        {
            GameObject g = Instantiate(placeOrderListing, orderStatusContent);
            g.GetComponent<PlacedOrderListing>().FillDetails(cartItemListing.productName.text,
                cartItemListing.shopName.text,
                "Order Placed");

            Destroy(cartItemListing.gameObject);
        }
    }
    #endregion
}
