using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class PadJumpManager : MonoBehaviour
{
	public GameObject mPads;
	public float mErrorMargin;
	public KeyCode mKeyCode;
	
	public float mDistanceToTravel;
	public float mDistanceRemaining;
	public float mMissedInputDist;
	public List<GameObject> mPadList = new List<GameObject>();
	public int mPadIndex;
	public bool bCanJump;
	public bool bMissedInput;
	
	public enum EBallState { BeforeJump, CanJump, Jumped };
	public EBallState mBallState;
	
	// Use this for initialization
	void Start ()
	{
		foreach(Transform pad in mPads.transform)
		{
			if(pad.gameObject.name.Contains("Pad"))
			{
				mPadList.Add(pad.gameObject);
			}
		}
		rigidbody.velocity = new Vector3(0, -2, 0);
		mPadIndex = 0;
		mMissedInputDist = 0.0f;
		bCanJump = false;
		bMissedInput = false;
		mBallState = EBallState.BeforeJump;
		
		FindDistanceToTravel();
	}
	
	// Update is called once per frame
	void Update ()
	{
		float distanceToCurrentPad = bMissedInput ? mMissedInputDist : mDistanceRemaining;
		switch(mBallState)
		{
		case EBallState.BeforeJump:
			if(distanceToCurrentPad <= mErrorMargin)
			{
				mBallState = EBallState.CanJump;
			}
			CheckJumpInput();
			break;
		case EBallState.CanJump:
			if(distanceToCurrentPad <= mErrorMargin)
			{
				CheckJumpInput();
			} 
			else
			{
				KillPlayer();
			}
			break;
		case EBallState.Jumped:
			break;
		}
	}
	
	void CheckJumpInput()
	{
		if(Input.GetKeyDown(mKeyCode))
		{
			switch(mBallState)
			{
			case EBallState.BeforeJump:
			case EBallState.Jumped:
				KillPlayer();
				break;
			case EBallState.CanJump:
				mBallState = EBallState.Jumped;
				if(bMissedInput)
				{
					mMissedInputDist = 0.0f;
					bMissedInput = false;
				}
				break;
			}
		}
	}
	
	void KillPlayer()
	{
		//gameObject.SetActive(false);
	}
	
	void FixedUpdate()
	{
		mDistanceRemaining -= (rigidbody.velocity * Time.deltaTime).magnitude;
		if(bMissedInput)
		{
			mMissedInputDist += (rigidbody.velocity * Time.deltaTime).magnitude;
		}
	}
	
	void OnCollisionEnter(Collision collision)
	{
  		Jump();
		FindDistanceToTravel();
		CheckMissedInput();
	}
	
	void Jump()
	{
		mPadIndex = (mPadIndex + 1) % mPadList.Count;
		Vector3 dir = mPadList[mPadIndex].transform.position - transform.position;
		rigidbody.velocity = Vector3.Normalize(dir) * 10;
	}
	
	void FindDistanceToTravel()
	{
		// This fails if the next pad is placed less than one ball wide
		Vector3 direction = rigidbody.velocity;
		direction.Normalize();
		RaycastHit hitInfo;
		Physics.SphereCast(transform.position + (((SphereCollider) collider).radius * 2 + 0.01f) * direction, ((SphereCollider) collider).radius, direction, out hitInfo);
		mDistanceToTravel = hitInfo.distance + (((SphereCollider) collider).radius * 2 + 0.01f);
		mDistanceRemaining = mDistanceToTravel;
	}
	
	void CheckMissedInput()
	{
		mMissedInputDist = 0.0f;
		bMissedInput = !(mBallState == EBallState.Jumped);
		if(!bMissedInput)
		{
			mBallState = EBallState.BeforeJump;
		}
	}
}
