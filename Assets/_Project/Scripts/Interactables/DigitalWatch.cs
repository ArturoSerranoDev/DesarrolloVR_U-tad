using System;
using TMPro;
using UnityEngine;

public class DigitalWatch : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI watchTimerText;

    // Update is called once per frame
    void Update()
    {
        watchTimerText.text = DateTime.Now.ToString(format: "hh:mm:ss");
    }
}
