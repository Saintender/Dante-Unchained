using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	private bool _isFacingRight;
	private CharacterController2D _controller;
	private float _normalizedHorizontalSpeed;

	public float MaxSpeed = 8; // maximum speed possible
	public float SpeedAccelerationOnGround = 10f; // how quickly player goes from not moving to moving, and same with below variable
	public float SpeedAccelerationInAir = 5f; // same, but if in air
	public int MaxHealth = 100;
	public GameObject OuchEffect;

	public int Health { get; private set; }
	public bool IsDead { get; private set; }

	public void Awake()
	{
		_controller = GetComponent<CharacterController2D>();
		_isFacingRight = transform.localScale.x > 0; // we are facing right if we are not flipped; this initializes this value correctly depending on where we start
		Health = MaxHealth;
	} // end Start

	public void Update() // updates every frame
	{
		if (!IsDead)
			HandleInput ();

		var movementFactor = _controller.State.IsGrounded ? SpeedAccelerationOnGround : SpeedAccelerationInAir; // set to 10 or 5

		//Resharper disable ConvertIfStatementToConditionalTernaryExpression

		if (IsDead)
			_controller.SetHorizontalForce(0);
		else
			_controller.SetHorizontalForce(Mathf.Lerp(_controller.Velocity.x, _normalizedHorizontalSpeed * MaxSpeed, Time.deltaTime * movementFactor)); // set x component of player's velocity to linear interpolation between max speed we want and current velocity 
	}

	public void Kill()
	{
		_controller.HandleCollisions = false;
		GetComponent<Collider2D>().enabled = false;
		IsDead = true;
		Health = 0;

		_controller.SetForce(new Vector2(0, 20));
	} // end Kill

	public void RespawnAt(Transform spawnPoint)
	{
		if (!_isFacingRight)
			flip();

		IsDead = false;
		GetComponent<Collider2D>().enabled = true;
		_controller.HandleCollisions = true;
		Health = MaxHealth;

		transform.position = spawnPoint.position;
	} // end RespawnAt

	public void TakeDamage (int damage)
	{
		FloatingText.Show(string.Format("-{0}", damage), "PlayerTakeDamageText", new FromWorldPointTextPositioner (Camera.main, transform.position, 2f, 60f));

		Instantiate (OuchEffect, transform.position, transform.rotation);
		Health -= damage;

		if (Health <= 0)
			LevelManager.Instance.KillPlayer();
	} // end TakeDamage

	private void HandleInput()
	{
		if (Input.GetKey(KeyCode.D)) 
		{
			_normalizedHorizontalSpeed = 1;
			if (!_isFacingRight)
				flip ();
		} 
		else if (Input.GetKey(KeyCode.A)) 
		{
			_normalizedHorizontalSpeed = -1;
			if (_isFacingRight)
				flip ();
		} 
		else 
		{
			_normalizedHorizontalSpeed = 0;
		}

		if (_controller.CanJump && Input.GetKeyDown(KeyCode.Space)) 
		{
			_controller.Jump ();
		}
	} // end HandleInput

	private void flip()
	{
		transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		_isFacingRight = transform.localScale.x > 0;
	} // end flip
}
