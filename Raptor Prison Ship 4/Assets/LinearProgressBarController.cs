using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearProgressBarController : MonoBehaviour
{
    public ProgressBar progressBar;
    public ProgressBarCircle progressBarCircle;
    public float progress;

    // Update is called once per frame
    void Update()
    {
        if (progressBarCircle != null) { progressBarCircle.BarValue = progress; }
        if (progressBar != null) { progressBar.BarValue = progress; }
    }
}
