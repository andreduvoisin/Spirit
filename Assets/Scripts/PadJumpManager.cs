using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class PadJumpManager : MonoBehaviour
{
	public GameObject pads;
	public float mDistanceToTravel;
	public float mDistanceRemaining;
	public List<GameObject> padList = new List<GameObject>();
	public int currentPad;
	public int padIndex;
	public bool canJump;
	public float errorMargin;
	public Vector3 hitSpot;

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

		// This fails if the next pad is placed less than one ball wide
		Vector3 direction = rigidbody.velocity;
		direction.Normalize();
		RaycastHit hitInfo;
		Physics.SphereCast(transform.position + (((SphereCollider) collider).radius * 2 + 0.01f) * direction, ((SphereCollider) collider).radius, direction, out hitInfo);
		mDistanceToTravel = hitInfo.distance + (((SphereCollider) collider).radius * 2 + 0.01f);
		mDistanceRemaining = mDistanceToTravel;
	}
	
	// Update is called once per frame
	void Update ()
	{
		float dist = Vector3.Distance(padList[currentPad].transform.position, transform.position);
		if(dist <= errorMargin)
		{
			canJump = true;
		}
		else
		{
			if(canJump)
			{
				rigidbody.position = new Vector3(-100, -500, -1000);
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

	void FixedUpdate()
	{
		mDistanceRemaining -= (rigidbody.velocity * Time.deltaTime).magnitude;
	}

	void OnCollisionEnter(Collision collision)
	{
		Jump();

		// This fails if the next pad is placed less than one ball wide
		Vector3 direction = rigidbody.velocity;
		direction.Normalize();
		RaycastHit hitInfo;
		Physics.SphereCast(transform.position + (((SphereCollider) collider).radius * 2 + 0.01f) * direction, ((SphereCollider) collider).radius, direction, out hitInfo);
		mDistanceToTravel = hitInfo.distance + (((SphereCollider) collider).radius * 2 + 0.01f);
		mDistanceRemaining = mDistanceToTravel;
	}

	void Jump()
	{
		int targetPad = (padIndex + 1) % padList.Count;
		
		Vector3 dir = padList[targetPad].transform.position - transform.position;
		rigidbody.velocity = Vector3.Normalize(dir) * 10;

		++padIndex;
	}
}
