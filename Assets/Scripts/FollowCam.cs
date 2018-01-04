using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {

	public float height = 5f;
	public float distance = -50f;
	public GameObject player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 camPos = (Vector3.right * player.transform.position.x) + (Vector3.up * (player.transform.position.y + height))
		                 + (Vector3.forward * distance);
		
		transform.position = Vector3.Lerp(transform.position, camPos, 10 * Time.deltaTime);
		//transform.position = camPos;



	}
}
