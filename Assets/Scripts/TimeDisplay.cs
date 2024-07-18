using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeDisplay : MonoBehaviour
{
    public Text dayText;
    public Text timeText;
    public Text ampmText;

    void Start()
    {
        if (dayText == null || timeText == null || ampmText == null)
        {
            Debug.LogError("One or more Text components are not assigned.");
            return;
        }

        InvokeRepeating("UpdateTime", 0f, 60f); // Update the time every minute
        UpdateTime();
    }

    void UpdateTime()
    {
        DateTime now = DateTime.Now;
        string day = now.ToString("dddd, MMMM dd", System.Globalization.CultureInfo.InvariantCulture);
        string time = now.ToString("hh : mm", System.Globalization.CultureInfo.InvariantCulture);
        string ampm = now.ToString("tt", System.Globalization.CultureInfo.InvariantCulture);

        dayText.text = day;
        timeText.text = time;
        ampmText.text = ampm;
    }
}
