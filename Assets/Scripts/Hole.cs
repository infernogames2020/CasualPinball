using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
	private Transform cachedTransform;
	private SphereCollider cachedCollider; 
	private void Awake()
	{
		cachedTransform = transform;
		cachedCollider = GetComponent<SphereCollider>();
	}

	private void OnTriggerStay(Collider collider)
	{
		if (collider.tag.Equals("Ball"))
		{
			//Game.Instance.LoadNextLevel();
			var ball = collider.gameObject;
			if ((ball.transform.position - cachedTransform.position).magnitude < cachedCollider.radius)
			{
				ball.GetComponent<Ball>().Sink();
			}
		}
	}

}
