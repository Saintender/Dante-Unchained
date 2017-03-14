using System;
using UnityEngine;


public class GiveDamageToPlayer : MonoBehaviour
{
	public  int DamageToGive = 10;

	private Vector2 
		_lastPosition,
		_velocity;

	public void LateUpdate()
	{
		_velocity = (_lastPosition - (Vector2)transform.position) / Time.deltaTime;
		_lastPosition = transform.position;
	} // end LateUpdate

	public void OnTriggerEnter2D(Collider2D other)
	{
		var player = other.GetComponent<Player>();
		if (player == null)
			return;

		player.TakeDamage(DamageToGive);
		var controller = player.GetComponent<CharacterController2D>();
		var totalVelocity = controller.Velocity + _velocity;

		controller.SetForce (new Vector2(
			-1 * Mathf.Sign(totalVelocity.x) * Mathf.Clamp(Mathf.Abs(totalVelocity.x) * 6, 10, 40), // negating knocks them back, not forward
			-1 * Mathf.Sign(totalVelocity.y) * Mathf.Clamp(Mathf.Abs(totalVelocity.y) * 2, 0, 15))); // these numbers could be variables to make them scalable; either way
	}
} // end GiveDamageToPlayer


