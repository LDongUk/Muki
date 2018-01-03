using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerCtrl2 : MonoBehaviour {

	private float hori = 0.0f;

	private Rigidbody rigd;
	private MeshRenderer meshRenderer;
	private CapsuleCollider bounder;
	private List<GameObject> listPrevObstacleObject = new List<GameObject>();
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
	public GameObject attackUpBullet;
	public GameObject nextMap;

	public GameObject[] hp = new GameObject[5];

	float dir = 1.0f;
	bool isAttack = false;
	bool isDelay = false;

	bool isCskill = false;
	bool isDashDelay = false;

	PatrolPos patrol;
	PatrolPos pat1R;
	PatrolPos pat2L;
	PatrolPos pat2R;
	PatrolPos pat3R;

	public int pre_nodeNum, next_nodeNum;
	public int path = 1;

	public bool isPowerUp = false;
	public Vector3 attackDir;


	void awake(){
		DontDestroyOnLoad (this);

	}


	// Use this for initialization
	void Start () {
		rigd = GetComponent<Rigidbody> ();
		meshRenderer = GetComponent<MeshRenderer> ();
		bounder = GetComponent<CapsuleCollider> ();

		for (int i = 0; i < 5; i++)
			meshRenderer.sharedMaterials [i].shader = Shader.Find("Standard");
		
		pat1R = GameObject.Find ("Patrols1_R").GetComponent<PatrolPos> ();
		pat2R = GameObject.Find ("Patrols2_R").GetComponent<PatrolPos> ();
		pat2L = GameObject.Find ("Patrols2_L").GetComponent<PatrolPos> ();
		pat3R = GameObject.Find ("Patrols3_R").GetComponent<PatrolPos> ();
		pre_nodeNum = 0; next_nodeNum = 1;
		patrol = pat1R;
	}

	// Update is called once per frame
	void Update () {

		inputKey ();
		obtacle_unvisible ();

	}

	void FixedUpdate() {

		Move ();
		Attack ();
		Jump ();
		Dash ();
	}

	void inputKey(){
		hori = Input.GetAxis ("Horizontal");

		hori *= moveSpeed;

		moveDir = (Vector3.right * hori );

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
				SceneManager.LoadScene ("Scene3");


		}

	}



	//이동 
	void Move(){

		if (hori == 0)
			return;
		
		if (path == 1) {
			if (pre_nodeNum < 36 && Input.GetButtonDown ("SelectRoad")) {
				path = 3;
				patrol = pat3R;
			} else if (40 <= pre_nodeNum && Input.GetButtonDown ("SelectRoad")) {
				path = 2;
				patrol = pat2R;
				pre_nodeNum -= 19;
				next_nodeNum -= 19;
			}
		} else if (path == 2) {
			if (20 <= pre_nodeNum && Input.GetButtonDown ("SelectRoad")) {
				path = 1;
				patrol = pat1R;
				if (20 <= pre_nodeNum && pre_nodeNum <= 25) {
					pre_nodeNum = 44;
					next_nodeNum = 45;
				} else {
					pre_nodeNum += 19;
					next_nodeNum += 19;
				}
			}

		} else if (path == 3) {
			if (pre_nodeNum < 36 && Input.GetButtonDown ("SelectRoad")) {
				path = 1;
				patrol = pat1R;
			}

			if (59 <= pre_nodeNum && Input.GetButtonDown ("SelectRoad")) {
				path = 4;
				patrol = pat2L;
					pre_nodeNum -= 36;
				next_nodeNum -= 36;
			}
		} else if (path == 4) {
			if (23 <= pre_nodeNum && Input.GetButtonDown ("SelectRoad")) {
				path = 3;
				patrol = pat3R;
				pre_nodeNum += 36;
				next_nodeNum += 36;
			}
		}
	

		//set node number
		if (moveDir.x < 0) {
			if (Vector3.Distance(Vector3.right * transform.position.x, Vector3.right * patrol.patrolPostition [pre_nodeNum].x) <= 0.1f &&
				Vector3.Distance(Vector3.forward * transform.position.z, Vector3.forward * patrol.patrolPostition [pre_nodeNum].z) <= 0.1f) {
				pre_nodeNum = pre_nodeNum - 1;
				if (pre_nodeNum < 0) {
					pre_nodeNum = 0;
				}

			}
		} else {
			if (Vector3.Distance(Vector3.right * transform.position.x, Vector3.right * patrol.patrolPostition [next_nodeNum].x) <= 0.1f &&
				Vector3.Distance(Vector3.forward * transform.position.z, Vector3.forward * patrol.patrolPostition [next_nodeNum].z) <= 0.1f) {
				pre_nodeNum = pre_nodeNum + 1;
				if (pre_nodeNum == patrol.patrolPostition.Count)
					pre_nodeNum = patrol.patrolPostition.Count - 1;
			}
		}
		next_nodeNum = pre_nodeNum + 1;

		//move

		if (hori > 0) {
			Vector3 moveDir = Vector3.right * patrol.patrolPostition [next_nodeNum].x + Vector3.up * transform.position.y + Vector3.forward * patrol.patrolPostition [next_nodeNum].z;
			transform.position = Vector3.MoveTowards (transform.position, moveDir, hori * Time.deltaTime);

		} else if (hori < 0) {
			Vector3 moveDir = Vector3.right * patrol.patrolPostition [pre_nodeNum].x + Vector3.up * transform.position.y + Vector3.forward * patrol.patrolPostition [pre_nodeNum].z;
			transform.position = Vector3.MoveTowards (transform.position, moveDir, -1 * hori * Time.deltaTime);


		}

		//print (pre_nodeNum.ToString ());

		Turn ();
	}


	//회전 
	void Turn(){


		if (hori == 0)
			return;
	
		if (moveDir.x > 0) {
			transform.LookAt (Vector3.right * patrol.patrolPostition [next_nodeNum].x + Vector3.up * transform.position.y + Vector3.forward * patrol.patrolPostition[next_nodeNum].z);
		}

		else if(moveDir.x < 0)
			transform.LookAt (Vector3.right * patrol.patrolPostition [pre_nodeNum].x + Vector3.up * transform.position.y + Vector3.forward * patrol.patrolPostition[pre_nodeNum].z);
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
				attackDir = this.transform.localRotation * Vector3.forward;
				if(!isPowerUp)
					Instantiate (attackBullet, rigd.position + (Vector3.up * 2), rigd.rotation);
				else
					Instantiate (attackUpBullet, rigd.position + (Vector3.up * 3), rigd.rotation);
				//SoundManager.instance.PlaySound ();
				rigd.velocity = Vector3.zero;
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
			if (col.gameObject.tag.Equals ("Enemy") || col.gameObject.tag.Equals ("Boss")) {

				if (col.gameObject.tag.Equals ("Enemy")) {
					playerHealth--;
					hp [playerHealth].SetActive (false);
					SoundManager.instance.PlayerDamaged ();
				}

				else if (col.gameObject.tag.Equals ("Boss")) {
					playerHealth -= 2;
					SoundManager.instance.PlayerDamaged ();
					if (playerHealth >= 0) {
						hp [playerHealth + 1].SetActive (false);
						hp [playerHealth].SetActive (false);
					}

					else
						hp [playerHealth + 1].SetActive (false);
				}


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

				else if (playerHealth <= 0)
					Destroy (this.gameObject);


			}



		}


		if (col.gameObject.tag.Equals ("Heart")) {
			if (playerHealth < 5) {
				hp [playerHealth++].SetActive (true);
				SoundManager.instance.Item ();
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



	void obtacle_unvisible(){
		//ray points
		Vector3 pointCenter = transform.TransformPoint (bounder.center);
//		Vector3 pointLeft = transform.TransformPoint (bounder.center) + new Vector3(bounder.radius, 0, 0);
//		Vector3 pointRight = transform.TransformPoint (bounder.center) - new Vector3(bounder.radius, 0, 0);
//		Vector3 pointUp = transform.TransformPoint (bounder.center) + new Vector3(0 , bounder.height / 2.0f, 0);
//		Vector3 pointDown = transform.TransformPoint (bounder.center) - new Vector3(0 , bounder.height / 2.0f, 0);

		Vector3 pointLeft = transform.TransformPoint (bounder.center) + new Vector3(25, 0, 0);
		Vector3 pointRight = transform.TransformPoint (bounder.center) - new Vector3(25, 0, 0);
		Vector3 pointUp = transform.TransformPoint (bounder.center) + new Vector3(0 , bounder.height / 2.0f, 0);
		Vector3 pointDown = transform.TransformPoint (bounder.center) - new Vector3(0 , bounder.height / 2.0f, 0);


		List<Ray> listRay = new List<Ray> ();

		Vector3 targetPosition = Camera.main.transform.position;

		listRay.Add (new Ray (pointCenter, targetPosition - pointCenter));
		listRay.Add (new Ray (pointLeft, targetPosition - pointLeft));
		listRay.Add (new Ray (pointRight, targetPosition - pointRight));
		listRay.Add (new Ray (pointUp, targetPosition - pointUp));
		listRay.Add (new Ray (pointDown, targetPosition - pointDown));

		List<RaycastHit[]> listHitInfo = new List<RaycastHit[]> ();

		//ray cast
		foreach (Ray ray in listRay) {

			RaycastHit[] hitInfo = Physics.RaycastAll (ray, 1000.0f);
			listHitInfo.Add (hitInfo);

			Debug.DrawRay( ray.origin, ray.direction * 500, Color.red);
		}


		List<GameObject> listNewObstacleObject = new List<GameObject> ();
		for (int i = 0; i < 5; i++) {
			RaycastHit[] listHit = listHitInfo[i];

			foreach (RaycastHit hitInfo in listHit) {

				if ("Wall" != hitInfo.collider.tag)
					continue;

				//Debug.Log (hitInfo.collider.name);
				listNewObstacleObject.Add (hitInfo.transform.gameObject);
			}

		}


		// new 
		foreach( GameObject obstacleObject in listNewObstacleObject )
		{
			// add
			if( !listPrevObstacleObject.Find(delegate( GameObject inObject ){    return (inObject.name == obstacleObject.name ); }))
			{
				// changed to transparent
//				string nameShader = "Transparent/VertexLit";
//
//				MeshRenderer renderer = obstacleObject.GetComponent<MeshRenderer>();
//				renderer.material.shader = Shader.Find(nameShader);
//
//				if(renderer.material.HasProperty("_Color"))
//				{
//					Color prevColor = renderer.material.GetColor("_Color");
//					renderer.material.SetColor("_Color", new Color(prevColor.r, prevColor.g, prevColor.b, 0.0f));
//				}

				StartCoroutine ("UnVisibleObstacle", obstacleObject);

			}
		}
		// prev
		foreach( GameObject obstacleObject in listPrevObstacleObject )
		{
			// remove
			if( !listNewObstacleObject.Find(delegate( GameObject inObject ){    return (inObject.name == obstacleObject.name ); }))
			{
//				// changed to opaque
//				string nameShader = "Standard";
//				MeshRenderer renderer = obstacleObject.GetComponent<MeshRenderer>();
//				renderer.material.shader = Shader.Find(nameShader);
//				Color prevColor = renderer.material.GetColor ("_Color");
//				renderer.material.SetColor("_Color", new Color(prevColor.r, prevColor.g, prevColor.b, 1.0f));

				StartCoroutine ("VisibleObstacle", obstacleObject);
			}
		}
		// swap
		listPrevObstacleObject = listNewObstacleObject;
	}




	bool FindColliderByName(RaycastHit[] inListRayCastInfo, string inName){

		foreach (RaycastHit hitInfo in inListRayCastInfo) {

			if (hitInfo.collider.name == inName)
				return true;

		}

		return false;

	}

	IEnumerator UnVisibleObstacle(GameObject obstacleObject)
	{
		int time;
		float alpha = 1.0f;
		float decrease = 0.1f;

		string nameShader = "Transparent/VertexLit";

		MeshRenderer renderer = obstacleObject.GetComponent<MeshRenderer>();
		renderer.material.shader = Shader.Find(nameShader);

		if(renderer.material.HasProperty("_Color"))
		{
			Color prevColor = renderer.material.GetColor("_Color");
			for (time = 0; time < 10; time++) {
				alpha -= decrease;
				renderer.material.SetColor ("_Color", new Color (prevColor.r, prevColor.g, prevColor.b, alpha));
				yield return new WaitForSeconds (0.01f);
			}
		}

	}


	IEnumerator VisibleObstacle(GameObject obstacleObject)
	{
		int time;
		float alpha = 0.0f;
		float increase = 0.1f;

		MeshRenderer renderer = obstacleObject.GetComponent<MeshRenderer>();
		if (renderer.material.HasProperty ("_Color")) {
			Color prevColor = renderer.material.GetColor ("_Color");
			for (time = 0; time < 10; time++) {
				alpha += increase;
				renderer.material.SetColor ("_Color", new Color (prevColor.r, prevColor.g, prevColor.b, alpha));
				yield return new WaitForSeconds (0.01f);
			}
		}

		string nameShader = "Standard";
		renderer.material.shader = Shader.Find(nameShader);
	}

}
