using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {

	public float dist = 50.0f;
	public float height = 7.0f;
	public Transform target;
	private Transform tr;

	float shakes = 0f;
	bool isShaking = false;
	private Vector3 originalPos;
	float shakeAmount = 0.3f;
	private PlayerCtrl2 playerctr;
	//private PlayerCtrl2 playerctr;

	int pre_nodeNum, next_nodeNum;
	PatrolPos pat1R;
	PatrolPos pat2R;
	PatrolPos pat2L;
	PatrolPos pat3R;

	// Use this for initialization
	void Start () {
		tr = GetComponent<Transform> ();
		playerctr = GameObject.Find ("Muki").GetComponent<PlayerCtrl2> ();

		pat1R = GameObject.Find ("Patrols1_R").GetComponent<PatrolPos> ();
		pat2R = GameObject.Find ("Patrols2_R").GetComponent<PatrolPos> ();
		pat2L = GameObject.Find ("Patrols2_L").GetComponent<PatrolPos> ();
		pat3R = GameObject.Find ("Patrols3_R").GetComponent<PatrolPos> ();
	}


	void FixedUpdate () {
		if (target && !isShaking) {
			PatrolPos pat;
			switch (playerctr.path) {
			case 1:
				pat = pat1R;
				break;
			case 2:
				pat = pat2R;
				break;
			case 3:
				pat = pat3R;
				break;
			default : // playerctr.path == 4;
				pat = pat2L;
				break;
			}
			pre_nodeNum = playerctr.pre_nodeNum;
			next_nodeNum = playerctr.next_nodeNum;

			if (playerctr.path < 4) {
				float slope = (pat.patrolPostition [next_nodeNum].z - pat.patrolPostition [pre_nodeNum].z) / (pat.patrolPostition [next_nodeNum].x - pat.patrolPostition [pre_nodeNum].x);
				slope = -1 / slope;
				Vector3 cameraDir;
				if(pat.patrolPostition[pre_nodeNum].z <= pat.patrolPostition[next_nodeNum].z)
					cameraDir = Vector3.right + Vector3.forward * slope;
				else
					cameraDir = Vector3.left - Vector3.forward * slope;

				cameraDir.Normalize ();

				tr.position = Vector3.Lerp(tr.position, target.position + (cameraDir * dist) + (Vector3.up * height), Time.deltaTime);

			}

			tr.LookAt (target);

		} else if (target && isShaking) {
			originalPos = tr.position;

			if (isShaking && shakes > 0) {
				tr.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
				//tr.position += new Vector3 (0, 0, -50f);
				shakes -= Time.deltaTime * 1.0f;

			} else if (isShaking && shakes <= 0) {

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
