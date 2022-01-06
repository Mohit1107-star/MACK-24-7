using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Networking;
using System.Text;

public class UserRegistration : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TMP_InputField emailInput;
    public TMP_InputField numberInput;
    public TMP_InputField alternateNumberInput;
    public TMP_InputField addressInput;
    public GameObject dashboard;

    private string fullName;
    private string mailID;
    private string number;
    private string alternateNumber;
    private string address;
    private string authUID;

    public void SetDetails(string name, string mail, string uid)
    {
        fullName = name;
        nameInput.text = fullName;

        mailID = mail;
        emailInput.text = mailID;

        authUID = uid;
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
        PlayerPrefs.SetString("Auth UID", authUID);

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

                dashboard.SetActive(true);
                this.gameObject.SetActive(false);
            }
        }
    }
}
