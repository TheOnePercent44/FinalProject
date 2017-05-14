using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelSwapper : MonoBehaviour
{

	// Use this for initialization
	void Start () {
	
	}

	public void LoadLevel(string s)
	{
		SceneManager.LoadScene(s);
	}
}
