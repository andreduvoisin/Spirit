using UnityEngine;
using System.Collections;

public class AlphaMasking : MonoBehaviour 
{
	public GameObject mBall;
	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = mBall.transform.position;
		transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);

		PadJumpManager padJumpMgr = (PadJumpManager) mBall.GetComponent("PadJumpManager");
		float distRemaining = padJumpMgr.mDistanceRemaining;
		float fullDist = padJumpMgr.mDistanceToTravel;


		float alpha = Mathf.InverseLerp(0.0f, fullDist, distRemaining);
		if(alpha == 0.0f)
		{
			alpha = 0.0001f;
		}
		renderer.material.SetFloat("_Cutoff", alpha);
	}
}
