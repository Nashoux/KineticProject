using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAlreadyMoving : MonoBehaviour {

	Vector3 positionStart;

	public BlockMove.Force myForce = new BlockMove.Force (new Vector3 (0, 0, 0), new Vector3 (0, 0, 0), 0);

	enum mouvementType {noMouv, yy, xx, zz, xz, zx, xy, yx, yz, zy, xzy,zxy, specific }

	[SerializeField] mouvementType myNewMouv = mouvementType.noMouv;

	[SerializeField] float speed = 1;

	[SerializeField] Vector3 specificPos1;
	[SerializeField] Vector3 specificPos2;
	bool goToSpePos1 = false;

	List<Vector3> AllThePlace = new List<Vector3> ();
	float timerReturn = 5;
	[SerializeField] float timerReturnBase = 5;

	// Use this for initialization
	void Start () {
		positionStart = transform.position;
	}
	
	// Update is called once per frame
	void Update () {


		switch (myNewMouv) {

		case mouvementType.noMouv:
			specificPos1 = positionStart;
			specificPos2 = positionStart;
			myNewMouv = mouvementType.specific;
			break;

		case mouvementType.yy:


			specificPos1 = positionStart;
			specificPos2 = new Vector3 (positionStart.x, positionStart.y + 5f, positionStart.z);
			myNewMouv = mouvementType.specific;
			break;

		case mouvementType.xx:
			specificPos1 = positionStart;
			specificPos2 = new Vector3 (positionStart.x+5f, positionStart.y, positionStart.z);
			myNewMouv = mouvementType.specific;
			break;

		case mouvementType.zz:

			specificPos1 = positionStart;
			specificPos2 = new Vector3 (positionStart.x, positionStart.y, positionStart.z+5f);
			myNewMouv = mouvementType.specific;
			break;

		case mouvementType.xz:
			specificPos1 = positionStart;
			specificPos2 = new Vector3 (positionStart.x+2.5f, positionStart.y, positionStart.z+2.5f);
			myNewMouv = mouvementType.specific;
			break;

		case mouvementType.zx:

			specificPos1 = positionStart;
			specificPos2 = new Vector3 (positionStart.x-2.5f, positionStart.y, positionStart.z+2.5f);
			myNewMouv = mouvementType.specific;
			break;

		case mouvementType.zxy:
			specificPos1 = positionStart;
			specificPos2 = new Vector3 (positionStart.x+2f, positionStart.y+2f, positionStart.z+2f);
			myNewMouv = mouvementType.specific;
			break;

		case mouvementType.xzy:
			specificPos1 = positionStart;
			specificPos2 = new Vector3 (positionStart.x+2f, positionStart.y+2f, positionStart.z-2f);
			myNewMouv = mouvementType.specific;
			break;

		case mouvementType.zy:
			specificPos1 = positionStart;
			specificPos2 = new Vector3 (positionStart.x, positionStart.y+2.5f, positionStart.z+2.5f);
			myNewMouv = mouvementType.specific;
			break;

		case mouvementType.yz:
			specificPos1 = positionStart;
			specificPos2 = new Vector3 (positionStart.x, positionStart.y+2.5f, positionStart.z-2.5f);
			myNewMouv = mouvementType.specific;
			break;

		case mouvementType.xy:
			specificPos1 = positionStart;
			specificPos2 = new Vector3 (positionStart.x+2.5f, positionStart.y-2.5f, positionStart.z);
			myNewMouv = mouvementType.specific;
			break;

		case mouvementType.yx:
			specificPos1 = positionStart;
			specificPos2 = new Vector3 (positionStart.x-2.5f, positionStart.y+2.5f, positionStart.z);
			myNewMouv = mouvementType.specific;
			break;

		case mouvementType.specific:

			if (timerReturn < timerReturnBase) {
				
				transform.position += myForce.direction * myForce.speed * Time.deltaTime;
				timerReturn += Time.deltaTime;
			
			} else {
				
				ReturnToStart ();


				if (!goToSpePos1) {
					if (Vector3.Distance (transform.position, specificPos2) > 0.1f) {
						myForce = new BlockMove.Force(-Vector3.Normalize( transform.position - specificPos2), new Vector3(0,0,0),speed);
						transform.position = Vector3.MoveTowards (transform.position, specificPos2, speed * Time.deltaTime);
					} else {
						goToSpePos1 = true;
					}

				} else {
					if (Vector3.Distance (transform.position, specificPos1) > 0.1f) {
						myForce = new BlockMove.Force(-Vector3.Normalize( transform.position - specificPos1), new Vector3(0,0,0),speed);
						transform.position = Vector3.MoveTowards (transform.position, specificPos1, speed * Time.deltaTime);
					} else {
						goToSpePos1 = false;
					}
				}
			}

			break;

		}

		if(Input.GetKeyDown(KeyCode.R)){
			transform.position = positionStart;
			myForce = new BlockMove.Force (new Vector3 (0, 0, 0), new Vector3 (0, 0, 0), 0);
		}
	}


	void OnCollisionEnter(){
		timerReturn = timerReturnBase + 1;
	}

	void ReturnToStart(){
		if (AllThePlace.Count > 0) {
			transform.position = Vector3.MoveTowards (transform.position, AllThePlace [AllThePlace.Count - 1], myForce.speed*Time.deltaTime);

			if (Vector3.Distance (transform.position, AllThePlace [AllThePlace.Count - 1]) < 0.8f) {
				Debug.Log ("c-2");
				AllThePlace.RemoveAt (AllThePlace.Count - 1);
			}
		}
	}

	public void changeMyForce ( BlockMove.Force _force ){

		timerReturn = 0;

		AllThePlace.Add (transform.position);

		myForce.direction = Vector3.Normalize (_force.direction);

		myForce.speed = _force.speed;

		//transform.rotation = Quaternion.identity;

		//transform.Rotate (_force.orientation);

	}


}
