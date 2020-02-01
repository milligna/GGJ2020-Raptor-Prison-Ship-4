using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RaptorAI : MonoBehaviour
{

	public Computer targettedComputer;
	[SerializeField]
	private GameObject targettedLocation;
	[SerializeField]
	private NavMeshAgent raptorAgent;

	private ComputerManager CM;

	[SerializeField]
	private NonComputer [] nonComputerLocations;


    // Start is called before the first frame update
    void Start()
    {
		CM = FindObjectOfType<ComputerManager> ();
		nonComputerLocations = FindObjectsOfType<NonComputer> ();
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
		if (targettedLocation == null) {

			FindNewTarget ();

		}
    }
}
