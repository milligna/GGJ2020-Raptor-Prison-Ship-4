using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyPress : MonoBehaviour
{
    public string NextScene = "Main";

    // Detects if any key has been pressed.
    void Update()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene(NextScene);
        }
    }
}
