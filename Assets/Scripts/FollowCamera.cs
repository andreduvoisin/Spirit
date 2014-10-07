using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {
	public GameObject gameObject;
	public float mSpringConstant = 0.0f;
	public float mDampConstant = 0.0f;
	public float mForwardDistFromBall = 5.0f;
	public float mUpDistFromBall = 5.0f;
	private Vector3 mIdealCameraPos = new Vector3();
	private Vector3 mVelocity = new Vector3();

	// Use this for initialization
	void Start () 
	{
		mDampConstant *= Mathf.Sqrt(mSpringConstant);
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		transform.rotation = Quaternion.LookRotation(gameObject.transform.position - transform.position);

		Vector3 direction = gameObject.transform.rigidbody.velocity;
		direction.Normalize();
		mIdealCameraPos = gameObject.transform.position - direction * mForwardDistFromBall;
		mIdealCameraPos += transform.up * mUpDistFromBall;
		Vector3 displacement = transform.position - mIdealCameraPos;
		Vector3 springAccel = (-mSpringConstant * displacement) - (mDampConstant * mVelocity);
		mVelocity += springAccel * Time.deltaTime;
		transform.position += mVelocity * Time.deltaTime;
	}
}
