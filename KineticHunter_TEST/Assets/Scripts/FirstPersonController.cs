using System;
using UnityEngine;
//using UnityStandardAssets.CrossPlatformInput;
//using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;


	[RequireComponent(typeof (Rigidbody))]
    public class FirstPersonController : MonoBehaviour
    {
		[SerializeField] private float speed;
        public MouseLook m_MouseLook;


        private Camera m_Camera;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private Vector3 m_OriginalCameraPosition;
		private Rigidbody rb;

		public bool grounded = false;
		[SerializeField] CineticGunV2 myGun;

		[SerializeField] Vector3 newVelocity;

        // Use this for initialization
        private void Start(){
		
			rb = GetComponent<Rigidbody> ();

            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;

			m_MouseLook.Init(transform , m_Camera.transform);
        }


        private void Update(){


            RotateView();

			GetInput();

			Vector3 desiredMove = transform.forward*m_Input.y + transform.right*m_Input.x;


			m_MoveDir.x = desiredMove.x*speed*Time.deltaTime*85;
			m_MoveDir.z = desiredMove.z*speed*Time.deltaTime*85;
		if ((!grounded /*&& !myGun.isLock */) ) {
			rb.useGravity = true;
			//m_MoveDir.y = -Time.deltaTime * 4;
		} else  {
			//
			rb.useGravity = false;
			grounded = false;
		}

		if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Space) ){
			rb.AddForce(0,100,0,ForceMode.Impulse);
		}
		if(myGun.blockLock != null){
			rb.useGravity = false;
			rb.velocity = new Vector3(m_MoveDir.x+newVelocity.x+myGun.blockLock.direction.x*myGun.blockLock.energie*Time.deltaTime, newVelocity.y/100+myGun.blockLock.direction.y*myGun.blockLock.energie*Time.deltaTime, m_MoveDir.z+newVelocity.z +myGun.blockLock.direction.z*myGun.blockLock.energie*Time.deltaTime);
		}else{
			rb.useGravity = true;
			rb.velocity = new Vector3(m_MoveDir.x+newVelocity.x, rb.velocity.y+newVelocity.y/100, m_MoveDir.z+newVelocity.z );   
		}
	}
       

        private void FixedUpdate(){
		     m_MouseLook.UpdateCursorLock();
        }


        private void GetInput(){
            // Read input
			float horizontal = Input.GetAxis("Horizontal");
			float vertical = Input.GetAxis("Vertical");


            // set the desired speed to be walking or running
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1){
                m_Input.Normalize();
            }           
        }

//	void OnCollisionEnter(Collision col){
//		Debug.Log (col.contacts [0].normal);
//		if (col.contacts [0].normal == new Vector3 (0, 1, 0)) {
//			grounded = true;
//		} else {
//			grounded = false;
//		}
//	}


        private void RotateView()
        {
            m_MouseLook.LookRotation (transform, m_Camera.transform);
        }

	void OnCollisionStay(Collision col){

		if (col.contacts [0].normal.y > -0.1f) {
			grounded = true;
		}
		if (col.gameObject.GetComponent<BlockAlreadyMovingV2> ()) {
			newVelocity = Vector3.Normalize( col.gameObject.GetComponent<BlockAlreadyMovingV2> ().direction)/1.5f * Time.deltaTime * col.gameObject.GetComponent<BlockAlreadyMovingV2> ().energie;
		} else {
			newVelocity = new Vector3 (0, 0, 0);
		}

	}

	void OnCollisionExit(){
		newVelocity = new Vector3 (0, 0, 0);
		grounded = false;
	}


}