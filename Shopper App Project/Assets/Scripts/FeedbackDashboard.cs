using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class FeedbackDashboard : MonoBehaviour
{
    public TMP_InputField feedback;
    public TMP_InputField shop;

    private string f;
    private string s;

    public void F()
    {
        f = feedback.text;
    }

    public void S()
    {
        s = shop.text;
    }

    public void AddFeedback()
    {
        StartCoroutine(Feedback());
    }

    private IEnumerator Feedback()
    {
        WWWForm form = new WWWForm();
        form.AddField("text", f);
        form.AddField("customerName", PlayerPrefs.GetString("Full Name"));
        form.AddField("shopName", s);

        using (UnityWebRequest www = UnityWebRequest.Post("https://oop-proj.herokuapp.com/feedback", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                feedback.text = null;
                shop.text = null;
            }
        }
    }
}
