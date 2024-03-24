using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fullscreen : MonoBehaviour
{
    public void ShowFullScreenAd()
    {
        Application.ExternalCall("ShowAd");
    }
}