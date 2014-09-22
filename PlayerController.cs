//
// FPS Player Controller
//

using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	// Add options for sensitivity and y-inversion
	// Assumes forwardSpeed > sideSpeed (I'm not sure an FPS would work very well otherwise)

	public Transform playerCamera;
	private Vector3 playerMovement = Vector3.zero;
	private CharacterController playerController;

	// Keyboard-Movement Modifiers
	// ---

	public float forwardSpeed;
	public float sideSpeed;
	public float jumpModifier;

	// --/

	// Mouse Modifiers
	// ---

	public float pitch;
	public float yaw;
	// Make this customizable later
	public float mouseSensitivity;

	public float minYaw;
	public float maxYaw;

	// Remove default Y-inversion - Make this customizable later with a bool
	private int yInvert = -1;
	private float yInput, xInput;
	private float currentPitch = 0;

	// --/

	void Start ()
	{
		playerController = GetComponent<CharacterController>();
	}

	void FixedUpdate ()
	{
		movePlayer();
		movePlayerMouse();
	}

	void movePlayer()
	{
		playerMovement.Set(Input.GetAxis("Horizontal") * sideSpeed, 
		                   0.0f,
		                   Input.GetAxis("Vertical") * forwardSpeed);

		// Clamped to forwardSpeed so moving in diagonal isn't faster
		playerController.Move(transform.TransformDirection(Vector3.ClampMagnitude(playerMovement, forwardSpeed)));


		if(Input.GetButton("Jump"))
		{
			playerController.Move(Vector3.up * jumpModifier);
			// Must add gravity
		}
	}

	void movePlayerMouse()
	{
		yInput = Input.GetAxisRaw("Mouse Y") * pitch * mouseSensitivity * yInvert;
		xInput = Input.GetAxisRaw("Mouse X") * yaw * mouseSensitivity;

		// Player rotates left and right (and camera with)
		transform.Rotate(Vector3.up * xInput);

		// Only camera rotates vertically
		currentPitch = Mathf.Clamp(currentPitch + yInput, minYaw, maxYaw);
		playerCamera.transform.localEulerAngles = Vector3.right * currentPitch;
	}
}
