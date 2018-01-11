﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CineticGunV2 : MonoBehaviour {

	[SerializeField] GameObject ParticulesAspiration;

	[SerializeField] GameObject myGameObbject;

	[SerializeField] private FirstPersonController fpc;

	float myEnergie = 0;

	bool stocked = false;
	LayerMask myMask;

	bool energiseGift = false;
	float energiseGiftTimer = 0.8f;
	bool energiseTake = false;
	float energiseTakeTimer = 0.8f;

	public BlockAlreadyMovingV2 blockLock;
	public GameObject[] myDirectionGo = new GameObject[2];

	[SerializeField] GameObject directionVectorSign;

	GameObject lastParticuleAspiration;

	float lastInputTrigger = 0;

	void Start () {
		//myForces = new BlockMove.Force (new Vector3(1,0,0), new Vector3( transform.rotation.x, transform.rotation.y, transform.rotation.z) , 0.5f);
		myMask = 5;

		//myMask = ~myMask;
	}
	
	void Update ()	{




		#region direction
		//Prendre une force

		if (Input.GetKey (KeyCode.Joystick1Button5) || Input.GetMouseButtonDown (1)) {
			RaycastHit hit; 
			if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, myMask) && hit.collider.GetComponent<BlockAlreadyMovingV2> ()) {

				if (myDirectionGo [0] != null) {
					Destroy (myDirectionGo [0].gameObject);
					Destroy (myDirectionGo [1].gameObject);
				}

				myDirectionGo[0] = (GameObject)Instantiate(myGameObbject);
				myDirectionGo [0].transform.position = new Vector3 (hit.transform.position.x, hit.transform.position.y, hit.transform.position.z);
				myDirectionGo[1] = (GameObject)Instantiate(myGameObbject);
				myDirectionGo [1].transform.position = new Vector3 (hit.transform.position.x + hit.transform.GetComponent<BlockAlreadyMovingV2> ().direction.x, hit.transform.position.y + hit.transform.GetComponent<BlockAlreadyMovingV2> ().direction.y , hit.transform.position.z + hit.transform.GetComponent<BlockAlreadyMovingV2> ().direction.z);


				myDirectionGo [0].transform.parent = transform.GetChild (0).transform;
				myDirectionGo [1].transform.parent = transform.GetChild (0).transform;
			}
		}



		//donner une force	
		float triger2 = Input.GetAxis ("trigger2");

		if (( triger2 > 0.2f  || Input.GetMouseButtonDown (0)) && !stocked) {
			stocked = true;
			RaycastHit hit;
			if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, myMask) &&  hit.collider.GetComponent<BlockAlreadyMovingV2>()  ) {
				
				hit.collider.GetComponent<BlockAlreadyMovingV2> ().direction = Vector3.Normalize( new Vector3 ( myDirectionGo [1].transform.position.x - myDirectionGo [0].transform.position.x,  myDirectionGo [1].transform.position.y - myDirectionGo [0].transform.position.y , myDirectionGo [1].transform.position.z - myDirectionGo [0].transform.position.z));
					
			}
		} else {
			stocked = false;
		}
		#endregion

		#region Energise
		//take
		bool isTackingEnergie = false;
		RaycastHit energiseHit;
		if ( Input.GetKey (KeyCode.Joystick1Button4)|| Input.GetKey (KeyCode.A) ) {
			if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out energiseHit, Mathf.Infinity, myMask) && energiseHit.collider.GetComponent<BlockAlreadyMovingV2> ()) {

				if(energiseHit.collider.GetComponent<BlockAlreadyMovingV2> ().energie>0){

					if (energiseTake && ( Input.GetKeyDown (KeyCode.Joystick1Button4) || Input.GetKeyDown (KeyCode.A) )) {
						myEnergie += energiseHit.collider.GetComponent<BlockAlreadyMovingV2> ().energie;
						energiseHit.collider.GetComponent<BlockAlreadyMovingV2> ().energie = 0;
						lastParticuleAspiration.GetComponent<ParticleSystem>().Emit((int)myEnergie/3);
						isTackingEnergie = false;
					} else {
						if(Input.GetKeyDown (KeyCode.Joystick1Button4)|| Input.GetKeyDown (KeyCode.A) ){

							lastParticuleAspiration = Instantiate<GameObject>(ParticulesAspiration);
							lastParticuleAspiration.GetComponent<particleAttractorLinear>().target = this.transform;
							lastParticuleAspiration.transform.parent = energiseHit.collider.transform;
							lastParticuleAspiration.transform.position = energiseHit.collider.transform.position;

							energiseTake = true;
							energiseTakeTimer = 0.8f;
						}
						isTackingEnergie = true;
						energiseHit.collider.GetComponent<BlockAlreadyMovingV2> ().energie -= 3;
						myEnergie += 3;
					}
				}
			}
		}
		if(energiseTakeTimer > 0){
			energiseTakeTimer -=Time.deltaTime;
		}else{
			energiseTake = false;
		}
		if(isTackingEnergie == false && lastParticuleAspiration != null){
			Debug.Log("a");
			lastParticuleAspiration.GetComponent<ParticleSystem>().Stop();
			Destroy(lastParticuleAspiration.gameObject,10);

		}



	

		//give
		float triger1 = Input.GetAxis ("trigger1");

		if ( triger1 > 0.2f  || Input.GetKey (KeyCode.E)) {
			RaycastHit hit;
			if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, myMask) && hit.collider.GetComponent<BlockAlreadyMovingV2> ()) {
				if(myEnergie>=3){


					if (energiseGift &&  (lastInputTrigger <= 0.09f  || Input.GetKeyDown (KeyCode.E))) {
						
					hit.collider.GetComponent<BlockAlreadyMovingV2> ().energie += myEnergie;
					myEnergie = 0;
					} else {
						if (lastInputTrigger <= 0.09f  || Input.GetKey (KeyCode.E)){
							energiseGift = true;
							energiseGiftTimer = 0.8f;
						}
						hit.collider.GetComponent<BlockAlreadyMovingV2> ().energie += 3;
						myEnergie -= 3;
					}
				}
			}
		}
		if(energiseGiftTimer > 0){
			energiseGiftTimer -=Time.deltaTime;
		}else{
			energiseGift = false;
		}
		lastInputTrigger = triger1;



		#endregion

		#region Lock
		// Système de lock

		if (Input.GetMouseButtonDown (2) || Input.GetKeyDown(KeyCode.JoystickButton9) ) {
			if (blockLock == null) {
				RaycastHit hit; 
				if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, myMask) && hit.collider.GetComponent<BlockAlreadyMovingV2> ()) {
					//rb.useGravity = false;
					blockLock = hit.collider.GetComponent<BlockAlreadyMovingV2> ();
				}

			} else {
				
				blockLock = null;

				RaycastHit hit; 
				if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, myMask) &&  hit.collider.GetComponent<BlockAlreadyMovingV2> ()) {
					blockLock = hit.collider.GetComponent<BlockAlreadyMovingV2> ();
				}
			}
		}
		#endregion

		LookRotationSign ();

	}

	void OnCollisionEnter(){
		delock ();
	}


	void delock(){
		blockLock = null;
	}



	public void LookRotationSign(){
		
		
	//	directionVectorSign.transform.rotation =   Quaternion.Euler ( new Vector3 ((-myForce.direction.y )*180,  (-myForce.direction.z )*180, (myForce.direction.x )*180 ));

	}
	


}
