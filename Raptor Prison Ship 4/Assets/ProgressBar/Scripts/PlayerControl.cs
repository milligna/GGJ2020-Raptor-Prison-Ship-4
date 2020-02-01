using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
	public enum playerState {
		Moving,
		TrainingRaptor,
		RebootingComputer
	}

	public playerState _pState;

	public NavMeshAgent player;

	public float PlayerComputerInteraction = 2.0f;	// Should be about the same as the computer's trigger colliders diameter/radius??

	public int targettedComputer = 0;

	private int ClickLayerMask;

    // Start is called before the first frame update
    void Start()
    {
		_pState = playerState.Moving;
		targettedComputer = 0;  // Not aiming at a computer
		ClickLayerMask = LayerMask.GetMask ("Floor", "Computer");
    }

	// Update is called once per frame
	void Update()
    {
		if (Input.GetMouseButtonDown (0)) {
			Ray raything = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit rayResult;

			if (_pState == playerState.Moving) {

				if (Physics.Raycast (raything, out rayResult, 200f, ClickLayerMask)) {
					player.SetDestination (rayResult.point);

					if (rayResult.collider.gameObject.layer == LayerMask.NameToLayer ("Computer")) {
						Computer touchedComputer = rayResult.collider.gameObject.GetComponent<Computer> ();
						targettedComputer = touchedComputer.computerID;
						if ((player.destination - player.gameObject.transform.position).magnitude < PlayerComputerInteraction) {
							if (touchedComputer._state == Computer.ComputerState.Crashed) {
								_pState = playerState.RebootingComputer;
								touchedComputer.rebootComputer ();
							}
						}
					} else {
						targettedComputer = 0;
					}


				}
			}
		}
    }
}
