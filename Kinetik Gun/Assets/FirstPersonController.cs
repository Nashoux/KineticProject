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
		[SerializeField] CineticGun myGun;

        // Use this for initialization
        private void Start()
        {
			rb = GetComponent<Rigidbody> ();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;

			m_MouseLook.Init(transform , m_Camera.transform);
        }


        private void Update(){


            RotateView();

			GetInput();

			Vector3 desiredMove = transform.forward*m_Input.y + transform.right*m_Input.x;


			m_MoveDir.x = desiredMove.x*speed*Time.deltaTime;
			m_MoveDir.z = desiredMove.z*speed*Time.deltaTime;
		if ((!grounded && !myGun.isLock) ) {
			rb.useGravity = true;
			//m_MoveDir.y = -Time.deltaTime * 4;
		} else  {
			//
			rb.useGravity = false;
			grounded = false;
		}
			transform.position += m_MoveDir;
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

}