using FuriousPlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
	private Transform cachedTransform;
	private SphereCollider cachedCollider;
	private bool ballHasSunk;
	private void Awake()
	{
		cachedTransform = transform;
		cachedCollider = GetComponent<SphereCollider>();
	}

	private void GoToNextLevel()
	{
		ActionManager.TriggerEvent(GameEvents.LOAD_NEXT);
	}

	private void Reset()
	{
		ballHasSunk = false;
	}


	private void OnTriggerEnter(Collider collider)
	{
		if (collider.tag.Equals("Ball") && !ballHasSunk)
		{
			var ball = collider.gameObject.GetComponent<Ball>();
			var direction = (cachedTransform.position - ball.transform.position);
			if (direction.magnitude < (cachedCollider.radius + ball.cachedCollider.radius) * 0.8f)
			{
				ball.Sink(direction.normalized);
				ballHasSunk = true;
				Invoke("GoToNextLevel", 2);
			}
		}
	}

	private void OnTriggerStay(Collider collider)
	{
		if (collider.tag.Equals("Ball"))
		{
			var ball = collider.gameObject.GetComponent<Ball>();

			var direction = (cachedTransform.position - ball.transform.position);
			if (direction.magnitude < (cachedCollider.radius + ball.cachedCollider.radius) * 0.8f)
			{
				ballHasSunk = true;
				ball.Sink(direction.normalized);
				Invoke("GoToNextLevel", 2);
			}
		}
	}

}
