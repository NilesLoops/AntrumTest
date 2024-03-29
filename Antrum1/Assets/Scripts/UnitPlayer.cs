using UnityEngine;
using System.Collections;

public class UnitPlayer : Unit {
	
	public float cameraRotX = 0f;
	public float cameraPitchMax = 45f;

	// Use this for initialization
	public override void Start () {
	
		//Screen.showCursor = false;
		
		base.Start ();
	}
	
	// Update is called once per frame
	public override void Update () {
		
		// Rotation
		
		transform.Rotate (0f, Input.GetAxis ("Mouse X") * turnSpeed * Time.deltaTime, 0f);
		
		cameraRotX -= Input.GetAxis ("Mouse Y") * turnSpeed * Time.deltaTime;
		cameraRotX = Mathf.Clamp(cameraRotX, -cameraPitchMax, cameraPitchMax);
		
		
		Camera.main.transform.forward = transform.forward;
		Camera.main.transform.Rotate (cameraRotX, 0f, 0f);
		
		// Movement
		
		move = new Vector3(Input.GetAxis ("Horizontal"), 0f, Input.GetAxis ("Vertical"));
		
		move.Normalize ();
		
		move = transform.TransformDirection (move);
		
		if (Input.GetKey(KeyCode.Space) && control.isGrounded) {
			
			jump = true;
		}
		
		running = Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift);
		
		base.Update ();
	}
}
