﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonComputer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

	private void OnTriggerStay (Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer ("Raptor")) {			
			if (other.gameObject.GetComponent<RaptorAI> ().targettedLocation == this.gameObject) {
				if (other.gameObject.GetComponent<RaptorAI> ()._rState != RaptorAI.RaptorState.WastingTime) {
					other.gameObject.GetComponent<RaptorAI> ().ArrivedAtLocation ();
				}
			}
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
