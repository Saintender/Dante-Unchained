using System;
using UnityEngine;

public class GameManager
{
	private static GameManager _instance;
	public static GameManager Instance { get { return _instance ?? (_instance = new GameManager()); } }

	public int Points { get; private set; }

	private GameManager() // because private none other than GameManager can make instance of itself
	{
	} // End empty GameManager constructor

	public void Reset()
	{
		Points = 0;
	} // end Reset

	public void ResetPoints(int points)
	{
		Points = points;
	} // end ResetPoints

	public void AddPoints(int pointsToAdd)
	{
		Points += pointsToAdd;
	} // end AddPoints
} // end GameManager

