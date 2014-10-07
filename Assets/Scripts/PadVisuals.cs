using UnityEngine;
using System.Collections;

public class PadVisuals : MonoBehaviour
{
	public GameObject mBall;
	public GameObject mParticleObject;
	private ParticleSystem[] mParticleSystem;

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
		mParticleObject = Instantiate (mParticleObject, transform.position, transform.rotation) as GameObject;
		mParticleSystem = mParticleObject.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem pSys in mParticleSystem)
		{
			pSys.Clear ();
			pSys.Play ();
		}
	}
}
