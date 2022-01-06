using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlacedOrderListing : MonoBehaviour
{
    public TextMeshProUGUI productName;
    public TextMeshProUGUI shopName;
    public TextMeshProUGUI status;

    public void FillDetails(string p, string sh, string s)
    {
        productName.text = p;
        shopName.text = sh;
        status.text = s;
    }
}
