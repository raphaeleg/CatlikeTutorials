using System;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] Transform hoursPivot;
    [SerializeField] Transform minutesPivot;
    [SerializeField] Transform secondsPivot;
    const float hoursToDeg = -30f;
    const float minToDeg = -6f;
    const float secToDeg = -6f;

    private void Update()
    {
        TimeSpan time = DateTime.Now.TimeOfDay;
        hoursPivot.localRotation = Quaternion.Euler(0f, 0f, hoursToDeg * (float)time.TotalHours);
        minutesPivot.localRotation = Quaternion.Euler(0f, 0f, minToDeg * (float)time.TotalMinutes);
        secondsPivot.localRotation = Quaternion.Euler(0f, 0f, secToDeg * (float)time.TotalSeconds);
    }
}
