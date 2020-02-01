using System.Collections;
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
			other.gameObject.GetComponent<RaptorAI> ().FindNewTarget ();

		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
