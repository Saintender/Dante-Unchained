using System;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
	public Player player;
	public Transform ForegroundSprite;
	public SpriteRenderer ForegroundRenderer;
	public Color MaxHealthColor = new Color(255 / 255f, 63 / 255f, 63 / 255f); // green
	public Color MinHealthColor = new Color(64 / 255f, 137 / 255f, 255 / 255f); // red

	public void Update()
	{
		var healthPercent = player.Health / (float) player.MaxHealth; // floating point cast ensures it results between 0 and 1

		ForegroundSprite.localScale = new Vector3(healthPercent, 1, 1);
		ForegroundRenderer.color = Color.Lerp(MaxHealthColor, MinHealthColor, healthPercent);
	} // end Update

}

