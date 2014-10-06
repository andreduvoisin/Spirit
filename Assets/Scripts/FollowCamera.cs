using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {
	public GameObject gameObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = gameObject.transform.position + new Vector3(0, 1, -10);
	}
}
