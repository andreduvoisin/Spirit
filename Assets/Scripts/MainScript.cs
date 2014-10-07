using UnityEngine;
using System.Collections;

public class MainScript : MonoBehaviour
{
	public KeyCode mRestartKey;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		CheckRestartInput();
	}

	void CheckRestartInput()
	{
		if(Input.GetKeyDown(mRestartKey))
		{
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
