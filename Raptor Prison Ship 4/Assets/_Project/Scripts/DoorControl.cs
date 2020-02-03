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

	private AudioSource DoorAudio;
	private Vector3 ClosedPosition;
	[SerializeField]
	private AudioClip DoorSoundClip;

	private DoorState _doorState = DoorState.closed;

	void Start ()
	{
		ClosedPosition = this.gameObject.transform.position;
		DoorAudio = GetComponent<AudioSource> ();
	}

	public void PlayDoorSound ()
	{
		DoorAudio.PlayOneShot (DoorSoundClip);
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

			if (timer > 2.0f && linkedRaptor._rState == RaptorAI.RaptorState.Imprisoned) {
				linkedRaptor._rState = RaptorAI.RaptorState.HeadingToTarget;
			}
		}
    }
}
