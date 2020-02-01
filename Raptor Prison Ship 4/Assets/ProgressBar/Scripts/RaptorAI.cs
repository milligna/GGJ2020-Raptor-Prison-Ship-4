using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RaptorAI : MonoBehaviour
{
	public enum RaptorState {
		Imprisoned,
		HeadingToTarget,
		FiddlingWithTarget,
		Learning,
		Content
	}

	private float _RaptorTimer;

	public float timeToEducate = 5f;

	public GameObject RaptorHappyPlace;

	public RaptorState _rState;

	[SerializeField]
	private LinearProgressBarController pBar;

	public Computer targettedComputer;
	public GameObject targettedLocation;
	[SerializeField]
	private NavMeshAgent raptorAgent;

	private ComputerManager CM;

	[SerializeField]
	private NonComputer [] nonComputerLocations;


    // Start is called before the first frame update
    void Start()
    {
		_rState = RaptorState.Imprisoned;
		CM = FindObjectOfType<ComputerManager> ();
		nonComputerLocations = FindObjectsOfType<NonComputer> ();
		_RaptorTimer = 5.0f;
    }

	public void ArrivedAtLocation ()
	{
		_rState = RaptorState.FiddlingWithTarget;
	}

	public void Tooled (int toolID)
	{
		if (targettedComputer != null) {
			if (toolID == 0) {
				_rState = RaptorState.HeadingToTarget;
				targettedComputer.RaptorInterferenceInterferedWith ();

				targettedComputer = null;
				targettedLocation = null;

			} else if (toolID == 1) {
				_rState = RaptorState.Learning;
				pBar.gameObject.SetActive (true);
				_RaptorTimer = timeToEducate;
				targettedComputer.RaptorInterferenceInterferedWith ();
				targettedComputer = null;
				targettedLocation = null;
			}
		}
	}

	public void FindNewTarget ()
	{
		targettedComputer = CM.GetRaptorTarget ();

		if (targettedComputer == null) {
			targettedLocation = nonComputerLocations [Random.Range (0, nonComputerLocations.Length)].gameObject;
		} else {
			targettedLocation = targettedComputer.gameObject;
		}

		raptorAgent.SetDestination (targettedLocation.transform.position);
	}

    // Update is called once per frame
    void Update()
    {
		_RaptorTimer -= Time.deltaTime;

		if (_RaptorTimer < 0 && _rState == RaptorState.Imprisoned) {
			_rState = RaptorState.HeadingToTarget;
		}

		if (_rState == RaptorState.HeadingToTarget) {
			if (targettedLocation == null) {
				FindNewTarget ();
			} else {
				if (targettedComputer != null) {
					if (targettedComputer._state != Computer.ComputerState.WaitingToCrash) {
						FindNewTarget ();
					}
				}
			}
		} else if (_rState == RaptorState.Learning) {
			pBar.progress = ((timeToEducate - _RaptorTimer) / timeToEducate) * 100;
			if (_RaptorTimer < 0) {
				_rState = RaptorState.Content;
				pBar.gameObject.SetActive (false);
				raptorAgent.SetDestination (RaptorHappyPlace.transform.position);
				// Need to tell the player that the lesson is over.
				FindObjectOfType<PlayerControl> ().LessonOver ();
				Debug.Log ("here");
			}
		}
    }
}
