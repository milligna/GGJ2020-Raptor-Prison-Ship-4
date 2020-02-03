using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
	[SerializeField]
	private string NextScene;

    public void StartGame() { SceneManager.LoadScene(NextScene); }

    public void QuitGame() { Application.Quit(); }
}
