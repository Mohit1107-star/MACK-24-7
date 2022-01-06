using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppLaunch : MonoBehaviour
{
    public GameObject dashboard;
    public GameObject titleScreen;

    private void Awake()
    {
        if(PlayerPrefs.GetString("Full Name") != "")
        {
            dashboard.SetActive(true);
        }
        else
        {
            titleScreen.SetActive(true);
        }
    }
}
