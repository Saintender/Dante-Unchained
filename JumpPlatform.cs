using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatform : MonoBehaviour 
{
	public float JumpMagnitude = 20;

	public void ControllerEnter2D(CharacterController2D controller)
	{
		controller.SetVerticalForce(JumpMagnitude);
	}

} // end JumpPlatform
