using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam1 : MonoBehaviour {

	public float dist = 50.0f;
	public float height = 7.0f;
	public Transform target;
	private Transform tr;

	float shakes = 0f;
	bool isShaking = false;
	private Vector3 originalPos;
	float shakeAmount = 0.3f;

	// Use this for initialization
	void Start () {
		tr = GetComponent<Transform> ();

	}


	void FixedUpdate () {
		if (target && !isShaking) {
			tr.position = target.position - (Vector3.forward * dist) + (Vector3.up * height);
			tr.LookAt (target);

		} else if (target && isShaking) {
			originalPos = tr.position = target.position - (Vector3.forward * dist) + (Vector3.up * height);

			if (isShaking && shakes > 0) {
				tr.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
				//tr.position += new Vector3 (0, 0, -50f);
				shakes -= Time.deltaTime * 1.0f;

			} else if (isShaking && shakes < 0) {

				shakes = 0f;
				tr.localPosition = originalPos;
				isShaking = false;
			}

		}
			


	}

	public void ShakeCamera(float shaking){
		isShaking = true;
		originalPos = tr.position;
		shakes = shaking;

	}

	IEnumerator Shaking(){

		while (shakes > 0) {
			tr.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
			//tr.position += new Vector3 (0, 0, -50f);
			shakes -= Time.deltaTime * 1.0f;

		}

		shakes = 0f;
		tr.localPosition = originalPos;
		isShaking = false;
		yield return null;

	}


}
