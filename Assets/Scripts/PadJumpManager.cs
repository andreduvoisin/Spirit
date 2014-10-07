using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class PadJumpManager : MonoBehaviour
{
	public GameObject mPads;
	public float mErrorMargin;
	public float mSweetspotMargin;
	public KeyCode mKeyCode;
	public GameObject mMain;
	
	public float mDistanceToTravel;
	public float mDistanceRemaining;
	public float mMissedInputDist;
	public List<GameObject> mPadList = new List<GameObject>();
	public int mPadIndex;
	
	public enum EBallState { BeforeJump, CanJump, MissedJump, Jumped };
	public EBallState mBallState;

	private MainScript mMainScript;
	
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
		mBallState = EBallState.BeforeJump;

		mMainScript = (MainScript) mMain.GetComponent("MainScript");
		
		FindDistanceToTravel();
	}
	
	// Update is called once per frame
	void Update ()
	{
		switch(mBallState)
		{
		case EBallState.BeforeJump:
			if(mDistanceRemaining <= mErrorMargin)
			{
				mBallState = EBallState.CanJump;
			}
			CheckJumpInput();
			break;
		case EBallState.CanJump:
			if(mDistanceRemaining <= mErrorMargin)
			{
				CheckJumpInput();
			} 
			else
			{
				KillPlayer();
			}
			break;
		case EBallState.MissedJump:
			if(mMissedInputDist <= mErrorMargin)
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
				CheckSweetspot();
				mBallState = EBallState.Jumped;
				break;
			case EBallState.MissedJump:
				CheckSweetspot();
				mBallState = EBallState.BeforeJump;
				mMissedInputDist = 0.0f;
				break;
			}
		}
	}

	void CheckSweetspot()
	{
		float sweetspotDistance = 0.0f;
		if(mBallState == EBallState.CanJump)
		{
			sweetspotDistance = mDistanceRemaining;
		}
		else if(mBallState == EBallState.MissedJump)
		{
			sweetspotDistance = mMissedInputDist;
		}

		if(sweetspotDistance <= mSweetspotMargin)
		{
			mMainScript.AddScore(mMainScript.SWEETSPOT_SCORE);
			// play sweetspot particles
		}
		else
		{
			mMainScript.AddScore(mMainScript.BOUNCE_SCORE);
		}
	}
	
	void KillPlayer()
	{
		gameObject.SetActive(false);
	}
	
	void FixedUpdate()
	{
		mDistanceRemaining -= (rigidbody.velocity * Time.deltaTime).magnitude;
		if(mBallState == EBallState.MissedJump)
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
		switch(mBallState)
		{
		case EBallState.CanJump:
			mBallState = EBallState.MissedJump;
			break;
		case EBallState.Jumped:
			mBallState = EBallState.BeforeJump;
			break;
		default:
			// Should never be in any other state here.
			Debug.Break();
			break;
		}
	}
}