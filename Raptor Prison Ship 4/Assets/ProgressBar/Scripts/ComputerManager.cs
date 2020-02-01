<<<<<<< HEAD:Raptor Prison Ship 4/Assets/_Project/Scripts/ComputerManager.cs
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerManager : MonoBehaviour
{
	private Computer [] computers;
	[SerializeField]
	private int WorkingComputers = 0;

	public float MinDefaultComputerCrashTime = 10f;
	public float MaxDefaultComputerCrashTime = 45f;
	public float TimeToCrashWithRaptor = 5f;
	public float TimeFromCrashToExplode = 30f;


    // Start is called before the first frame update
    void Start()
    {
		computers = FindObjectsOfType<Computer> ();
		WorkingComputers = computers.Length;
    }

	public void ComputerCrashed ()
	{
		WorkingComputers--;
	}

	public void ComputerRebooted ()
	{
		WorkingComputers++;
	}

	public Computer GetRaptorTarget ()
	{
		Computer targettedComputer;

		if (WorkingComputers > 0) {
			do {
				targettedComputer = computers [Random.Range (0, computers.Length)];
			} while (targettedComputer._state != Computer.ComputerState.WaitingToCrash);
		} else
			targettedComputer = null;
		
		return targettedComputer;

	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerManager : MonoBehaviour
{
	private Computer [] computers;
	[SerializeField]
	private int WorkingComputers = 0;

    // Start is called before the first frame update
    void Start()
    {
		computers = FindObjectsOfType<Computer> ();
		WorkingComputers = computers.Length;
    }

	public void ComputerCrashed ()
	{
		WorkingComputers--;
	}

	public void ComputerRebooted ()
	{
		WorkingComputers++;
	}

	public Computer GetRaptorTarget ()
	{
		Computer targettedComputer;

		if (WorkingComputers > 0) {
			do {
				targettedComputer = computers [Random.Range (0, computers.Length)];
			} while (targettedComputer._state != Computer.ComputerState.WaitingToCrash);
		} else
			targettedComputer = null;
		
		return targettedComputer;

	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
>>>>>>> 68cebb98d44a83cb4a78af7b1155da55d170afc8:Raptor Prison Ship 4/Assets/ProgressBar/Scripts/ComputerManager.cs
