<<<<<<< HEAD:Raptor Prison Ship 4/Assets/_Project/Scripts/Computer.cs
﻿using System.Collections;
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
		RaptorSafelyUsingComputer,
		Crashed,
		Exploding
	}

	public int computerID;
	public ComputerType computerType;
	public ComputerState _state;
	[SerializeField]
	private float _crashTimer;
	private RaptorAI currentRaptorUser;

	private ComputerManager CM;

    // Start is called before the first frame update
    void Start()
    {
		CM = FindObjectOfType<ComputerManager> ();

		if (computerID == 0) {
			computerID = Random.Range (0, 100);
		}
		computerType =  (ComputerType)Random.Range (0, System.Enum.GetValues(typeof(ComputerType)).Length);
		_state = ComputerState.WaitingToCrash;
		_crashTimer = Random.Range (CM.MinDefaultComputerCrashTime, CM.MaxDefaultComputerCrashTime);
    }

	private void OnTriggerEnter (Collider other)
	{
=======
﻿using System.Collections;
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
		Crashed,
		SafelyOccupied,
		Exploding
	}

	public int computerID;
	public ComputerType computerType;
	public ComputerState _state;
	private float _crashTimer;

	private ComputerManager CM;

    // Start is called before the first frame update
    void Start()
    {
		CM = FindObjectOfType<ComputerManager> ();

		if (computerID == 0) {
			computerID = Random.Range (0, 100);
		}
		computerType =  (ComputerType)Random.Range (0, System.Enum.GetValues(typeof(ComputerType)).Length);
		_state = ComputerState.WaitingToCrash;
		_crashTimer = Random.Range (3f, 20f);
    }

	private void OnTriggerEnter (Collider other)
	{
>>>>>>> 68cebb98d44a83cb4a78af7b1155da55d170afc8:Raptor Prison Ship 4/Assets/ProgressBar/Scripts/Computer.cs
		// If we've already clicked on the computer before being in range
		if (other.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			if (other.gameObject.GetComponent<PlayerControl> ().targettedComputer == computerID) {
				rebootComputer ();
			}
		} else if(other.gameObject.layer == LayerMask.NameToLayer("Raptor")) {
			if (_state == ComputerState.WaitingToCrash) {
<<<<<<< HEAD:Raptor Prison Ship 4/Assets/_Project/Scripts/Computer.cs
				RaptorVisitsComputer ( other.gameObject.GetComponent<RaptorAI>() );
			}
		}
	}

	private void OnTriggerStay (Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer ("Raptor")) {
			if (other.gameObject.GetComponent<RaptorAI> ().targettedComputer == this && _state == ComputerState.WaitingToCrash ) {
				Debug.Log ("Here");
				RaptorVisitsComputer (other.gameObject.GetComponent<RaptorAI> ());
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
			currentRaptorUser = raptor;
		}
	}

	/// <summary>
	/// Called when a computer randomly crashes
	/// </summary>
	private void crashComputer ()
	{
		Debug.Log (_state);
		// Only allow waiting to crash computers to crash
		if (_state == ComputerState.WaitingToCrash || _state == ComputerState.RaptorCrashingComputer ) {
			SetComputerColour (Color.magenta);
			_state = ComputerState.Crashed;

			_crashTimer = CM.TimeFromCrashToExplode;
			// Release the raptor (if it hasn't already been released and there is one associated with this computer
			CM.ComputerCrashed ();
		}
	}

	private void SetComputerColour (Color colour)
	{
		this.gameObject.GetComponent<Renderer> ().material.color = colour;

	}

	public void rebootComputer ()
	{
		if (_state == ComputerState.Crashed || _state == ComputerState.RaptorCrashingComputer) {
			_state = ComputerState.WaitingToCrash;
			_crashTimer = Random.Range (CM.MinDefaultComputerCrashTime, CM.MaxDefaultComputerCrashTime);
			CM.ComputerRebooted ();
			SetComputerColour (Color.white);
		}
	}

    // Update is called once per frame
    void Update()
    {
		_crashTimer -= Time.deltaTime;
		if (_crashTimer < 0) {
			if (_state == ComputerState.WaitingToCrash) {
				crashComputer ();
			} else if (_state == ComputerState.Crashed) {
				_state = ComputerState.Exploding;
				SetComputerColour (Color.black);
			} else if (_state == ComputerState.RaptorCrashingComputer) {
				crashComputer ();
				currentRaptorUser.FindNewTarget ();
			}
		}
    }
}
=======
				crashComputer ();
				other.gameObject.GetComponent<RaptorAI> ().FindNewTarget ();
			}
		}
	}

	private void OnTriggerStay (Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer ("Raptor")) {
			if (other.gameObject.GetComponent<RaptorAI> ().targettedComputer == this) {
				crashComputer ();
				other.gameObject.GetComponent<RaptorAI> ().FindNewTarget ();
			}
		}
	}

	private void crashComputer ()
	{
		// Only allow waiting to crash computers to crash
		if (_state == ComputerState.WaitingToCrash) {
			SetComputerColour (Color.magenta);
			_state = ComputerState.Crashed;
			_crashTimer = Random.Range (10f, 20f);
			// Release the raptor (if it hasn't already been released and there is one associated with this computer
			CM.ComputerCrashed ();
		}
	}

	private void SetComputerColour (Color colour)
	{
		this.gameObject.GetComponent<Renderer> ().material.color = colour;

	}

	public void rebootComputer ()
	{
		if (_state == ComputerState.Crashed) {
			_state = ComputerState.WaitingToCrash;
			_crashTimer = Random.Range (3f, 20f);
			CM.ComputerRebooted ();
			SetComputerColour (Color.white);
		}
	}

    // Update is called once per frame
    void Update()
    {
		_crashTimer -= Time.deltaTime;

		if (_crashTimer < 0) {
			if (_state == ComputerState.WaitingToCrash) {
				crashComputer ();
			} else if (_state == ComputerState.Crashed) {
				_state = ComputerState.Exploding;
				SetComputerColour (Color.black);
			}
		}
    }
}
>>>>>>> 68cebb98d44a83cb4a78af7b1155da55d170afc8:Raptor Prison Ship 4/Assets/ProgressBar/Scripts/Computer.cs
