using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour {

    public ProgressBar Pb;
    public ProgressBarCircle PbC;

    private void Start()
    {
        Pb.BarValue = 50;
        PbC.BarValue = 50;
    }

    void FixedUpdate () {
		
		if(Input.GetKey(KeyCode.KeypadPlus) || Input.GetAxis("Vertical") > 0 )
        {
            Pb.BarValue += 1;
            PbC.BarValue += 1;
        }

		if (Input.GetKey(KeyCode.KeypadMinus)|| Input.GetAxis ("Vertical") < 0)
        {
            Pb.BarValue -= 1;
            PbC.BarValue -= 1;
        }
    }
}
