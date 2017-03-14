using System;
using UnityEngine;

public class FromWorldPointTextPositioner : IFloatingTextPositioner
{
	private readonly Camera _camera;
	private readonly Vector3 _worldPosition;
	private readonly float _speed;
	private float _timeToLive;
	private float _yOffSet;

	public FromWorldPointTextPositioner(Camera camera, Vector3 worldPosition, float timeToLive, float speed) // main camera determination, where in the world it appears, how long it's there(s), how fast text goes up
	{
		_camera = camera;
		_worldPosition = worldPosition;
		_timeToLive = timeToLive;
		_speed = speed;
	} // end constructor

	public bool GetPosition(ref Vector2 position, GUIContent content, Vector2 size)
	{
		if ((_timeToLive -= Time.deltaTime) <= 0) // indicates to floating text that it's time to destroy the floating text
			return false;

		var screenPosition = _camera.WorldToScreenPoint(_worldPosition);
		position.x = screenPosition.x - (size.x / 2);
		position.y = Screen.height - screenPosition.y - _yOffSet;

		_yOffSet += Time.deltaTime * _speed;
		return true;
	} // end GetPosition
} // end FromWorldPointTextPositioner

