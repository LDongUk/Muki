using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnPathScript : MonoBehaviour {

	public EditorPathScript PathToFollow;
	public int CurrentWayPointID = 0;
	public float speed;
	private float reachDistance = 1.0f;
	public float rotationSpeed = 5.0f;
	public string pathName;

	Vector3 current_position;
	bool left = true;

	void Start () {
		PathToFollow = GameObject.Find (pathName).GetComponent<EditorPathScript> ();
	}
	
	void Update () {
		float distance = Vector3.Distance (PathToFollow.path_objs [CurrentWayPointID].position, transform.position);
		transform.position = Vector3.MoveTowards (transform.position, PathToFollow.path_objs [CurrentWayPointID].position, Time.deltaTime * speed);

		var rotation = Quaternion.LookRotation (PathToFollow.path_objs [CurrentWayPointID].position - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * rotationSpeed);

		if (left) {

			if (distance <= reachDistance) {
				CurrentWayPointID++;
			}

			if (CurrentWayPointID >= PathToFollow.path_objs.Count) {
				left = false;
				CurrentWayPointID--;
			}
		}

		else {
			if (distance <= reachDistance) {
				CurrentWayPointID--;
			}

			if (CurrentWayPointID == 0) {
				left = true;
				CurrentWayPointID++;
			}
		}
	}
}
