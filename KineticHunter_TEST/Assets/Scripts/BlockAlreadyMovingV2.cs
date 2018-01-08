using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlockAlreadyMovingV2 : MonoBehaviour {


float maxEnergie = 200;

public float energie = 0;
public Vector3 direction = new Vector3(0,0,0);

Rigidbody rb;

	Material myMat;

	float _BoundsUp;
	float _BoundsDown;

void Start(){

	direction = Vector3.Normalize(direction);
	rb = GetComponent<Rigidbody>();
	myMat = GetComponent<MeshRenderer> ().material;
	Collider myCol = gameObject.AddComponent<BoxCollider> ();
	_BoundsUp = myCol.bounds.max.y;
	_BoundsDown = myCol.bounds.min.y;

	Destroy (myCol);
}


void Update(){

		myMat.SetFloat ("_BoundsUp", _BoundsUp );
		myMat.SetFloat ("_BoundsDown", _BoundsDown);

		if (energie <= 0) {
			energie = 0;
		} else if (energie > maxEnergie) {
			energie = maxEnergie;
		}

		float energieNew = energie / 20;
		energieNew= Mathf.Log10 (energieNew)*3;

		myMat.SetFloat("_Size1", energieNew);
		myMat.SetFloat("_Size2", energieNew*70/100);

		myMat.SetFloat ("_fillPourcent", energie / maxEnergie*2 -1);


	
		Vector3 velocity = direction * Time.deltaTime * energie;



rb.velocity = velocity;
}



	void OnCollisionEnter(Collision col){

		if (col.gameObject.GetComponent<BlockAlreadyMovingV2> ()) {
			if (energie > maxEnergie / 2) {

			}
		} 
		if (!col.gameObject.GetComponent<CineticGunV2> ()) {
			direction = col.contacts [0].normal.normalized;
		}



	}

}