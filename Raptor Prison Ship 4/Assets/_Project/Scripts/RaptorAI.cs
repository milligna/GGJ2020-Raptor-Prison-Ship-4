using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RaptorAI : MonoBehaviour
{
	public enum RaptorState {
		Imprisoned,
		HeadingToTarget,
		FiddlingWithComputer,
		WastingTime,
		Learning,
		Content
	}

	public float _RaptorTimer;
	private AudioSource AS;

	public float timeToEducate = 5f;

	public Computer RaptorHappyPlace;

	public RaptorState _rState;

	public PlayerControl playerInteracting;

	[SerializeField]
	private LinearProgressBarController pBar;

	[SerializeField]
	private AudioClip [] RaptorSounds;

	public float laziness = 7.5f;

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
		AS = GetComponent<AudioSource> ();
		nonComputerLocations = FindObjectsOfType<NonComputer> ();
		_RaptorTimer = 0f;
    }

	public void ArrivedAtLocation ()
	{
		_rState = RaptorState.WastingTime;
		_RaptorTimer = laziness;
		targettedLocation = null;
		targettedComputer = null;
	}

	public int Tooled (int toolID)
	{
		if (targettedComputer != null) {
			if (toolID == 0) {
				_rState = RaptorState.HeadingToTarget;
				targettedComputer.RaptorInterferenceInterferedWith ();
				playRaptorSound ();
				targettedComputer = null;
				targettedLocation = null;
				return 0;

			} else if (toolID == 1) {
				_rState = RaptorState.Learning;
				pBar.gameObject.SetActive (true);
				_RaptorTimer = timeToEducate;
				targettedComputer.RaptorInterferenceInterferedWith ();
				targettedComputer = null;
				targettedLocation = null;
				return 1;
			}
			return -1;
		} else
			return -1;
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

		if (playerInteracting != null) {
			playerInteracting.TooSlowRaptorMovedOn ();
			playerInteracting = null;
		}

	}

	public void playRaptorSound ()
	{
		if (AS != null && RaptorSounds.Length > 0) {
			AS.PlayOneShot (RaptorSounds [Random.Range (0, RaptorSounds.Length)]);
		}
	}

    // Update is called once per frame
    void Update()
    {
		_RaptorTimer -= Time.deltaTime;

		if (_RaptorTimer < 0 &&  _rState == RaptorState.WastingTime) {
			playRaptorSound ();
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
				playRaptorSound ();
				_rState = RaptorState.Content;
				pBar.gameObject.SetActive (false);
				raptorAgent.SetDestination (RaptorHappyPlace.gameObject.transform.position);
				targettedComputer = RaptorHappyPlace;
				targettedLocation = RaptorHappyPlace.gameObject;
				// Need to tell the player that the lesson is over.
				FindObjectOfType<PlayerControl> ().LessonOver ();
			}
		}
    }
}
