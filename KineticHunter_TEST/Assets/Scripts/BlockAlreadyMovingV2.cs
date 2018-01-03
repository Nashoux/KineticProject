using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAlreadyMovingV2 : MonoBehaviour {


float maxEnergie = 200;

public float energie = 0;
public Vector3 direction = new Vector3(0,0,0);

Rigidbody rb;


void Start(){

	direction = Vector3.Normalize(direction);
	rb = GetComponent<Rigidbody>();
}


void Update(){

		if (energie < 0) {
			energie = 0;
		} else if (energie > maxEnergie) {
			energie = maxEnergie;
		}

	
Vector3 velocity = direction*Time.deltaTime*energie;





rb.velocity = velocity;
}



	void OnCollisionEnter(Collision col){

		if (col.gameObject.GetComponent<BlockAlreadyMovingV2> ()) {
			if (energie > maxEnergie / 2) {

			}
		} else if (col.gameObject.GetComponent<CineticGun> ()) {

		}




	}








}
