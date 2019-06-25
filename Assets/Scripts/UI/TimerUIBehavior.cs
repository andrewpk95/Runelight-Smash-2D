using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUIBehavior : MonoBehaviour
{
    Text timerUIText;

    public Timer timer;

    // Start is called before the first frame update
    void Start()
    {
        timerUIText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer != null) {
            int frame = timer.durationLeft;
            int minutes = frame / 3600;
            float seconds = (frame % 3600) / 60.0f;
            timerUIText.text = string.Format("{0}:{1:00.00}", minutes, seconds);
        }
    }
}
