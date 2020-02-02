using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{

    public Animator animator;

	public enum DoorState {
		open,
		closed
	}


	DoorState DoorPosition {
		get {
			return _doorState;
		}
	}

	[SerializeField]
	private RaptorAI linkedRaptor;

	private float timer = 0;
	private Vector3 ClosedPosition;

	private DoorState _doorState = DoorState.closed;

	void Start ()
	{
		ClosedPosition = this.gameObject.transform.position;
	}

	public void ReleaseTheRaptor ()
	{
		_doorState = DoorState.open;
        animator.SetInteger("OpenDoor", 1);
	}

    // Update is called once per frame
    void Update()
    {
		if (DoorPosition == DoorState.open) {
			timer += Time.deltaTime;

			//this.gameObject.transform.position = Vector3.Slerp (ClosedPosition, ClosedPosition - Vector3.up * 4, (timer / 3.0f));

			if (timer > 3.0f) {
				linkedRaptor._rState = RaptorAI.RaptorState.HeadingToTarget;
			}
		}
    }
}
