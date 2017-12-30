using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CineticGunV2 : MonoBehaviour {

	[SerializeField] GameObject myGameObbject;

	[SerializeField] private FirstPersonController fpc;

	float myEnergie = 0;

	bool stocked = false;
	LayerMask myMask;

	Quaternion myVectorRot;
	Rigidbody rb;
	public bool isLock = false;

	public GameObject[] myDirectionGo = new GameObject[2];

	[SerializeField] GameObject directionVectorSign;

	void Start () {
		//myForces = new BlockMove.Force (new Vector3(1,0,0), new Vector3( transform.rotation.x, transform.rotation.y, transform.rotation.z) , 0.5f);
		myMask = 5;
		rb = GetComponent<Rigidbody> ();

		//myMask = ~myMask;
	}
	
	void Update ()	{

		myVectorRot = transform.rotation;


		//Prendre une force

		float triger2 = Input.GetAxis ("trigger2");

		if (triger2 > 0.2f || Input.GetMouseButtonDown (1)) {
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

		float triger1 = Input.GetAxis ("trigger1");

		if ((triger1 > 0.2f || Input.GetMouseButtonDown (0)) && !stocked) {
			stocked = true;
			RaycastHit hit;
			if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, myMask) &&  hit.collider.GetComponent<BlockAlreadyMovingV2>()  ) {


				hit.collider.GetComponent<BlockAlreadyMovingV2> ().direction = Vector3.Normalize( new Vector3 ( myDirectionGo [1].transform.position.x - myDirectionGo [0].transform.position.x,  myDirectionGo [1].transform.position.y - myDirectionGo [0].transform.position.y , myDirectionGo [1].transform.position.z - myDirectionGo [0].transform.position.z));
							
				
				
			}
		} else if (triger1 < 0.2f) {
			stocked = false;
		}


		//Energize

		//take
		if (Input.GetKey (KeyCode.Joystick1Button4)) {
			RaycastHit hit;
			if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, myMask) && hit.collider.GetComponent<BlockAlreadyMovingV2> ()) {
				if(hit.collider.GetComponent<BlockAlreadyMovingV2> ().energie>=0){
					hit.collider.GetComponent<BlockAlreadyMovingV2> ().energie -= 3;
					myEnergie += 3;
				}
			}
		}

		//give
		if (Input.GetKey (KeyCode.Joystick1Button5)) {
			RaycastHit hit;
			if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, myMask) && hit.collider.GetComponent<BlockAlreadyMovingV2> ()) {
				if(myEnergie>=0){
					hit.collider.GetComponent<BlockAlreadyMovingV2> ().energie += 3;
					myEnergie -= 3;
				}
			}
		}



		// Système de lock

		if (Input.GetMouseButtonDown (2) || Input.GetKeyDown(KeyCode.JoystickButton9) ) {
			if (transform.parent == null) {
				RaycastHit hit; 
				if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, myMask) && hit.collider.GetComponent<BlockAlreadyMovingV2> ()) {
					//rb.useGravity = false;
					isLock = true;
					Quaternion rotation = Quaternion.Euler( transform.rotation.eulerAngles);

					transform.SetParent(hit.transform,true);

					rotation = Quaternion.Euler( rotation.eulerAngles -transform.parent.transform.rotation.eulerAngles);

					Debug.Log (rotation.eulerAngles);

					transform.rotation = rotation;
					fpc.m_MouseLook.m_CharacterTargetRot = Quaternion.identity;
					fpc.m_MouseLook.m_CharacterTargetRot = rotation;
					transform.position += new Vector3 (0, 0.001f, 0);
				}

			} else {
				
				Quaternion rotation = Quaternion.Euler( transform.rotation.eulerAngles);
				transform.SetParent( null, true );
				isLock = false;
				fpc.m_MouseLook.m_CharacterTargetRot = rotation;

				RaycastHit hit; 
				if (Physics.Raycast (transform.position, Camera.main.transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, myMask) && (hit.collider.GetComponent<BlockMove> () || hit.collider.GetComponent<BlockAlreadyMovingV2> ())) {
					transform.SetParent(hit.transform,true);
					rotation = Quaternion.Euler( rotation.eulerAngles - transform.parent.transform.rotation.eulerAngles);
					isLock = true;
					fpc.m_MouseLook.m_CharacterTargetRot = rotation;
					transform.position += new Vector3 (0, 0.001f, 0);
				}
			}
		}

		LookRotationSign ();

	}

	void OnCollisionEnter(){
		delock ();
	}


	void delock(){
		Quaternion rotation = Quaternion.Euler( transform.rotation.eulerAngles);
		transform.SetParent( null, true );
		isLock = false;
		fpc.m_MouseLook.m_CharacterTargetRot = rotation;
	}



	public void LookRotationSign(){
		
		
	//	directionVectorSign.transform.rotation =   Quaternion.Euler ( new Vector3 ((-myForce.direction.y )*180,  (-myForce.direction.z )*180, (myForce.direction.x )*180 ));

	}
	


}
