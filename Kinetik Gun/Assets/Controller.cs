using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

	[SerializeField] float speed = 5f ;
	[SerializeField] float turnSpeed = 5f;

	Vector3 lastPositionOfMouse;

	Rigidbody rb;


	void Start () {
		
		rb = GetComponent<Rigidbody> ();
		lastPositionOfMouse = Input.mousePosition;

	}
	

	void Update () {

		float h = Input.GetAxis ("Horizontal");
		float f = Input.GetAxis ("Vertical");

		Vector3 myMove = Vector3.Normalize(transform.forward * f);

		Vector3 myMoveLeftAndRight = Vector3.Normalize (transform.right * f);

		Vector3 leftOrRight = ( Input.mousePosition - lastPositionOfMouse)*Time.deltaTime*turnSpeed ;

		leftOrRight = new Vector3 (-leftOrRight.y, leftOrRight.x, 0);
	

		transform.Rotate ( leftOrRight );




		lastPositionOfMouse = Input.mousePosition;

	}
}
