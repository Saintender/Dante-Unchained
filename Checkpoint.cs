using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Checkpoint : MonoBehaviour
{

	private List<IPlayerRespawnListener> _listeners;

	public void Awake()
	{
		_listeners = new List<IPlayerRespawnListener> ();
	} // end Start

	public void PlayerHitCheckpoint()
	{		
		StartCoroutine(PlayerHitCheckpointCo(LevelManager.Instance.CurrentTimeBonus)); // coroutine executed over course of multiple frames
	} // end PlayerHitCheckpoint

	private IEnumerator PlayerHitCheckpointCo(int bonus)
	{
		FloatingText.Show("Checkpoint!", "CheckPointText", new CenteredTextPositioner (.5f));
		yield return new WaitForSeconds(.5f);
		FloatingText.Show(string.Format("+{0} time bonus!", bonus), "CheckPointText", new CenteredTextPositioner(.5f));
	} // end PlayerHitCheckpointCo

	public void PlayerLeftCheckpoint()
	{
		
	} // end PlayerLeftCheckpoint

	public void SpawnPlayer(Player player)
	{
		player.RespawnAt(transform);

		foreach (var listener in _listeners)
			listener.OnPlayerRespawnInThisCheckpoint(this, player);
	} // end SpawnPlayer

	public void AssignObjectToCheckpoint(IPlayerRespawnListener listener)
	{
		_listeners.Add(listener);
	} // end AssignObjectToCheckpoint

} // end Checkpoint


