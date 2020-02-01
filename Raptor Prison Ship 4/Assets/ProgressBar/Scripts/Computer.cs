using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ComputerType {
	Mac,
	Windows,
	AppleIIe,
	Commodore64
};

public class Computer : MonoBehaviour
{
	public enum ComputerState {
		WaitingToCrash,
		RaptorCrashingComputer,
		TrainingRaptor,
		RaptorSafelyUsingComputer,
		Rebooting,
		Crashed,
		Exploding
	}

	[SerializeField]
	private CrashController CC;
	[SerializeField]
	private LinearProgressBarController pBar;

	public int computerID;
	public ComputerType computerType;
	public float rebootTime = 5f;
	public ComputerState _state;
	[SerializeField]
	private float _crashTimer;
	private float _timerLength;
	private RaptorAI currentRaptorUser;

	private ComputerManager CM;

    // Start is called before the first frame update
    void Start()
    {
		if(pBar == null)
			pBar = this.gameObject.GetComponentInChildren<LinearProgressBarController> ();


		CC = GetComponent<CrashController> ();
		CM = FindObjectOfType<ComputerManager> ();

		if (computerID == 0) {
			computerID = Random.Range (0, 100);
		}

		// Pick a random computer type 
		computerType =  (ComputerType)Random.Range (0, System.Enum.GetValues(typeof(ComputerType)).Length);
		_state = ComputerState.WaitingToCrash;
		_crashTimer = Random.Range (CM.MinDefaultComputerCrashTime, CM.MaxDefaultComputerCrashTime);
    }

	private void OnTriggerEnter (Collider other)
	{
		// If we've already clicked on the computer before being in range
		if (other.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			if (other.gameObject.GetComponent<PlayerControl> ().targettedComputer == computerID) {
				rebootComputer ();
			}
		} else if(other.gameObject.layer == LayerMask.NameToLayer("Raptor")) {
			if ( other.gameObject.GetComponent<RaptorAI> ().targettedComputer == this ) {
				if (_state == ComputerState.WaitingToCrash) {
					RaptorVisitsComputer (other.gameObject.GetComponent<RaptorAI> ());
				} else {
					// Computer crashed before the raptor could crash it, but after it targetted it
					other.gameObject.GetComponent<RaptorAI> ().FindNewTarget ();
				}
			}
		}
	}

	private void OnTriggerStay (Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer ("Raptor")) {
			if (other.gameObject.GetComponent<RaptorAI> ().targettedComputer == this)
			{
				if (_state == ComputerState.WaitingToCrash) {
					RaptorVisitsComputer (other.gameObject.GetComponent<RaptorAI> ());
				} else if (_state == ComputerState.Crashed) {
					other.gameObject.GetComponent<RaptorAI> ().FindNewTarget ();
				}
			}
		}
	}

	/// <summary>
	/// Called the moment a Raptor starts to interact with a computer and puts it on the shortcut to crashing
	/// </summary>
	private void RaptorVisitsComputer (RaptorAI raptor)
	{
		if (_state == ComputerState.WaitingToCrash) {
			_state = ComputerState.RaptorCrashingComputer;
			_crashTimer = CM.TimeToCrashWithRaptor;
			_timerLength = _crashTimer;
			pBar.gameObject.SetActive (true);
			raptor._rState = RaptorAI.RaptorState.FiddlingWithTarget;
			currentRaptorUser = raptor;
		}
	}

	/// <summary>
	/// Called when a computer randomly crashes
	/// </summary>
	private void crashComputer ()
	{
		// Only allow waiting to crash computers to crash
		if (_state == ComputerState.WaitingToCrash || _state == ComputerState.RaptorCrashingComputer ) {

			SetComputerColour ( _state == ComputerState.WaitingToCrash ? Color.magenta : Color.yellow  );
			_state = ComputerState.Crashed;

			_crashTimer = CM.TimeFromCrashToExplode;
			_timerLength = _crashTimer;
			pBar.gameObject.SetActive (true);
			// Release the raptor (if it hasn't already been released and there is one associated with this computer
			if (currentRaptorUser != null) {
				currentRaptorUser.FindNewTarget ();
				currentRaptorUser._rState = RaptorAI.RaptorState.HeadingToTarget;
			}
		}
	}

	private void SetComputerColour (Color colour)
	{
		this.gameObject.GetComponent<Renderer> ().material.color = colour;

	}

	public void rebootComputer ()
	{
		if (_state == ComputerState.Crashed || _state == ComputerState.RaptorCrashingComputer) {
			_state = ComputerState.Rebooting;
			CC.CancelCrashEffects ();
			_crashTimer =  rebootTime;
			_timerLength = _crashTimer;
			pBar.gameObject.SetActive (true);
			SetComputerColour (Color.green);
			FindObjectOfType<PlayerControl> ()._pState = PlayerControl.playerState.RebootingComputer;
		}
	}

    // Update is called once per frame
    void Update()
    {
		_crashTimer -= Time.deltaTime;

		if(pBar != null)
			pBar.progress = ((_timerLength - _crashTimer) / _timerLength) * 100;

		if (_crashTimer < 0) {
			if (_state == ComputerState.WaitingToCrash) {
				crashComputer ();
			} else if (_state == ComputerState.Crashed) {
				_state = ComputerState.Exploding;
				CC.TriggerExplosionEffect ();
				SetComputerColour (Color.black);
				_crashTimer = CM.TimeToEndGame;
				// TODO
			} else if (_state == ComputerState.RaptorCrashingComputer) {
				// Raptor has finished crashing the computer
				crashComputer ();
				currentRaptorUser.FindNewTarget ();
			} else if (_state == ComputerState.Rebooting) {
				SetComputerColour (Color.white);
				_state = ComputerState.WaitingToCrash;
				pBar.progress = 0;
				pBar.gameObject.SetActive (false);
				_crashTimer = Random.Range (CM.MinDefaultComputerCrashTime, CM.MaxDefaultComputerCrashTime);
				FindObjectOfType<PlayerControl> ()._pState = PlayerControl.playerState.Moving;
			} else if (_state == ComputerState.Exploding) {
				Application.Quit ();
			}
		} else if (_state == ComputerState.Crashed && _crashTimer < CC.audioData.clip.length && !CC.audioData.isPlaying) {
			CC.TriggerCountdownAudio ();
		} else if (_state == ComputerState.Crashed && _crashTimer < CM.TimeFromCrashToExplode - 5f) {
			CC.StartSmokingEffect ();
		}
    }
}

