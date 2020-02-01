using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetProgressBar : MonoBehaviour
{

    public ProgressBar progressBar;
    public ProgressBarCircle progressBarCircle;

    [SerializeField]
    private float progress;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        progressBar.BarValue = progress;
        progressBarCircle.BarValue = progress;
    }
}
