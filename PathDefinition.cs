using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathDefinition : MonoBehaviour {

	public Transform[] Points;

	public IEnumerator<Transform> GetPathEnumerator() // returns a looping sequence as opposed to a collection
	{
		if (Points == null || Points.Length < 1) // we need at least one point for the sequence to exist
			yield break; // more appropriate for ending a sequence

		var direction = 1; //Which direction are we going?
		var index = 0; //current index observed

		while (true) 
		{
			yield return Points [index]; //this yields execution back to whoever is invoking the enumerator, avoiding CPU stack overflow; this allows invoker to decide whether to move next or not

			if (Points.Length == 1)
				continue;

			if (index <= 0)
				direction = 1;
			else if (index >= Points.Length - 1)
				direction = -1;

			index = index + direction;
		} // terminates when the enumerator caller stops requesting the next element

	} // end IEnumerator

	public void OnDrawGizmos()
	{
		
		if (Points == null || Points.Length < 2)
			return;

		var points = Points.Where(t => t != null).ToList (); 
		if (points.Count < 2)
			return;

		for (var i = 1; i < points.Count; i++) 
		{
			Gizmos.DrawLine(points [i - 1].position, points[i].position);
		}

	} // end OnDrawGizmos

} //end PathDefinition
