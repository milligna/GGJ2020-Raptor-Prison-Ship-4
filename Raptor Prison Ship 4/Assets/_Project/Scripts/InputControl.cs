using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InputControl : MonoBehaviour
{
	public NavMeshAgent player;

	public float PlayerComputerInteraction = 2.0f;	// Should be about the same as the computer's trigger colliders diameter/radius??

	public int targettedComputer = 0;

    // Start is called before the first frame update
    void Start()
    {
		targettedComputer = 0;	// Not aiming at a computer
    }

	// Update is called once per frame
	void Update()
    {
		if (Input.GetMouseButtonDown (0)) {
			Ray raything = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit rayResult;

			if (Physics.Raycast (raything, out rayResult)) {
				player.SetDestination (rayResult.point);

				if (rayResult.collider.gameObject.layer == LayerMask.NameToLayer ("Computer")) {
					Computer touchedComputer = rayResult.collider.gameObject.GetComponent<Computer> ();
					targettedComputer = touchedComputer.computerID;
					if ((player.destination - player.gameObject.transform.position).magnitude < PlayerComputerInteraction ) {
						touchedComputer.rebootComputer ();
					}
				} else {
					targettedComputer = 0;
				}


			}

		}
    }
}
