using System;
using UnityEngine;
using System.Runtime.InteropServices;
using TMPro;

public class TestMobile : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private void Start()
    {
        if(IsMobile())
            text.enabled = true;
        
    }

    [DllImport("__Internal")]
    private static extern int IsMobileBrowser();

    public bool IsMobile()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        return IsMobileBrowser() == 1;
#else
        return false;
#endif
    }
}
