using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class PadJumpManager : MonoBehaviour
{
	public GameObject pads;
	public List<GameObject> padList = new List<GameObject>();
	public int padIndex;
	public int currentPad;
	public bool shouldJump;
	public bool canJump;

	// Use this for initialization
	void Start ()
	{
		foreach(Transform pad in pads.transform)
		{
			if(pad.gameObject.name.Contains("Pad"))
			{
				padList.Add(pad.gameObject);
			}
		}
		rigidbody.velocity = new Vector3(0, -2, 0);
		padIndex = 0;
		currentPad = 0;
		canJump = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		float dist = Vector3.Distance(padList[currentPad].transform.position, transform.position);
		if(dist <= 1.5)
		{
			canJump = true;
		}
		else
		{
			if(canJump)
			{
				//rigidbody.position = new Vector3(-100, -500, -1000);
			}
			canJump = false;
		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			if(canJump)
			{
				currentPad = (currentPad + 1) % padList.Count;
				canJump = false;
			}
			else
			{
				// game over
				rigidbody.position = new Vector3(-100, -500, -1000);
			}
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		Jump();
	}

	void Jump()
	{
		int targetPad = (padIndex + 1) % padList.Count;
		
		Vector3 dir = padList[targetPad].transform.position - transform.position;
		rigidbody.velocity = Vector3.Normalize(dir) * 10;

		++padIndex;
	}
}
