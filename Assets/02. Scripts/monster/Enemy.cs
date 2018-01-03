using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent))]
public class Enemy : LivingEntity {

	private Transform player;
	private Transform enemy;
	private NavMeshAgent nma;
	private Vector3 initMoveDirection = Vector3.left;
	private Rigidbody rd;

	public float moveSpeed = 5.0f;
	public float rightBdd = 10;
	public float leftBdd = -10;
	public float detectRange = 10;
	public float detectHeight = 6;

	public int monsterHealth = 3;
	private bool unBeatTime = false;

	// Use this for initialization
	void Start () {
		nma = GetComponent<NavMeshAgent> ();
		player = GameObject.Find ("Muki").GetComponent<Transform> ();
		enemy = GetComponent<Transform> ();
		nma.speed = moveSpeed;
		rd = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {

		float dist = 0.0f;
		float height = 0.0f;
		if (player) {
			dist = Vector3.Distance (player.position, enemy.position);
			height = Mathf.Abs ((player.position - enemy.position).y);

			if (dist <= detectRange && height < detectHeight)
				StartCoroutine ("Chase");
			
			else {
				StartCoroutine ("Move");
				nma.ResetPath ();
			}
		}
		//nma.destination = player.position;

	}

	IEnumerator Chase(){

		//if (dist < 5.0) {
		nma.SetDestination (player.position);
		yield return new WaitForSeconds(.1f);
		//}
	}

	IEnumerator Move(){

		if (enemy.position.x <= leftBdd ) {
			initMoveDirection = Vector3.right;
		}
			
		else if (enemy.position.x >= rightBdd)
			initMoveDirection = Vector3.left;


		nma.Move (initMoveDirection * moveSpeed * Time.deltaTime);
		yield return new WaitForSeconds(.1f);

	}

	void OnCollisionEnter(Collision col){
		rd.velocity = Vector3.zero;
		if (!unBeatTime) {
			if (col.gameObject.tag.Equals ("Bullet")) {
				monsterHealth--;

				SoundManager.instance.Damaged ();
				Camera.main.GetComponent<FollowCam1>().ShakeCamera(0.1f);


				if (monsterHealth > 0) {
					unBeatTime = true;
					StartCoroutine ("UnBeatTime");
				} else if (monsterHealth == 0) {

//					Destroy (gameObject);

					Time.timeScale = 0.1f;
					Invoke ("UnPause", 0.02f);

				}

			}
		}
	}

	//무적시간
	IEnumerator UnBeatTime()
	{
		int time = 0;

		while (time < 5) {

			yield return new WaitForSeconds (0.05f);
			time++;
		}
			

		unBeatTime = false;
	}

	void UnPause(){
		Time.timeScale = 1;
		Destroy (gameObject);
	}




}
