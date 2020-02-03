using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour {
	public enum playerState {
		Moving,
		TrainingRaptor,
		SelectRaptorTool,
		SelectComputerTool,
		RebootingComputer
	}

	public playerState _pState;

	public NavMeshAgent player;

	public float PlayerComputerInteraction = 2.0f;  // Should be about the same as the computer's trigger colliders diameter/radius??

	public int targettedComputer = 0;
	private Computer currentComputer;
	private RaptorAI currentRaptor;

	[SerializeField]
	private RectTransform ComputerToolsPanel;
	[SerializeField]
	private RectTransform RaptorToolsPanel;

	public Sprite [] ComputerInterfaces;
	public Image ComputerToolHint;

	private int ClickLayerMask;

	// Start is called before the first frame update
	void Start ()
	{
		_pState = playerState.Moving;
		targettedComputer = 0;  // Not aiming at a computer
		ClickLayerMask = LayerMask.GetMask ("Floor", "Computer", "Raptor");
	}

	public void ShowComputerTools ()
	{
		if (ComputerInterfaces.Length > 0) {
			ComputerToolHint.sprite = ComputerInterfaces [(int)currentComputer.computerType];
		}
		_pState = playerState.SelectComputerTool;
		ComputerToolsPanel.gameObject.SetActive (true);
	}

	public void HideComputerTools ()
	{
		ComputerToolsPanel.gameObject.SetActive (false);
	}

	public void ShowRaptorTools ()
	{
		_pState = playerState.SelectRaptorTool;
		RaptorToolsPanel.gameObject.SetActive (true);
	}

	public void HideRaptorTools ()
	{
		RaptorToolsPanel.gameObject.SetActive (false);

	}

	public void UseComputerTool (int toolID)
	{
		int result = currentComputer.Tooled (toolID);

		if (result == -1) {
			_pState = playerState.Moving;
		} else {
			currentComputer.rebootComputer ();
		}

		currentComputer = null;
		targettedComputer = 0;
		HideComputerTools ();
	}

	public void UseRaptorTool (int toolID)
	{
		int result = currentRaptor.Tooled (toolID);

		// Using the stick
		if (result == 0) {
			_pState = playerState.Moving;
		}
		else if (result == 1)   // Training the raptor properly
		{
			_pState = playerState.TrainingRaptor;
		}
		else if (result == -1)  // Not sure how we got here, but we did, so run away
		{
			_pState = playerState.Moving;
			currentRaptor = null;
		}

		HideRaptorTools ();
	}

	public void LessonOver ()
	{
		_pState = playerState.Moving;
		currentRaptor = null;

	}

	public void TooSlowRaptorMovedOn ()
	{
		if (_pState == playerState.SelectRaptorTool) {
			HideRaptorTools ();
			_pState = playerState.Moving;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			Ray raything = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit rayResult;

			if (_pState == playerState.Moving) {

				if (Physics.Raycast (raything, out rayResult, 200f, ClickLayerMask)) {
					player.SetDestination (rayResult.point);

					if (rayResult.collider.gameObject.layer == LayerMask.NameToLayer ("Computer")) {
						currentComputer = rayResult.collider.gameObject.GetComponent<Computer> ();
						targettedComputer = currentComputer.computerID;
						currentRaptor = null;
						if ((player.destination - player.gameObject.transform.position).magnitude < PlayerComputerInteraction) {
							if (currentComputer._state == Computer.ComputerState.Crashed) {
								ShowComputerTools ();
							}
						}
					} else if (rayResult.collider.gameObject.layer == LayerMask.NameToLayer ("Raptor")) {
						targettedComputer = 0;
						currentComputer = null;
						currentRaptor = rayResult.collider.gameObject.GetComponent<RaptorAI> ();

						if ((player.destination - player.gameObject.transform.position).magnitude < PlayerComputerInteraction) {
							if (currentRaptor._rState == RaptorAI.RaptorState.FiddlingWithComputer) {
								ShowRaptorTools ();
								currentRaptor.playerInteracting = this;
							}
						}

					} else {
						targettedComputer = 0;
						currentComputer = null;
					}
				}
			}
		}
	}
}