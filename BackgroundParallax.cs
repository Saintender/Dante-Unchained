using UnityEngine;

public class BackgroundParallax : MonoBehaviour 
{
	public Transform[] Backgrounds;
	public float ParallaxScale;
	public float ParallaxReductionFactor;
	public float Smoothing;

	private Vector3 _lastPosition; // position of camera in last frame

	public void Start()
	{
		_lastPosition = transform.position;
	}

	public void Update()
	{
		var parallax = (_lastPosition.x - transform.position.x) * ParallaxScale;

		for (var i = 0; i < Backgrounds.Length; i++) 
		{
			var backgroundTargetPosition = Backgrounds [i].position.x + parallax * (i * ParallaxReductionFactor + 1); // adding one avoids a value of 0, which would negate the effect
			Backgrounds[i].position = Vector3.Lerp(
				Backgrounds[i].position, // from 
				new Vector3(backgroundTargetPosition, Backgrounds[i].position.y, Backgrounds[i].position.z), //to
				Smoothing * Time.deltaTime);

			_lastPosition = transform.position;
		}
	} // end Update

}
