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

	public DoorControl linkedDoor;

	public int computerID;
	public ComputerType computerType;
	public float rebootTime = 5f;
	public ComputerState _state;
	[SerializeField]
	private float _crashTimer = 0;
	private float _timerLength;
	private RaptorAI currentRaptorUser;

	private AudioSource computerSpeaker;

	private ComputerManager CM;

    // Start is called before the first frame update
    void Start()
    {
		if(pBar == null)
			pBar = this.gameObject.GetComponentInChildren<LinearProgressBarController> (true);

		computerSpeaker = GetComponent<AudioSource> ();


		CC = GetComponent<CrashController> ();
		CM = FindObjectOfType<ComputerManager> ();

		if (computerID == 0) {
			computerID = Random.Range (0, 100);
		}

		// Pick a random computer type 
		computerType =  (ComputerType)Random.Range (0, System.Enum.GetValues(typeof(ComputerType)).Length);
		_state = ComputerState.WaitingToCrash;

		// if the timer is greater than 0, we must have set it in the ComputerManager->start, so don't over write it
		if(_crashTimer <= 0)
			_crashTimer = Random.Range (CM.MinDefaultComputerCrashTime, CM.MaxDefaultComputerCrashTime);
    }

	public void SetTimer (float thisTime)
	{
		_crashTimer = thisTime;
	}

	private void OnTriggerEnter (Collider other)
	{
		// If we've already clicked on the computer before being in range
		if (other.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			if (other.gameObject.GetComponent<PlayerControl> ().targettedComputer == computerID) {
				if (_state == ComputerState.Crashed) {
					other.gameObject.GetComponent<PlayerControl> ().ShowComputerTools ();
				}
			}
		} else if(other.gameObject.layer == LayerMask.NameToLayer("Raptor")) {
			RaptorAI tmpRaptor = other.gameObject.GetComponent<RaptorAI> ();
			if (tmpRaptor.targettedComputer == this) {
				if (tmpRaptor._rState == RaptorAI.RaptorState.Content) {
					// Take permanent control of this computer.
					if (currentRaptorUser != null) {
						// There's another raptor at this computer already??
						// Chase it away
						RaptorInterferenceInterferedWith ();
					}
					// Set up this computer to be happy forever
					TrainedRaptorAtComputer ();

				} else {
					if (_state == ComputerState.WaitingToCrash) {
						RaptorVisitsComputer (tmpRaptor);
					} else
					{
						// Computer crashed before the raptor could crash it, but after it targetted it
						tmpRaptor.FindNewTarget ();
					}
				}
			}
		}
	}

	private void OnTriggerStay (Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer ("Raptor")) {
			RaptorAI tmpRaptor = other.gameObject.GetComponent<RaptorAI> ();
			if (tmpRaptor.targettedComputer == this)
			{
				if (tmpRaptor._rState == RaptorAI.RaptorState.Content) {
					if (currentRaptorUser != null) {
						RaptorInterferenceInterferedWith ();
					}
					// Set up this computer to be happy forever but only if we haven't already done that.
					if (_state != ComputerState.RaptorSafelyUsingComputer) {
						TrainedRaptorAtComputer ();
					}

				} else {
					if (_state == ComputerState.WaitingToCrash) {
						RaptorVisitsComputer (other.gameObject.GetComponent<RaptorAI> ());
					} else if (_state == ComputerState.Crashed) {
						other.gameObject.GetComponent<RaptorAI> ().FindNewTarget ();
					}
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
			raptor._rState = RaptorAI.RaptorState.FiddlingWithComputer;
			currentRaptorUser = raptor;
			computerSpeaker.PlayOneShot (CM.TypingSound);
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

			if (linkedDoor != null) {
				linkedDoor.ReleaseTheRaptor ();
			}

			_crashTimer = CM.TimeFromCrashToExplode;
			_timerLength = _crashTimer;
			pBar.gameObject.SetActive (true);
			// Release the raptor (if it hasn't already been released and there is one associated with this computer
			if (currentRaptorUser != null) {
				currentRaptorUser.FindNewTarget ();
				currentRaptorUser._rState = RaptorAI.RaptorState.HeadingToTarget;
				currentRaptorUser = null;
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
			_crashTimer =  rebootTime;
			_timerLength = _crashTimer;
			pBar.gameObject.SetActive (true);
			SetComputerColour (Color.green);
			FindObjectOfType<PlayerControl> ()._pState = PlayerControl.playerState.RebootingComputer;
		}
	}

	private void TrainedRaptorAtComputer ()
	{
		SetComputerColour (Color.cyan);
		_crashTimer = 0;
		_state = ComputerState.RaptorSafelyUsingComputer;
		pBar.gameObject.SetActive (false);  // Ensure the progress bar gets hidden
		CC.CancelCrashEffects ();
		CM.ContentRaptors++;
		// So player can't click on computer anymore
		this.gameObject.layer = LayerMask.NameToLayer ("NonComputer");
	}

	private void PlayRebootSound ()
	{
		if(CM.rebootSound.Length > 0)
			computerSpeaker.PlayOneShot (CM.rebootSound [Random.Range (0, CM.rebootSound.Length)]);
	}

	public void RaptorInterferenceInterferedWith ()
	{
		_state = ComputerState.WaitingToCrash;
		pBar.progress = 0;
		pBar.gameObject.SetActive (false);
		currentRaptorUser = null;
		_crashTimer = Random.Range (CM.MinDefaultComputerCrashTime, CM.MaxDefaultComputerCrashTime);
	}

	public int Tooled (int toolID)
	{
		// Just incase the computer blew up before a tool was selected
		if (_state == Computer.ComputerState.Exploding) {
			return -1;
		} else if (toolID == (int)computerType) {
			if(toolID < CM.ComputerToolSounds.Length)
				computerSpeaker.PlayOneShot (CM.ComputerToolSounds [toolID]);
			return 1;
		} else {
			return -1;
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

				// If user was in the process of selecting a tool to use on the computer, need to reset the player
				PlayerControl PC = FindObjectOfType<PlayerControl> ();
				if (PC._pState == PlayerControl.playerState.SelectComputerTool && PC.targettedComputer == computerID ) {
					// select a non-existant tool which is the same as essentially cancelling the tool selection
					PC.UseComputerTool (-1);
				}
				_crashTimer = CM.TimeToEndGame;
				// TODO
			} else if (_state == ComputerState.RaptorCrashingComputer) {
				// Raptor has finished crashing the computer
				crashComputer ();
			} else if (_state == ComputerState.Rebooting) {
				SetComputerColour (Color.white);
				CC.CancelCrashEffects ();
				PlayRebootSound ();
				_state = ComputerState.WaitingToCrash;
				pBar.progress = 0;
				pBar.gameObject.SetActive (false);
				_crashTimer = Random.Range (CM.MinDefaultComputerCrashTime, CM.MaxDefaultComputerCrashTime);
				FindObjectOfType<PlayerControl> ()._pState = PlayerControl.playerState.Moving;
			} else if (_state == ComputerState.Exploding) {
				CM.EndGameLose ();
			}
		} else if (_state == ComputerState.Crashed && _crashTimer < CC.audioData.clip.length && !CC.audioData.isPlaying) {
			CC.TriggerCountdownAudio ();
		} else if (_state == ComputerState.Crashed && _crashTimer < CM.TimeFromCrashToExplode - 5f) {
			CC.StartSmokingEffect ();
		}
    }
}

