using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OPenDoor : MonoBehaviour
{

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {

        animator.SetInteger("OpenDoor", 1);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
