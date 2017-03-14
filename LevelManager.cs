using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class LevelManager : MonoBehaviour
{
	public static LevelManager Instance { get; private set; }

	public Player  Player { get; private set; }
	public CameraController Camera { get; private set; }
	public TimeSpan RunningTime { get { return DateTime.UtcNow - _started; } }

	public int CurrentTimeBonus
	{
		get 
		{
			var secondDifference = (int)(BonusCutoffSeconds - RunningTime.TotalSeconds);
			return Mathf.Max(0, secondDifference) * BonusSecondMultiplier;
		}	
	}

	private List<Checkpoint> _checkpoints;
	private int _currentCheckpointIndex;
	private DateTime _started;
	private int _savedPoints;

	public Checkpoint DebugSpawn;
	public int BonusCutoffSeconds; // time where player can still receive a bonus
	public int BonusSecondMultiplier; // how many seconds left over * x points should be your bonus


	public void Awake()
	{
		Instance = this;
	} // end Awake

	public void Start()
	{
		_checkpoints = FindObjectsOfType<Checkpoint>().OrderBy(t => t.transform.position.x).ToList();
		_currentCheckpointIndex = _checkpoints.Count > 0 ? 0 : -1; // if there are more than 0 checkpoints, set the checkpoint index to 0; otherwise there are no checkpoints in the scene and we don't need this behavior

		//Following code caches these variables locally to LevelManager
		Player = FindObjectOfType<Player>();
		Camera = FindObjectOfType<CameraController>();

		_started = DateTime.UtcNow; // when we started current checkpoint

		var listeners = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerRespawnListener>();
		foreach (var listener in listeners) 
		{
			for (var i = _checkpoints.Count - 1; i >= 0; i--) 
			{
				var distance = ((MonoBehaviour)listener).transform.position.x - _checkpoints [i].transform.position.x;
				if (distance < 0)
					continue;

				_checkpoints[i].AssignObjectToCheckpoint(listener);
				break;
			}
		}

#if UNITY_EDITOR
		if (DebugSpawn != null) // if we've set a debug spawn, tell that to spawn player
			DebugSpawn.SpawnPlayer(Player);
		else if (_currentCheckpointIndex != -1) // if index is valid, then tell that checkpoint to spawn our player
			_checkpoints[_currentCheckpointIndex].SpawnPlayer(Player);
#else
		if (_currentCheckpointIndex != -1)
			_checkpoints[_currentCheckpointIndex].SpawnPlayer(Player);
#endif

	} // end Start

	public void Update()
	{
		var isAtLastCheckpoint = _currentCheckpointIndex + 1 >= _checkpoints.Count;
		if (isAtLastCheckpoint)
			return;

		var distanceToNextCheckpoint = _checkpoints[_currentCheckpointIndex + 1].transform.position.x - Player.transform.position.x;
		if (distanceToNextCheckpoint >= 0) // if we haven't hit next checkpoint yet, early exit from method
			return;

		_checkpoints[_currentCheckpointIndex].PlayerLeftCheckpoint();
		_currentCheckpointIndex++;
		_checkpoints[_currentCheckpointIndex].PlayerHitCheckpoint();

		GameManager.Instance.AddPoints(CurrentTimeBonus);
		_savedPoints = GameManager.Instance.Points; //caches current amount of points in case player dies
		_started = DateTime.UtcNow;
	} // end Update

	public void KillPlayer()
	{
		StartCoroutine(KillPlayerCo());
	} // end KillPlayer

	private IEnumerator KillPlayerCo()
	{
		Player.Kill();
		Camera.IsFollowing = false;
		yield return new WaitForSeconds(2f);

		Camera.IsFollowing = true;

		if (_currentCheckpointIndex != -1) // if the player has hit a checkpoint
			_checkpoints [_currentCheckpointIndex].SpawnPlayer(Player); // checkpoint will spawn player

		_started = DateTime.UtcNow;
		GameManager.Instance.ResetPoints(_savedPoints);
	} // end KillPlayerCo

} // end LevelManager

