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
		float dist = Vector3.Distance(padJumpMgr.padList[padJumpMgr.currentPad].transform.position, padJumpMgr.transform.position);
		int prevPadIndex = padJumpMgr.currentPad - 1;
		if(prevPadIndex < 0) 
		{
			prevPadIndex = padJumpMgr.padList.Count - 1;
		}

		float fullDist = Vector3.Distance(padJumpMgr.padList[padJumpMgr.currentPad].transform.position, padJumpMgr.padList[prevPadIndex].transform.position);

		float alpha = Mathf.InverseLerp(0.0f, fullDist, dist);
		if(alpha == 0.0f)
		{
			alpha = 0.0001f;
		}
		renderer.material.SetFloat("_Cutoff", alpha);
	}
}
