using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public AudioClip soundAttack;
	public AudioClip damaged;
	public AudioClip item;
	public AudioClip jump;
	public AudioClip playerDamaged;
	public AudioClip earthQuake;
	public AudioClip die;
	public AudioClip disappearCube;
	public AudioClip appearSphear;
	public AudioClip bossDie;

	AudioSource myAudio;

	public static SoundManager instance;


	void Awake(){

		if (SoundManager.instance == null)
			SoundManager.instance = this;

	}


	// Use this for initialization
	void Start () {

		myAudio = GetComponent<AudioSource> ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayerAttackSound(){
		myAudio.PlayOneShot (soundAttack);
	}

	public void Damaged(){
		myAudio.PlayOneShot (damaged);
	}

	public void Item(){
		myAudio.PlayOneShot (item);
	}

	public void Jump(){
		myAudio.PlayOneShot (jump);
	}

	public void PlayerDamaged(){
		myAudio.PlayOneShot (playerDamaged);
	}

	public void EarthQuake(){
		myAudio.PlayOneShot (earthQuake);
	}

	public void Die(){
		myAudio.PlayOneShot (die);
	}

	public void DisappearCube(){
		myAudio.PlayOneShot (disappearCube);
	}

	public void AppearShpear(){
		myAudio.PlayOneShot (appearSphear);
	}

	public void BossDie(){
		myAudio.PlayOneShot (bossDie);	
	}

}
