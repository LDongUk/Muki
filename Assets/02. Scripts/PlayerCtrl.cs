using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCtrl : MonoBehaviour {

	private float hori = 0.0f;

	private Rigidbody rigd;
	private MeshRenderer meshRenderer;
	private Vector3 moveDir;
	public float moveSpeed = 8.0f;

	private bool isJumping = false;
	public float jumpForce = 10.0f;
	private int jumpCount = 0;

	public int playerHealth = 5;
	private bool unBeatTime = false;

	public GameObject attackEffect;
	public GameObject damagedEffect;
	public GameObject attackBullet;
	public GameObject nextMap;


	public GameObject[] hp = new GameObject[5];


	float dir = 1.0f;
	bool isAttack = false;
	bool isDelay = false;

	bool isCskill = false;
	bool isDashDelay = false;

	// Use this for initialization
	void Start () {
		rigd = GetComponent<Rigidbody> ();
		meshRenderer = GetComponent<MeshRenderer> ();

		for (int i = 0; i < 5; i++)
			meshRenderer.sharedMaterials [i].shader = Shader.Find("Standard");

	}
	
	// Update is called once per frame
	void Update () {
		



		hori = Input.GetAxis ("Horizontal");


		moveDir = Vector3.right * hori * moveSpeed * Time.deltaTime;

		if (Input.GetButtonDown ("Jump")) {
			//2 jump
			if (jumpCount < 2) {
				isJumping = true;
			}
		}


		if (rigd.velocity.y < -25) {
			float fallSpeed = -25 - rigd.velocity.y;

			rigd.velocity += Vector3.up * fallSpeed;
		}

		if (moveDir.x > 0)
			dir = 1.0f;
		else if (moveDir.x < 0)
			dir = -1.0f;
		else {
		}
			
			

		if (Input.GetButtonDown ("Attack")) 
			isAttack = true;

		if (Input.GetButtonDown ("C_Skill"))
			isCskill = true;

		if (Input.GetButtonDown ("Portal")) {
			if (Vector3.Distance (transform.position, nextMap.transform.position) < 3)
				SceneManager.LoadScene ("Scene2");


		}


	}

	void FixedUpdate() {
		
		Move ();
		Attack ();
		Jump ();
		Dash ();
	}

	//이동 
	void Move(){
		
		Turn ();
		rigd.MovePosition (transform.position + moveDir);

	}

	//회전 
	void Turn(){

		if (moveDir == Vector3.zero)
			return;

		Quaternion rot = Quaternion.LookRotation (moveDir);
		rigd.MoveRotation (rot);
	}

	//점프
	void Jump(){
		if (isJumping == false)
			return;
		SoundManager.instance.Jump ();
		jumpCount++;
		rigd.AddForce (Vector3.up * jumpForce, ForceMode.Impulse);
		rigd.velocity = Vector3.zero;
		isJumping = false;
	}

	//일반 공격
	void Attack(){
		if (!isDelay) {
			if (isAttack) {
				Instantiate (attackBullet, rigd.position + (Vector3.right * dir * 3) + (Vector3.up * 2), rigd.rotation);
				isDelay = true;
				StartCoroutine ("attackDelay");
			}

		}

		isAttack = false;
	}

	IEnumerator attackDelay(){
		yield return new WaitForSeconds (0.5f);
		isDelay = false;
	}

	//대시
	void Dash(){

		if (!isDashDelay) {
			if (isCskill) {
//				for (int i = 0; i < 10; i++) {
//					rigd.MovePosition (transform.position + (transform.forward * i));
//				}
//
				//rigd.MovePosition (transform.position + transform.forward * 20.0f);
				rigd.AddForce(Vector3.right * dir * 500.0f);

				isDashDelay = true;
				StartCoroutine ("dashDelay");
			}
		}

		isCskill = false;
	}

	IEnumerator dashDelay(){

		yield return new WaitForSeconds (0.7f);
		isDashDelay = false;
	}


	void OnCollisionEnter(Collision col){

		//땅인지 체크
		if(col.gameObject.tag.Equals("Land")){
			jumpCount = 0;

		}


		//피격 & 무적
		if (unBeatTime == false) {
			if (col.gameObject.tag.Equals ("Enemy")) {
				playerHealth--;

				Camera.main.GetComponent<FollowCam1>().ShakeCamera(0.2f);
				SoundManager.instance.PlayerDamaged ();
				print (playerHealth.ToString());
				hp [playerHealth].SetActive (false);
				rigd.velocity = Vector3.zero;
				rigd.AddForce (Vector3.up * 15.0f, ForceMode.Impulse);
				jumpCount = 1; // 피격시 점프 x (2단 점프 가능시 점프 한번 가능)

				if(damagedEffect != null)
					Instantiate (damagedEffect, rigd.position + (Vector3.up * 3), Quaternion.Euler(Vector3.left * 90));

				//StartCoroutine ("TimePause");


				if (playerHealth > 0) {
					unBeatTime = true;
					StartCoroutine ("UnBeatTime");
				}

				else if (playerHealth == 0)
					Destroy (this.gameObject);


			}
		}


		if (col.gameObject.tag.Equals ("Heart")) {
			if (playerHealth < 5) {
				hp [playerHealth++].SetActive (true);

			}

		}


	}
		

	//무적시간
	IEnumerator UnBeatTime()
	{
		int time = 0;
		int i;

		Shader s1 = Shader.Find ("Standard");
		Shader s2 = Shader.Find ("Legacy Shaders/Bumped Diffuse");


		while (time < 10) {
			
			if (time % 2 == 0) {
				for (i = 0; i < 5; i++)
					meshRenderer.sharedMaterials [i].shader = s1;
				
			} 

			else {
				for (i = 0; i < 5; i++)
					meshRenderer.sharedMaterials [i].shader = s2;
		}

			yield return new WaitForSeconds (0.2f);
			time++;
		}

		for (i = 0; i < 5; i++) {
			meshRenderer.sharedMaterials [i].shader = s1;

		}
	

		unBeatTime = false;
	}


	IEnumerator TimePause(){
		Time.timeScale = 0.5f;

		yield return new WaitForSecondsRealtime (0.2f);

		Time.timeScale = 0;
		yield return new WaitForSecondsRealtime (0.5f);
		Time.timeScale = 1;
	}



}
