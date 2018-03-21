using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondAttack : MonoBehaviour {

	int frameCount = 0;
	public bool isActive = false;
	Vector2 moveDir;
	SpriteRenderer render;

	BoxCollider2D colli;
	bool isHit = false;

	// Use this for initialization
	void Start () {
		render = gameObject.GetComponent<SpriteRenderer> ();
		colli = gameObject.GetComponent<BoxCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {


		GameObject.Find ("Player").GetComponent<PlayerCtrl> ().rigid.gravityScale = 0f;
		moveDir = GameObject.Find ("Player").GetComponent<PlayerCtrl> ().moveDir;
		GameObject.Find ("Player").GetComponent<PlayerCtrl> ().isAttacking = true;


		// isActive is controlled in PlayerCtrl.cs
		// If player push down attack key, then isActive set true.
		if (isActive) {

			if (moveDir.x > 0) {
				render.flipX = true;
				colli.offset = new Vector2 (1.4f, -0.48f);
			}
			else {
				render.flipX = false;
				colli.offset = new Vector2 (-1.4f, -0.48f);
			}
			frameCount++;

			// Disactive first attack after 10 frame
			if (frameCount >= 16) {
				frameCount = 0;
				GameObject.Find ("Player").GetComponent<PlayerCtrl> ().rigid.gravityScale = 1f;
				GameObject.Find ("Player").GetComponent<PlayerCtrl> ().isAttacking = false;
				GameObject.Find ("Player").GetComponent<PlayerCtrl> ().isSecondAttack = false;
				gameObject.SetActive (false);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other){

		if (other.gameObject.tag == "Monster") {


			GameObject obj = other.gameObject;

			StartCoroutine (Damage (obj));
		}
	}

	IEnumerator Damage(GameObject obj){

		if (!isHit) {
			isHit = true;

			obj.GetComponent<TestMonster> ().health--;

			isHit = false;
		}



		yield return null;
	}
}
