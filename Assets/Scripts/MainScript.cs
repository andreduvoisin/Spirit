using UnityEngine;
using System.Collections;

public class MainScript : MonoBehaviour
{
	public KeyCode mRestartKey;
	public GUIText mScoreText;

	public int BOUNCE_SCORE;
	public int SWEETSPOT_SCORE;
	private int mScore;

	// Use this for initialization
	void Start ()
	{
		mScore = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		UpdateScore();
		CheckRestartInput();
	}

	void UpdateScore()
	{
		mScoreText.text = "Score: " + mScore;
	}

	public void AddScore(int score) { mScore += score; }
	public void SubtractScore(int score) { mScore -= score; }

	void CheckRestartInput()
	{
		if(Input.GetKeyDown(mRestartKey))
		{
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
