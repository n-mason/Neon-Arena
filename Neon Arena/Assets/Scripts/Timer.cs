using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerTextValue;
    private float seconds;

    void Start()
    {
        seconds = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        seconds += Time.deltaTime;
        timerTextValue.text = "" + (seconds);
    }
}
