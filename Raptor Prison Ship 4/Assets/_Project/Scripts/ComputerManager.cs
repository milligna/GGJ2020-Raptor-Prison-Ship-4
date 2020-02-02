using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerManager : MonoBehaviour
{
	private Computer [] computers;
	[SerializeField]

	public float MinDefaultComputerCrashTime = 10f;
	public float MaxDefaultComputerCrashTime = 45f;
	public float TimeToCrashWithRaptor = 5f;
	public float TimeFromCrashToExplode = 30f;
	public float TimeToEndGame = 7f;

	private List<Computer> NonCrashedComputers;
	private List<Computer> CrashedComputers;

	private AudioClip [] rebootSound;

    // Start is called before the first frame update
    void Start()
    {
		computers = FindObjectsOfType<Computer> ();
		NonCrashedComputers = new List<Computer> ();
		CrashedComputers = new List<Computer> ();

		rebuildComputerList ();

		List<Computer> tmpList = new List<Computer> ();

		for (int i = 0; i < computers.Length; i++) {
			if (computers [i].linkedDoor != null) {
				tmpList.Add (computers [i]);
			}
		}

		if (tmpList.Count > 0) {
			tmpList [Random.Range (0, tmpList.Count)].SetTimer (3.0f);
		}

    }

	public void rebuildComputerList ()
	{
		NonCrashedComputers.Clear ();
		CrashedComputers.Clear ();

		for (int i = 0; i < computers.Length; i++) {
			if (computers [i]._state == Computer.ComputerState.WaitingToCrash) {
				NonCrashedComputers.Add (computers [i]);
			} else {
				CrashedComputers.Add (computers [i]);
			}

		}
	}


	public Computer GetRaptorTarget ()
	{
		Computer targettedComputer;

		rebuildComputerList ();

		if (NonCrashedComputers.Count > 0) {
			targettedComputer = NonCrashedComputers [Random.Range (0, NonCrashedComputers.Count)];
		} else {
			targettedComputer = null;
		}
		return targettedComputer;

	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
