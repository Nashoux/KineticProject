using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAlreadyMovingV2 : MonoBehaviour {




public float energie = 0;
public Vector3 direction = new Vector3(0,0,0);

Rigidbody rb;


void Start(){

	direction = Vector3.Normalize(direction);
	rb = GetComponent<Rigidbody>();
}


void Update(){

if(energie<0){
	energie = 0;
}

	
Vector3 velocity = direction*Time.deltaTime*energie;





rb.velocity = velocity;	
}







}
