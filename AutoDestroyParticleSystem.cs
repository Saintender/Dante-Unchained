using System;
using UnityEngine; 

public class AutoDestroyParticleSystem : MonoBehaviour
{
	private ParticleSystem _particleSystem;

	public void Start()
	{
		_particleSystem = GetComponent<ParticleSystem>();
	} // end Start

	public void Update()
	{
		if (_particleSystem.isPlaying)
			return;

		Destroy(gameObject); // once it's done, that system is cleaned up
	} // end Update
} // end AutoDestroyParticleSystem


