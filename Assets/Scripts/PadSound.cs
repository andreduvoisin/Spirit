using UnityEngine;
using System.Collections;

public class PadSound : MonoBehaviour 
{
	public AudioClip mBounceSound;


	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnCollisionEnter(Collision collision)
	{
		audio.PlayOneShot(mBounceSound);
	}
}
