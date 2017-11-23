using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CineticGun : MonoBehaviour {

	BlockMove.Force myForces;

	bool stocked = false;
	LayerMask myMask;

	Quaternion myVectorRot;
	Rigidbody rb;

	void Start () {
		//myForces = new BlockMove.Force (new Vector3(1,0,0), new Vector3( transform.rotation.x, transform.rotation.y, transform.rotation.z) , 0.5f);
		myMask = 5;
		rb = GetComponent<Rigidbody> ();

		//myMask = ~myMask;
	}
	
	void Update ()	{

		myVectorRot = transform.rotation;

		if (Input.GetKeyDown (KeyCode.Joystick1Button5) || Input.GetMouseButtonDown (1)) {
			RaycastHit hit; 
			if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, myMask) && (hit.collider.GetComponent<BlockMove> () || hit.collider.GetComponent<BlockAlreadyMoving> ())) {
			
				if (hit.collider.GetComponent<BlockMove> ()) {
					myForces = hit.collider.GetComponent<BlockMove> ().myForce;
				} else {
					myForces = hit.collider.GetComponent<BlockAlreadyMoving> ().myForce;
				}			
			}
		}

//		if(Input.GetKeyDown(KeyCode.Joystick1Button4)){
//
//			RaycastHit hit; 
//			if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection(Vector3.forward),  out hit, Mathf.Infinity, myMask) && (hit.collider.GetComponent<BlockMove>() || hit.collider.GetComponent<BlockAlreadyMoving>())) {
//
//				float result = Mathf.InverseLerp (0.9f, 9000, hit.distance);
//				result = Mathf.Lerp (0.90f, 10, result);
//			
//				myForces.Add(hit.collider.GetComponent<BlockMove> ().myForce);
//
//				BlockMove.Force intermediaryForce = myForces [myForces.Count - 1];
//				intermediaryForce.speed /= result;
//				myForces [myForces.Count - 1] = intermediaryForce;
//			}
//
//		}

		float triger1 = Input.GetAxis ("trigger1");

		if ((triger1 > 0.2f || Input.GetMouseButtonDown (0)) && !stocked) {
			stocked = true;
			RaycastHit hit; 
			if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, myMask) && ( hit.collider.GetComponent<BlockMove> () ||  hit.collider.GetComponent<BlockAlreadyMoving>()  )) {
				if (hit.collider.GetComponent<BlockMove> ()) {
					hit.collider.GetComponent<BlockMove> ().changeMyForce (myForces);
				} else {
					hit.collider.GetComponent<BlockAlreadyMoving> ().changeMyForce (myForces);
				}
			}
		} else if (triger1 < 0.2f) {
			stocked = false;

		}

		if (Input.GetMouseButtonDown (2)) {
			if (transform.parent == null) {
				RaycastHit hit; 
				if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, myMask) && (hit.collider.GetComponent<BlockMove> () || hit.collider.GetComponent<BlockAlreadyMoving> ())) {
					Quaternion rotation = transform.rotation;
					rb.useGravity = false;
					Debug.Log (rotation.eulerAngles);

					transform.SetParent(hit.transform,true);

					transform.rotation = rotation;
					//transform.localRotation = Quaternion.identity;
				}

			} else {
				Quaternion rotation = transform.rotation;
				transform.SetParent( null, true );
				rb.useGravity = true;

				RaycastHit hit; 
				if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, myMask) && (hit.collider.GetComponent<BlockMove> () || hit.collider.GetComponent<BlockAlreadyMoving> ())) {
					transform.SetParent(hit.transform,true);
					rb.useGravity = false;
				
				}
			}
		}

	}

	void LateUpdate (){
		
		//transform.rotation = myVectorRot;

		}
	


}
